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
    void Start()
    {
        colors.Add(Color.blue);
        colors.Add(Color.green);
        colors.Add(Color.cyan);
        InstantiatePlayers();
        GameUIManager.Instance.instantiateUI(players);
        InstantiatePoints(30);
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
    public void ChangeCharacterControl(PlayerBehaviour player){
        InGameCharacterController charController = gameCharacter.GetComponent<InGameCharacterController>();
        if(charController.controllingPlayer != null){
            GameUIManager.Instance.UpdateUISkillCD(charController.controllingPlayer.gameUiPosition,player.GetCDTimer(),1);
            StartCoroutine(charController.controllingPlayer.TakeControllCooldown());
        }
        charController.controllingPlayer = player;
        charController.joystick = player.GetJoystick();
        charController.isControlledByPlayer = true;
        charController.color = player.GetColor();
        GameUIManager.Instance.UpdateUISkillCD(player.gameUiPosition,player.GetCDTimer(),0);
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

    Vector3 newVec = new Vector3(Random.Range (minX, -minX),
                                 plane.transform.position.y,
                                 Random.Range (minZ, -minZ));
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