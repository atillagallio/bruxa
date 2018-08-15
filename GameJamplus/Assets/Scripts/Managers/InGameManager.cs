using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class InGameManager : Singleton<InGameManager>
{

  /// <summary>
  /// Start is called on the frame when a script is enabled just before
  /// any of the Update methods is called the first time.
  /// </summary>


  bool gameStarted = false;
  //Prefabs
  public GameObject gameCharacter;
  public GameObject playerPrefab;

  public GameObject endGameCanvas;

  private bool gameFinished = false;

  public int matchDuration = 0;
  public List<Color> colors;
  public TextMeshProUGUI timerText;
  private List<GameObject> players;
  public GameObject plane;
  public GameObject pointPrefab;
  public GameObject bombPrefab;

  public GameObject superPointPrefab;
  public GameObject megaPointPrefab;
  public GameObject getSpellPrefab;

  public bool spell1Lock = false;
  public bool spell2Forward = false;
  public bool spell3Slow = false;
  public bool spell5Drunk = false;

  public TextMeshProUGUI spellText;

  public List<Spell> spellList;

  public List<string> realJoysticks;

  public bool parryActive = false;

  public ParticleSystem trail;

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
    StartCoroutine(Spell1Duration(4));
  }

  public void UseSpell2Forward()
  {
    spell2Forward = true;
    StartCoroutine(Spell2Duration(3));
  }

  public void UseSpell3Slow()
  {
    spell3Slow = true;
    StartCoroutine(Spell3Duration(3));
  }

  public void UseSpell5Drunk()
  {
    spell5Drunk = true;
    StartCoroutine(Spell5Duration(3));
  }

  public void UseSpell4Bomb()
  {

    AudioClip mine = gameCharacter.GetComponent<InGameCharacterController>().mineSetSound;
    AudioSource.PlayClipAtPoint(mine, gameCharacter.transform.position);
    Vector3 bombPos = new Vector3(gameCharacter.transform.position.x, gameCharacter.transform.position.y + 3, gameCharacter.transform.position.z);
    GameObject bomb = Instantiate(bombPrefab, bombPos, Quaternion.identity);
    Spell4BombBehaviour bombBehaviour = bomb.GetComponent<Spell4BombBehaviour>();
    bombBehaviour.player = gameCharacter.GetComponent<InGameCharacterController>().controllingPlayer;
    Debug.Log(gameCharacter.GetComponent<InGameCharacterController>().controllingPlayer);
  }


  //JESUS PRECISO REFATORAR ESSAS CORROUTINA MAS N HJ NA MORAL
  private IEnumerator Spell1Duration(int seconds)
  {
    int i = 0;
    gameCharacter.transform.GetChild(6).gameObject.SetActive(true);
    while (i < seconds)
    {
      yield return new WaitForSecondsRealtime(1f);
      i++;
    }
    spell1Lock = false;
    gameCharacter.transform.GetChild(6).gameObject.SetActive(false);
    spellText.text = "";
  }


  private IEnumerator Spell2Duration(int seconds)
  {
    int i = 0;
    gameCharacter.transform.GetChild(5).gameObject.SetActive(true);

    while (i < seconds)
    {
      yield return new WaitForSecondsRealtime(1f);
      i++;
    }
    spell2Forward = false;
    gameCharacter.transform.GetChild(5).GetComponent<ParticleSystem>().Stop();
    gameCharacter.transform.GetChild(5).gameObject.SetActive(false);
    spellText.text = "";
  }
  private IEnumerator Spell3Duration(int seconds)
  {
    int i = 0;
    while (i < seconds)
    {
      yield return new WaitForSecondsRealtime(1f);
      i++;
    }
    spell3Slow = false;
    spellText.text = "";
  }

  private IEnumerator Spell5Duration(int seconds)
  {
    int i = 0;
    while (i < seconds)
    {
      yield return new WaitForSecondsRealtime(1f);
      i++;
    }
    spell5Drunk = false;
    spellText.text = "";
  }

  void Start()
  {
    matchDuration = GameDataManager.Data.MatchDuration;
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
    StartCoroutine(runStartCounter(3));
    StartCoroutine(respawn());

  }

  public IEnumerator respawn()
  {
    int i = 1;
    while (!gameFinished)
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
    return gameStarted;
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
    players = new List<GameObject>();
    int inputPos = 0;
    PopulateRealJoysticks();
    foreach (string input in realJoysticks)
    {
      Debug.Log(input + " ->" + colors[inputPos].ToString());
      GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
      PlayerBehaviour playerController = player.GetComponent<PlayerBehaviour>();
      playerController.gameUiPosition = inputPos;
      playerController.SetPlayerInfo(new Joystick(input, inputPos), colors[inputPos]);
      inputPos++;
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
    InGameCharacterController charController = gameCharacter.GetComponent<InGameCharacterController>();
    if (player.spell.type == 0)
    {
      if (player != charController.controllingPlayer)
      {
        player.spell.UseSpell();
        player.spell = null;
        AudioSource.PlayClipAtPoint(charController.witchesLaughter[player.gameUiPosition], charController.gameObject.transform.position);
        GameUIManager.Instance.SetSkill(player.gameUiPosition, "");
      }
    }
    else if (player.spell.type == 1)
    {
      if (player == charController.controllingPlayer)
      {
        player.spell.UseSpell();
        player.spell = null;
        GameUIManager.Instance.SetSkill(player.gameUiPosition, "");
      }
    }
  }

  public void UseChangeBlockSkill(PlayerBehaviour player)
  {
    InGameCharacterController charController = gameCharacter.GetComponent<InGameCharacterController>();
    if (player == charController.controllingPlayer)
    {
      AudioSource.PlayClipAtPoint(charController.witchesLaughter[player.gameUiPosition], charController.gameObject.transform.position);
      parryActive = true;
      Debug.Log("BlockSkill");
      StartCoroutine(BlockDuration());
    }
  }

  private IEnumerator BlockDuration()
  {
    gameCharacter.transform.GetChild(10).gameObject.SetActive(true);
    yield return new WaitForSeconds(GameDataManager.Data.ParryDuration);
    parryActive = false;
    gameCharacter.transform.GetChild(10).gameObject.SetActive(false);
  }


  public void ChangeCharacterControl(PlayerBehaviour player)
  {
    InGameCharacterController charController = gameCharacter.GetComponent<InGameCharacterController>();
    //MOVE TO PLAYER
    if (charController.controllingPlayer != null)
    {
      charController.controllingPlayer.switchCooldown = 0;
      charController.controllingPlayer.isInControl = false;
    }
    player.switchCooldown = 0;
    charController.controllingPlayer = player;
    charController.joystick = player.GetJoystick();
    charController.isControlledByPlayer = true;
    charController.color = player.GetColor();
    int i = 0;
    bool hasChar = false;
    var trailSettings = trail.main;
    trailSettings.startColor = player.GetColor();

    player.isInControl = true;
    player.parryCoolDown = GameDataManager.Data.ParryCooldown - GameDataManager.Data.InitialParryDelay; ;
    AudioSource.PlayClipAtPoint(charController.witchesLaughter[player.gameUiPosition], charController.gameObject.transform.position);
    foreach (GameObject playerObj in players)
    {

      if (playerObj == player.gameObject)
      {
        hasChar = true;
        StartCoroutine(changeWitchParticle(charController.transform.GetChild(4).gameObject.GetComponentInChildren<ParticleSystem>()));
        charController.transform.GetChild(i).gameObject.SetActive(true);
      }
      else
        charController.transform.GetChild(i).gameObject.SetActive(false);
      i++;

    }
    if (!hasChar)
      charController.transform.GetChild(4).gameObject.GetComponentInChildren<ParticleSystem>().Stop();
  }

  private IEnumerator changeWitchParticle(ParticleSystem particle)
  {
    particle.Play();
    yield return new WaitForSecondsRealtime(0.5f);
    particle.Stop();

  }

  private IEnumerator runStartCounter(int timer)
  {
    while (timer > 0)
    {
      timerText.text = timer.ToString();
      yield return new WaitForSecondsRealtime(1);
      timer--;
    }
    timerText.gameObject.SetActive(false);
    gameStarted = true;
    StartCoroutine(MatchTimer());
  }
  void FixedUpdate()
  {

  }

  public Vector3 GetARandomTreePos(int pos)
  {

    Mesh planeMesh = plane.GetComponent<MeshFilter>().mesh;
    Bounds bounds = planeMesh.bounds;

    float minX = plane.transform.position.x - plane.transform.localScale.x * bounds.size.x * 0.5f;
    float minZ = plane.transform.position.z - plane.transform.localScale.z * bounds.size.z * 0.5f;

    Vector3 newVec = new Vector3(Random.Range(minX * 3, -minX),
                                 plane.transform.position.y + pos,
                                 Random.Range(minZ * 3, -minZ));
    return newVec;
  }
  private IEnumerator MatchTimer()
  {
    Debug.Log("Match Start");
    int i = 0;
    while (i < matchDuration)
    {
      yield return new WaitForSecondsRealtime(1f);
      i++;
      GameUIManager.Instance.UpdateTimer(i);
    }
    gameFinished = true;
    string txt = "";
    foreach (GameObject player in players)
    {
      txt += player.GetComponent<PlayerBehaviour>().name + " -> " + player.GetComponent<PlayerBehaviour>().gameUiPosition + " -> " + player.GetComponent<PlayerBehaviour>().points;
      Debug.Log(txt);
    }
    endGameCanvas.SetActive(true);

    foreach (GameObject player in players)
    {
      EndGame.Instance.playerList.Add(player.GetComponent<PlayerBehaviour>());
    }
    EndGame.Instance.FindWinner();
    UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
    //endGameCanvas.GetComponentInChildren<TextMeshProUGUI>().text = txt;
  }

}