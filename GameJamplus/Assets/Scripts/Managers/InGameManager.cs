using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class InGameManager : Singleton<InGameManager> {

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>


    bool gameStarted = false;
    //Prefabs
    public GameObject gameCharacter;
    public GameObject playerPrefab;

    public int matchDuration = 5*60;
    public List<Color> colors;
    public TextMeshProUGUI timerText;
    private List<GameObject> players;
    public GameObject plane;
    public GameObject pointPrefab;
    public GameObject bombPrefab;

    public GameObject getSpellPrefab;

    public bool spell1Lock = false;
    public bool spell2Forward = false;
    public bool spell3Slow = false;

    public TextMeshProUGUI spellText;

    public List<Spell> spellList;

    public void SetSpellList(){
        spellList = new List<Spell>();
        spellList.Add(new Spell1Locker());
        spellList.Add(new Spell2Forward());
        spellList.Add(new Spell3Slow());
        spellList.Add(new Spell4Bomb());
    }
    public void UseSpell1Lock(){
        spell1Lock = true;
        StartCoroutine(Spell1Duration(2));
    }

    public void UseSpell2Forward(){
        spell2Forward = true;
        StartCoroutine(Spell2Duration(2));
    }

    public void UseSpell3Slow(){
        spell3Slow = true;
        StartCoroutine(Spell3Duration(3));
    }

    public void UseSpell4Bomb(){
        GameObject bomb = Instantiate(bombPrefab, gameCharacter.transform.position, Quaternion.identity);
        Spell4BombBehaviour bombBehaviour = bomb.GetComponent<Spell4BombBehaviour>();
        bombBehaviour.player = gameCharacter.GetComponent<InGameCharacterController>().controllingPlayer;
        Debug.Log(gameCharacter.GetComponent<InGameCharacterController>().controllingPlayer);
    }


    //JESUS PRECISO REFATORAR ESSAS CORROUTINA MAS N HJ NA MORAL
    private IEnumerator Spell1Duration(int seconds){
        int i = 0;
        while(i < seconds){
            yield return new WaitForSecondsRealtime(1f);
            i++;
        }
        spell1Lock = false;
        spellText.text = "";
    } 

    private IEnumerator Spell2Duration(int seconds){
        int i = 0;
        while(i < seconds){
            yield return new WaitForSecondsRealtime(1f);
            i++;
        }
        spell2Forward = false;
        spellText.text = "";
    }
    private IEnumerator Spell3Duration(int seconds){
        int i = 0;
        while(i < seconds){
            yield return new WaitForSecondsRealtime(1f);
            i++;
        }
        spell3Slow = false;
        spellText.text = "";
    } 

    void Start()
    {
        colors.Add(Color.blue);
        colors.Add(Color.green);
        colors.Add(Color.cyan);
        SetSpellList();
        InstantiatePlayers();
        GameUIManager.Instance.instantiateUI(players);
        InstantiatePoints(30);
        InstantiateSpells(10);
        StartCoroutine(runStartCounter(3));

    }

    public bool HasGameStarted(){
        return gameStarted;
    }

    void InstantiatePlayers(){
        players = new List<GameObject>();
        int inputPos = 0;
        foreach (string input in Input.GetJoystickNames()){
			GameObject player = Instantiate(playerPrefab,Vector3.zero, Quaternion.identity);
            PlayerBehaviour playerController = player.GetComponent<PlayerBehaviour>();
            playerController.gameUiPosition = inputPos;
            playerController.SetPlayerInfo(new Joystick(input, inputPos), colors[inputPos]);
            Debug.Log(input + " ->" + colors[inputPos].ToString());
            inputPos++;
            players.Add(player);
		}  
    }

    private void InstantiatePoints(int number){
        int i = 0;
        while(i < number){
            Instantiate(pointPrefab, GetARandomTreePos(), Quaternion.identity);
            i++;
        }
    }

    private void InstantiateSpells(int number){
        int i = 0;
        while(i < number){
            Instantiate(getSpellPrefab, GetARandomTreePos(), Quaternion.identity);
            i++;
        }
    }   

    

    public void PlayerUseSpell(PlayerBehaviour player){
        InGameCharacterController charController = gameCharacter.GetComponent<InGameCharacterController>();
        if(player.spell.type == 0){
            if(player != charController.controllingPlayer){
                player.spell.UseSpell();
                player.spell = null;
            }
        }else if(player.spell.type == 1){
            if(player == charController.controllingPlayer){
                player.spell.UseSpell();
                player.spell = null;

            }
        }
    }
    public void ChangeCharacterControl(PlayerBehaviour player){
        if(!spell1Lock){
            InGameCharacterController charController = gameCharacter.GetComponent<InGameCharacterController>();
            if(charController.controllingPlayer != null){
                GameUIManager.Instance.UpdateUISkillCD(charController.controllingPlayer.gameUiPosition,player.GetCDTimer(),1);
                Debug.Log(charController.controllingPlayer.name);
                StartCoroutine(charController.controllingPlayer.TakeControllCooldown());
            }
            charController.controllingPlayer = player;
            charController.joystick = player.GetJoystick();
            charController.isControlledByPlayer = true;
            charController.color = player.GetColor();
            GameUIManager.Instance.UpdateUISkillCD(player.gameUiPosition,player.GetCDTimer(),0);
        }
    }

    private IEnumerator runStartCounter(int timer){
        while(timer > 0){
            timerText.text = timer.ToString();
            yield return new WaitForSecondsRealtime(1);
            timer--;
        }
        timerText.gameObject.SetActive(false);
        gameStarted = true;
        StartCoroutine(MatchTimer());


    }
    void FixedUpdate(){

    }

    public Vector3 GetARandomTreePos(){

    Mesh planeMesh = plane.GetComponent<MeshFilter>().mesh;
    Bounds bounds = planeMesh.bounds;

    float minX = plane.transform.position.x - plane.transform.localScale.x * bounds.size.x * 0.5f;
    float minZ = plane.transform.position.z - plane.transform.localScale.z * bounds.size.z * 0.5f;

    Vector3 newVec = new Vector3(Random.Range (minX*3, -minX),
                                 plane.transform.position.y+ 2,
                                 Random.Range (minZ*3, -minZ));
    return newVec;
    }
    private IEnumerator MatchTimer()
    {
        Debug.Log("Match Start");
        int i = 0;
        while(i < matchDuration){
            yield return new WaitForSecondsRealtime(1f);
            i++;
            GameUIManager.Instance.UpdateTimer(i);
        }
        Debug.Log("Game Over");
    }

}