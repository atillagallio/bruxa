using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using System;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MatchManager))]
public class InGameManager : Singleton<InGameManager>
{

  /// <summary>
  /// Start is called on the frame when a script is enabled just before
  /// any of the Update methods is called the first time.
  /// </summary>


  //Prefabs
  public InGameCharacterController gameCharacter;
  [SerializeField]
  private PlayerBehaviour playerPrefab;

  public GameObject endGameCanvas;

  public bool GameStarted => MatchManager.GameStarted;
  public bool GameFinished => MatchManager.GameFinished;

  public int matchDuration = 0;
  public List<Color> colors;
  public TextMeshProUGUI timerText;

  [SerializeField]
  public List<PlayerBehaviour> players
  {
    get; private set;
  }
  public GameObject plane;
  public GameObject pointPrefab;
  public GameObject bombPrefab;

  public GameObject superPointPrefab;
  public GameObject megaPointPrefab;
  public GameObject getSpellPrefab;

  [HideInInspector]
  public bool spell1Lock = false;
  [HideInInspector]
  public bool spell2Forward = false;
  [HideInInspector]
  public bool spell3Slow = false;
  [HideInInspector]
  public bool spell5Drunk = false;

  public TextMeshProUGUI spellText;

  public List<Spell> spellList;

  [HideInInspector]
  public List<string> realJoysticks;

  [HideInInspector]
  public bool parryActive = false;

  [SerializeField]
  private List<Character> AvailableWitches;

  private MatchManager _matchManager;
  private MatchManager MatchManager
  {
    get
    {
      _matchManager = _matchManager ?? (GetComponent<MatchManager>());
      return _matchManager;
    }
  }


  public SpriteRenderer witchEffect;
  public List<Sprite> itemIconSpriteList;
  void Update()
  {
    if (MatchManager.Countdown > 0)
    {
      var CountDownRemap = new List<string>() { "Go!", "Set", "Ready" };
      timerText.text = CountDownRemap[Mathf.FloorToInt(MatchManager.Countdown)];
    }
    else
    {
      timerText.gameObject.SetActive(false);
    }
  }

  public void SetSpellList()
  {
    spellList = new List<Spell>();
    spellList.Add(new Spell1Locker());
    spellList.Add(new Spell2Forward());
    spellList.Add(new Spell3Slow());
    spellList.Add(new Spell4Bomb());
    spellList.Add(new Spell5Drunk());
  }
  public void UseSpell1Lock()
  {
    spell1Lock = true;
    witchEffect.sprite = itemIconSpriteList[0];
    witchEffect.gameObject.SetActive(true);
    StartCoroutine(Spell1Duration(4));
  }

  public void UseSpell2Forward()
  {
    spell2Forward = true;
    witchEffect.sprite = itemIconSpriteList[1];
    witchEffect.gameObject.SetActive(true);
    StartCoroutine(Spell2Duration(3));
  }

  public void UseSpell3Slow()
  {
    spell3Slow = true;
    witchEffect.sprite = itemIconSpriteList[2];
    witchEffect.gameObject.SetActive(true);
    StartCoroutine(Spell3Duration(3));
  }

  public void UseSpell5Drunk()
  {
    witchEffect.sprite = itemIconSpriteList[4];
    witchEffect.gameObject.SetActive(true);
    spell5Drunk = true;
    StartCoroutine(Spell5Duration(3));
  }

  public void UseSpell4Bomb()
  {

    AudioClip mine = gameCharacter.mineSetSound;
    AudioSource.PlayClipAtPoint(mine, gameCharacter.transform.position);
    Vector3 bombPos = new Vector3(gameCharacter.transform.position.x, gameCharacter.transform.position.y + 3, gameCharacter.transform.position.z);
    GameObject bomb = Instantiate(bombPrefab, bombPos, Quaternion.identity);
    Spell4BombBehaviour bombBehaviour = bomb.GetComponent<Spell4BombBehaviour>();
    bombBehaviour.player = gameCharacter.controllingPlayer;
    Debug.Log(gameCharacter.controllingPlayer);

  }


