using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbitable : MonoBehaviour
{

  public SpriteRenderer FaceSpriteRenderer;

  [HideInInspector]
  public Transform playerPos;
  private float radius;
  private float rotationSpeed;

  public ParticleSystem ballParticle;
  public ParticleSystem trailParticle;

  public ParticleSystem fullExplosion;
  public ParticleSystem explosionParticle;
  public ParticleSystem explosionGlowParticle;
  public ParticleSystem destroySphereParticle;

  public ParticleSystem usingItemParticle;
  [HideInInspector]
  public float Phase
  {
    get { return _phase; }
    set
    {
      StartCoroutine(PhaseLerp(value));
    }
  }

  private float _phase;
  // Use this for initialization

  private float randomSeed;
  private float noiseVelocity;
  private float noiseAmplitude;
  private float sphereY;

  private bool isMovingToWitch = false;
  private bool isBeingBlocked = false;

  public bool IsControllingWitch = false;

  private Sprite itemSprite;
  private Sprite faceSprite;

  public float cdTime;

  private Vector3 startAnimationPos;

  [Header("ParticleTimers")]
  public float desapearSphereTime;

  public AnimationCurve MoveToWitchCurve;
  public AnimationCurve ItemOnWitchCurve;

  private OrbitManager orbitManager;
  private bool isInCD = false;

  private float faceTime;
  void Start()
  {
    orbitManager = OrbitManager.Instance;
    FaceSpriteRenderer.color = Color.white;
    faceSprite = FaceSpriteRenderer.sprite;
    GetComponent<Rigidbody>().isKinematic = true;
    radius = orbitManager.Radius;
    rotationSpeed = orbitManager.RotationSpeed;
    noiseVelocity = orbitManager.NoiseVelocity;
    noiseAmplitude = orbitManager.NoiseAmplitude;
    sphereY = orbitManager.SphereY;
    playerPos = orbitManager.playerPos;

    randomSeed = Random.Range(0f, 1f);
    this.transform.localScale = Vector3.zero;
    StartCoroutine(SpawnOrb(orbitManager.orbSpawnTimer));
  }

  // Update is called once per frame
  void Update()
  {
    SwitchFaceSprite();

    if (!isMovingToWitch && !isBeingBlocked)
    {
      var obtTransform = gameObject.transform;
      obtTransform.position = playerPos.position + new Vector3(Noise(radius) * Mathf.Cos(rotationSpeed * Time.time + _phase), sphereY, radius * Mathf.Sin(rotationSpeed * Time.time + _phase));
    }
    if (cdTime + orbitManager.orbSpawnTimer >= GameDataManager.Data.SwitchCooldown && isInCD && !IsControllingWitch)
    {
      StartCoroutine(SpawnOrb(orbitManager.orbSpawnTimer));
      isInCD = false;
    }
    cdTime += Time.deltaTime;
    faceTime += Time.deltaTime;
  }

  void SwitchFaceSprite()
  {
    if (itemSprite)
    {
      if (faceTime <= 0.5f)
      {
        FaceSpriteRenderer.sprite = faceSprite;
      }
      else
      {
        FaceSpriteRenderer.sprite = itemSprite;
      }
    }
    if (faceTime > 2)
    {
      faceTime = 0;
    }
  }
  public void SetItemOnSphere(Sprite sp)
  {
    itemSprite = sp;

  }
  public void UseItemEffect()
  {
    itemSprite = null;
    StartCoroutine(UseCurseOnWitch(playerPos, 0.5f));
    StartCoroutine(KeepLokingAt(playerPos, usingItemParticle.main.duration));

  }

  IEnumerator KeepLokingAt(Transform obj, float duration)
  {
    float time = 0f;
    while (time <= duration)
    {
      time += Time.deltaTime;
      this.gameObject.transform.LookAt(obj);
      yield return null;

    }
  }

  public void MoveToWitch()
  {
    StartCoroutine(MoveToWitch(playerPos, OrbitManager.Instance.orbControlAnimationTimer));
  }

  public void GetWitchControl()
  {
    StartCoroutine(GettingControl());
  }

  public void GetBlockedByWitch()
  {
    StartCoroutine(BlockedAnimation(desapearSphereTime));
  }

  float Noise(float radius)
  {
    float value = Mathf.PerlinNoise(Time.time * noiseVelocity, randomSeed);
    value = (value - 0.5f) * 2f;
    return radius + noiseAmplitude * value;

  }

  [ContextMenu("Set Phase")]
  void ChangePhase()
  {
    Phase = TargetPhase;
  }
  public float TargetPhase;

  IEnumerator PhaseLerp(float targetPhase)
  {
    //print(targetPhase);
    var time = 1f;

    return CoroutineHelpers.InterpolateByTime(time, (k =>
    {
      //print(_phase);
      _phase = Mathf.Lerp(0, targetPhase, k);
    }));

  }

  IEnumerator MoveToWitch(Transform witchTransform, float time)
  {
    cdTime = 0;
    isInCD = true;
    if (transform.parent == null)
    {
      FaceSpriteRenderer.color = Color.white;
      GetComponent<Rigidbody>().isKinematic = true;
      transform.SetParent(witchTransform);
    }
    startAnimationPos = transform.localPosition;
    isMovingToWitch = true;
    var currPos = transform.localPosition;
    yield return CoroutineHelpers.InterpolateByTime(time, (k =>
        {
          var ev = MoveToWitchCurve.Evaluate(k);
          transform.localPosition = Vector3.LerpUnclamped(currPos, Vector3.zero, ev);
        }));
    isMovingToWitch = false;

  }
  IEnumerator UseCurseOnWitch(Transform witchTransform, float time)
  {
    var obj = Instantiate(usingItemParticle.gameObject, transform.position, Quaternion.identity);
    obj.SetActive(true);
    obj.transform.SetParent(witchTransform);
    obj.GetComponent<ParticleSystem>().Play();
    var currPos = transform.localPosition;
    yield return CoroutineHelpers.InterpolateByTime(time, (k =>
        {
          var ev = ItemOnWitchCurve.Evaluate(k);
          obj.transform.localPosition = Vector3.LerpUnclamped(currPos, Vector3.zero, ev);
        }));
    var explosion = Instantiate(destroySphereParticle.gameObject, transform.position, Quaternion.identity);
    explosion.transform.SetParent(witchTransform);
    explosion.transform.localPosition = Vector3.zero;
    explosion.GetComponent<ParticleSystem>().Play();
    Destroy(obj);
    yield return new WaitForSeconds(1f);
    Destroy(explosion);
  }
  IEnumerator GettingControl()
  {
    float time = 0f;
    while (time <= 0.08f)
    {
      time += Time.deltaTime;
      yield return null;

    }
    DeleteSphere();


  }
  IEnumerator BlockedAnimation(float time)
  {
    while (isMovingToWitch)
    {
      yield return null;
    }
    print("block");
    isBeingBlocked = true;
    var initialPos = transform.position;

    GetComponent<Rigidbody>().isKinematic = false;
    GetComponent<Rigidbody>().AddForce(startAnimationPos.normalized * 5f, ForceMode.VelocityChange);
    transform.parent = null;
    yield return new WaitForSeconds(time);
    DeleteSphere();
    destroySphereParticle.Play();
    yield return new WaitForSeconds(1f);
    isBeingBlocked = false;


  }

  void DeleteSphere()
  {
    foreach (ParticleSystem pt in GetComponentsInChildren<ParticleSystem>())
    {
      pt.Stop();
    }
    FaceSpriteRenderer.color = Color.clear;
  }

  IEnumerator SpawnOrb(float time)
  {
    FaceSpriteRenderer.color = Color.white;
    GetComponent<Rigidbody>().isKinematic = true;
    transform.SetParent(playerPos);
    foreach (ParticleSystem pt in GetComponentsInChildren<ParticleSystem>())
    {
      pt.Play();
    }

    yield return CoroutineHelpers.InterpolateByTime(time, (k =>
    {
      //print(_phase);
      transform.localScale = Mathf.Lerp(0, 1, k) * Vector3.one;
    }));

    fullExplosion.Play(true);
  }

}
