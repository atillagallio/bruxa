using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCharacterController : MonoBehaviour
{

  public PlayerBehaviour controllingPlayer
  {
    get;
    set;
  }

  public GameObject block;
  public GameObject Spell2FX;
  public GameObject Spell1FX;
  public InGameCharacterData data;
  private float slidingVel;
  CharacterController charController;
  private bool isSliding = false;

  public bool isControlledByPlayer = false;
  [SerializeField]
  private ParticleSystem trail;
  [SerializeField]
  private ParticleSystem pointsPS;
  [SerializeField]
  private ParticleSystem superPointsPS;
  [SerializeField]
  private ParticleSystem megaPointsPS;
  [SerializeField]
  private Projector blobShadow;
  private Color color;
  public Color Color
  {
    get
    {
      return color;
    }
    set
    {
      color = value;
      blobShadow.material.color = color;
      var m = trail.main;
      m.startColor = color;
      var change = ChangeParticle.GetComponentInChildren<ParticleSystem>().main;
      change.startColor = color;
    }
  }

  public InGameCharacterRepresentation BruxaView { get; internal set; }

  public GameObject ChangeParticle;

  private bool isStunned = false;
  public AudioClip pointSound;
  public AudioClip pointBigSound;
  public AudioClip pointMegaSound;
  public AudioClip spellGetSound;
  public AudioClip mineSetSound;
  public AudioClip mineExplode;
  public GameObject stunPS;
  public List<AudioClip> witchesLaughter;
  public float WeirdAngle;
  public float ySpeed;


  [SerializeField]
  private Vector3 moveDirection = Vector3.right;

  Vector3 lastPos;
  // Use this for initialization
  void Start()
  {
    //targetRotation = transform.rotation;
    //horizontalInput = verticalInput = 0;
    charController = GetComponent<CharacterController>();
    lastPos = transform.position;
  }


  // Update is called once per frame

  Vector3 GetInput()
  {
    float horizontalInput = 0, verticalInput = 0;
    horizontalInput = controllingPlayer.HorizontalInput;
    verticalInput = controllingPlayer.VerticalInput;
    if (InGameManager.Instance.spell5Drunk)
    {
      switch (GameDataManager.Data.Skill5DrunkType)
      {
        case DrunknessType.Direction:
          {

            horizontalInput = -horizontalInput;
            verticalInput = -verticalInput;
            break;
          }
        case DrunknessType.Orientation:
          {
            var aux = horizontalInput;
            horizontalInput = verticalInput;
            verticalInput = aux;
            break;
          }
        default:
          {
            Debug.LogError("Not handled drunkness type");
            break;
          }
      }
    }
    return new Vector3(horizontalInput, 0, verticalInput).normalized;
  }

  void FixedUpdate()
  {
    if (controllingPlayer == null) return;
    var input = GetInput();
    bool canMove = (isControlledByPlayer && !isStunned);
    var speed = canMove ? data.ForwardSpeed : 0;

    if (InGameManager.Instance.spell3Slow)
    {
      speed = data.SlowSpellVel;
    }
    if (!InGameManager.Instance.spell2Forward)
    {
      if (input.magnitude != 0)
      {
        var targetDirection = input;
        var rotationDirection = Mathf.Sign(
          Vector3.Cross(
          moveDirection,
          targetDirection
        ).y);
        if ((targetDirection - moveDirection).magnitude == 0)
        {
          rotationDirection = 1;
        }
        var rotSpeed = speed / data.RotationRadius;
        moveDirection = Quaternion.AngleAxis(
            rotSpeed * Mathf.Rad2Deg * Time.deltaTime * rotationDirection, Vector3.up
        ) * moveDirection;
        //print(targetDirection + "" + moveDirection + "" + rotationDirection);
      }
    }

    transform.rotation = Quaternion.Euler(0, WeirdAngle, 0) * Quaternion.LookRotation(moveDirection);
    var r = transform.rotation;
    r.z = 0;
    r.x = 0;
    transform.rotation = r;

    ySpeed -= data.Gravity * Time.deltaTime;
    var velocity = moveDirection.normalized * speed + Vector3.up * ySpeed;
    if (charController.isGrounded)
    {
      velocity = velocity.normalized * speed;
    }
    charController.Move(velocity * Time.deltaTime);
    var planarSpeed = Vector3.ProjectOnPlane((transform.position - lastPos) / Time.deltaTime, Vector3.up);
    ySpeed = (transform.position.y - lastPos.y) / Time.deltaTime;
    lastPos = transform.position;
    isGrounded = charController.isGrounded;

    if (planarSpeed.sqrMagnitude <= data.SlowSpellVel)
    {
      trail.Stop();
      IsMakingTrail = false;
    }
    else if (!IsMakingTrail)
    {
      trail.Play();
      IsMakingTrail = true;
    }

  }

  private bool IsMakingTrail = false;

  public bool isGrounded;

  void OnTriggerEnter(Collider col)
  {
    //Point cube
    if (col.gameObject.layer == LayerMask.NameToLayer("Point"))
    {
      AudioSource.PlayClipAtPoint(pointSound, transform.position);
      pointsPS.Play();
      controllingPlayer.Points += GameDataManager.Data.SmallPoint;
      Destroy(col.gameObject);
    }
    else if (col.gameObject.layer == LayerMask.NameToLayer("SuperPoint"))
    {
      controllingPlayer.Points += GameDataManager.Data.MediumPoint;
      AudioSource.PlayClipAtPoint(pointBigSound, transform.position);
      superPointsPS.Play();
      Destroy(col.gameObject);
    }
    else if (col.gameObject.layer == LayerMask.NameToLayer("MegaPoint"))
    {
      AudioSource.PlayClipAtPoint(pointMegaSound, transform.position);
      controllingPlayer.Points += GameDataManager.Data.LargePoint;
      megaPointsPS.Play();
      Destroy(col.gameObject);
    }
    else if (col.gameObject.layer == LayerMask.NameToLayer("Spell"))
    {
      AudioSource.PlayClipAtPoint(spellGetSound, transform.position);
      controllingPlayer.Spell = col.gameObject.GetComponent<GetSpellBehaviour>().spell;
      GameUIManager.Instance.SetSkill(controllingPlayer.GameUiPosition, controllingPlayer.Spell.spellName, controllingPlayer.Spell.id);
      //Debug.Log(controllingPlayer.Spell.spellName);
      Destroy(col.gameObject);
    }

    else if (col.gameObject.layer == LayerMask.NameToLayer("Bomb"))
    {
      print("collided with bomb");
      if (col.gameObject.GetComponent<Spell4BombBehaviour>().player != controllingPlayer)
      {
        DoBombThing(col.gameObject.GetComponent<Spell4BombBehaviour>().player);
        Destroy(col.gameObject);
      }
    }
  }

  [ContextMenu("TestBomb")]
  void DoBombThing(PlayerBehaviour controllingPlayer)
  {
    StartCoroutine(BombEffect(controllingPlayer));
  }

  void OnControllerColliderHit(ControllerColliderHit col)
  {
    if (col.gameObject.tag == "Wall")
    {
      var normalxz = col.normal;
      normalxz.y = 0;
      moveDirection = Vector3.Reflect(moveDirection, normalxz);
    }
  }

  private IEnumerator BombEffect(PlayerBehaviour player)
  {
    yield return null;
    AudioSource.PlayClipAtPoint(mineExplode, transform.position);
    isStunned = true;
    var stunImpulse = data.BombIsMathematical ?
        data.BombStunTime * data.Gravity
        : data.BombExplosionImpulse;
    print(stunImpulse);
    //ySpeed = stunImpulse;
    charController.Move(Vector3.up * Time.deltaTime * stunImpulse);
    foreach (var ps in stunPS.GetComponentsInChildren<ParticleSystem>())
    {
      ps.Play();
    }
    float stunTime = Time.deltaTime;
    while (stunTime < data.BombStunTime)
    {
      transform.Rotate(Vector3.up * data.BombRotationSpeed * Time.deltaTime);
      yield return null;
      stunTime += Time.deltaTime;
    }
    isStunned = false;
    foreach (var ps in stunPS.GetComponentsInChildren<ParticleSystem>())
    {
      ps.Stop();
    }
    ChangeCharacterControl(player);
  }

  public void ChangeCharacterControl(PlayerBehaviour player)
  {
    //MOVE TO PLAYER
    if (controllingPlayer != null)
    {
      controllingPlayer.SwitchCooldown = 0;
      controllingPlayer.isInControl = false;
      EventManager.OnPlayerLeavingWitch(controllingPlayer);
    }
    else
    {
    }
    player.SwitchCooldown = 0;
    controllingPlayer = player;
    EventManager.OnPlayerEnteringWitch(player);
    isControlledByPlayer = true;
    Color = player.Color;

    bool hasChar = false;

    player.isInControl = true;
    player.parryCoolDown = GameDataManager.Data.ParryCooldown - GameDataManager.Data.InitialParryDelay; ;

    // TODO: move audio to audio place
    AudioSource.PlayClipAtPoint(witchesLaughter[player.GameUiPosition], gameObject.transform.position);

    if (BruxaView != null)
    {
      Destroy(BruxaView.gameObject);
    }
    BruxaView = Instantiate(player.CharacterInfo.InGameRepresentation, this.transform);

    // TODO: O QUE ISSO FAZ?
    if (!hasChar)
    {
      ChangeParticle.GetComponentInChildren<ParticleSystem>().Stop();
    }
  }
}