  //JESUS PRECISO REFATORAR ESSAS CORROUTINA MAS N HJ NA MORAL
  private IEnumerator Spell1Duration(int seconds)
  {
    int i = 0;
    gameCharacter.Spell1FX.SetActive(true);
    while (i < seconds)
    {
      yield return new WaitForSecondsRealtime(1f);
      i++;
    }
    witchEffect.gameObject.SetActive(false);
    spell1Lock = false;
    gameCharacter.Spell1FX.SetActive(false);

  }


  private IEnumerator Spell2Duration(int seconds)
  {
    int i = 0;
    gameCharacter.Spell2FX.gameObject.SetActive(true);

    while (i < seconds)
    {
      yield return new WaitForSecondsRealtime(1f);
      i++;
    }
    spell2Forward = false;
    witchEffect.gameObject.SetActive(false);
    gameCharacter.Spell2FX.GetComponent<ParticleSystem>().Stop();
    gameCharacter.Spell2FX.gameObject.SetActive(false);

  }
  private IEnumerator Spell3Duration(int seconds)
  {
    int i = 0;
    while (i < seconds)
    {
      yield return new WaitForSecondsRealtime(1f);
      i++;
    }
    witchEffect.gameObject.SetActive(false);
    spell3Slow = false;

  }

  private IEnumerator Spell5Duration(int seconds)
  {
    int i = 0;
    while (i < seconds)
    {
      yield return new WaitForSecondsRealtime(1f);
      i++;
    }
    witchEffect.gameObject.SetActive(false);
    spell5Drunk = false;

  }

  void Start()
  {
    colors.Add(Color.blue);
    colors.Add(Color.green);
    colors.Add(Color.cyan);
    colors.Add(Color.red);
    SetSpellList();
    InstantiatePlayers();
    GameUIManager.Instance.instantiateUI(players);
    InstantiatePoints(30, pointPrefab);
    InstantiatePoints(10, superPointPrefab);
    InstantiateSpells(5);
    StartCoroutine(respawn());

    MatchManager.StartMatch();
  }

  public IEnumerator respawn()
  {
    int i = 1;
    while (!GameFinished)
    {
      yield return new WaitForSecondsRealtime(2f);
      InstantiatePoints(1, pointPrefab);
      if (i % 10 == 0)
      {
        InstantiatePoints(4, superPointPrefab);
        InstantiateSpells(5);
        InstantiatePoints(2, megaPointPrefab);
      }
      i++;
    }
  }

  public bool HasGameStarted()
  {
    return GameStarted;
  }

  void PopulateRealJoysticks()
  {
    realJoysticks = new List<string>();
    foreach (string name in Input.GetJoystickNames())
    {
      if (name != "")
        realJoysticks.Add(name);
    }
  }
  void InstantiatePlayers()
  {
    players = new List<PlayerBehaviour>();
    int inputPos = 0;
    PopulateRealJoysticks();
    foreach (string input in realJoysticks)
    {
      Debug.Log(input + " ->" + colors[inputPos].ToString());
      var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
      PlayerBehaviour playerController = player.GetComponent<PlayerBehaviour>();
      playerController.CharacterInfo = AvailableWitches[inputPos];
      playerController.GameUiPosition = inputPos;
      playerController.SetJoystick(new Joystick(input, inputPos));
      inputPos++;
      // TODO: CHANGE TO PLAYERBEHAVIOUR
      players.Add(player);
    }
  }

  private void InstantiatePoints(int number, GameObject prefab)
  {
    int i = 0;
    while (i < number)
    {
      Instantiate(prefab, GetARandomTreePos(30), Quaternion.identity);
      i++;
    }
  }

  private void InstantiateSpells(int number)
  {
    int i = 0;
    while (i < number)
    {
      Instantiate(getSpellPrefab, GetARandomTreePos(2), Quaternion.identity);
      i++;
    }
  }



