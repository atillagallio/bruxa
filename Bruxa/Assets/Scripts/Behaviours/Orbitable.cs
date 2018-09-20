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

  private Vector3 startAnimationPos;

  public AnimationCurve MoveToWitchCurve;
  void Start()
  {
    GetComponent<Rigidbody>().isKinematic = true;
    var orbitManager = OrbitManager.Instance;
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

    if (!isMovingToWitch && !isBeingBlocked)
    {
      var obtTransform = gameObject.transform;
      obtTransform.position = playerPos.position + new Vector3(Noise(radius) * Mathf.Cos(rotationSpeed * Time.time + _phase), sphereY, radius * Mathf.Sin(rotationSpeed * Time.time + _phase));
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
    StartCoroutine(BlockedAnimation(5f));
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
    if (transform.parent == null)
    {
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
  IEnumerator GettingControl()
  {
    while (isMovingToWitch)
    {
      yield return null;
    }
    //yield return new WaitForSeconds(time);
    this.gameObject.SetActive(false);

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
    yield return CoroutineHelpers.InterpolateByTime(time, (k =>
        {
          //transform.position = Vector3.Lerp(initialPos, new Vector3(initialPos.x * -1.05f, initialPos.y * -1.05f, initialPos.z), k);
        }));
    isBeingBlocked = false;

  }

  IEnumerator SpawnOrb(float time)
  {

    yield return CoroutineHelpers.InterpolateByTime(time, (k =>
    {
      //print(_phase);
      transform.localScale = Mathf.Lerp(0, 1, k) * Vector3.one;
    }));

    fullExplosion.Play(true);
  }

}
