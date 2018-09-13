using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class GameUIManager : Singleton<GameUIManager>
{

  public TextMeshProUGUI timerText;
  private List<GameObject> playersUIs;

  public GameObject UiPositionObj;
  public GameObject Uiprefab;
  public Sprite noSkillUISprite;

  public GameObject blockedTextObj;
  public List<Sprite> spriteList;

  private IEnumerator timerCr;

  private List<PlayerBehaviour> playerList;
  public TextMeshProUGUI POINTS;


  // Use this for initialization
  void Start()
  {
    playersUIs = new List<GameObject>();
  }

  public void instantiateUI(List<PlayerBehaviour> players)
  {

    foreach (var player in players)
    {
      GameObject curPlayerUI = Instantiate(Uiprefab, Vector3.zero, Quaternion.identity);
      playersUIs.Add(curPlayerUI);
      //curPlayerUI.GetComponent<Image>().color = player.GetComponent<PlayerBehaviour>().GetColor();
      curPlayerUI.transform.SetParent(UiPositionObj.transform);
    }
    playerList = players;
  }

  public void SetSkill(int pos, string skillName, int id = 0)
  {
    playersUIs[pos].GetComponent<PlayerUIInfo>().ItemImg.sprite = spriteList[id];

    if (skillName == "")
    {
      playersUIs[pos].GetComponent<PlayerUIInfo>().ItemImg.sprite = noSkillUISprite;
    }
  }

  public void UpdateUISkillCD(int position)
  {
    Image playerImg = playersUIs[position].GetComponent<PlayerUIInfo>().CooldownFillImg;
    //Image playerImg = playersUIs[position].GetComponent<Image>();
    float _cd = playerList[position].switchCooldown;
    if (_cd > GameDataManager.Data.SwitchCooldown)
      playerImg.fillAmount = 1;
    else
      playerImg.fillAmount = _cd / GameDataManager.Data.SwitchCooldown;

  }




  // Update is called once per frame
  void Update()
  {
    for (int i = 0; i < playerList.Count; i++)
    {

      UpdateUISkillCD(i);
    }

    var points = playerList.Select((p) => p.points).ToList();
    var strPoints = "POINTS: ";
    foreach (var p in points)
    {
      strPoints += p + " ";
    }
    POINTS.text = strPoints;
  }

  public void StartBlockedAnimation()
  {
    StartCoroutine(BlockedAnimationCorroutine());
  }

  private IEnumerator BlockedAnimationCorroutine()
  {
    blockedTextObj.SetActive(true);
    yield return new WaitForSeconds(1f);
    blockedTextObj.SetActive(false);
  }

  public void UpdateTimer(float time)
  {
    string timeFormat;

    float minutes = Mathf.Floor(time / 60);
    float seconds = time % 60;
    string secondsAux = "";
    string minutesAux = "";
    if (seconds < 10)
      secondsAux = "0";
    if (minutes < 10)
      minutesAux = "0";

    timeFormat = minutesAux + minutes + ":" + secondsAux + seconds;
    timerText.text = timeFormat;


  }
}