  public void PlayerUseSpell(PlayerBehaviour player)
  {
    if (player.Spell.type == 0)
    {
      if (player != gameCharacter.controllingPlayer)
      {
        player.Spell.UseSpell();
        player.Spell = null;
        AudioSource.PlayClipAtPoint(gameCharacter.witchesLaughter[player.GameUiPosition], gameCharacter.gameObject.transform.position);
        GameUIManager.Instance.SetSkill(player.GameUiPosition, "");
      }
    }
    else if (player.Spell.type == 1)
    {
      if (player == gameCharacter.controllingPlayer)
      {
        player.Spell.UseSpell();
        player.Spell = null;
        GameUIManager.Instance.SetSkill(player.GameUiPosition, "");
      }
    }
  }

  public void UseChangeBlockSkill(PlayerBehaviour player)
  {
    if (player == gameCharacter.controllingPlayer)
    {
      AudioSource.PlayClipAtPoint(gameCharacter.witchesLaughter[player.GameUiPosition], gameCharacter.gameObject.transform.position);
      parryActive = true;
      Debug.Log("BlockSkill");
      StartCoroutine(BlockDuration());
    }
  }

  private IEnumerator BlockDuration()
  {
    gameCharacter.block.SetActive(true);
    yield return new WaitForSeconds(GameDataManager.Data.ParryDuration);
    parryActive = false;
    gameCharacter.block.SetActive(false);
  }


  public void ChangeCharacterControl(PlayerBehaviour player)
  {
    //MOVE TO PLAYER
    if (gameCharacter.controllingPlayer != null)
    {
      gameCharacter.controllingPlayer.SwitchCooldown = 0;
      gameCharacter.controllingPlayer.isInControl = false;
      EventManager.OnPlayerLeavingWitch(gameCharacter.controllingPlayer);
    }
    else
    {
    }
    player.SwitchCooldown = 0;
    gameCharacter.controllingPlayer = player;
    EventManager.OnPlayerEnteringWitch(player);
    gameCharacter.joystick = player.GetJoystick();
    gameCharacter.isControlledByPlayer = true;
    gameCharacter.Color = player.Color;
    int i = 0;
    bool hasChar = false;

    player.isInControl = true;
    player.parryCoolDown = GameDataManager.Data.ParryCooldown - GameDataManager.Data.InitialParryDelay; ;
    AudioSource.PlayClipAtPoint(gameCharacter.witchesLaughter[player.GameUiPosition], gameCharacter.gameObject.transform.position);
    foreach (var playerObj in players)
    {

      if (playerObj == player)
      {
        hasChar = true;
        StartCoroutine(changeWitchParticle(gameCharacter.ChangeParticle.gameObject.GetComponentInChildren<ParticleSystem>()));
        // TODO: REMOVER GET CHILD
        gameCharacter.bruxasGO[i].SetActive(true);
      }
      else
        gameCharacter.bruxasGO[i].SetActive(false);
      i++;

    }
    // TODO: O QUE ISSO FAZ?
    if (!hasChar)
    {
      gameCharacter.ChangeParticle.GetComponentInChildren<ParticleSystem>().Stop();
    }

  }

  private float ChangefxTime = 0.5f;
  private IEnumerator changeWitchParticle(ParticleSystem particle)
  {
    particle.Play();
    yield return new WaitForSecondsRealtime(ChangefxTime);
    particle.Stop();

  }


  public Vector3 GetARandomTreePos(int pos)
  {

    Mesh planeMesh = plane.GetComponent<MeshFilter>().mesh;
    Bounds bounds = planeMesh.bounds;

    float minX = plane.transform.position.x - plane.transform.localScale.x * bounds.extents.x;
    float maxX = plane.transform.position.x + plane.transform.localScale.x * bounds.extents.x;
    float minZ = plane.transform.position.z - plane.transform.localScale.z * bounds.extents.z;
    float maxZ = plane.transform.position.z + plane.transform.localScale.z * bounds.extents.z;

    Vector3 newVec = new Vector3(Random.Range(minX, maxX),
                                 plane.transform.position.y + pos,
                                 Random.Range(minZ, maxX));
    return newVec;
  }
}