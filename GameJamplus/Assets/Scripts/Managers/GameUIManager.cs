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
  private List<PlayerUIInfo> playersUIs;

  public GameObject UiPositionObj;
  public PlayerUIInfo Uiprefab;
  public Sprite noSkillUISprite;

  public GameObject blockedTextObj;
  public List<Sprite> spriteList;

  private IEnumerator timerCr;

  private List<PlayerBehaviour> playerList;

  [SerializeField]
  private TextMeshProUGUI POINTS;


  // Use this for initialization
  void Start()
  {
    playersUIs = new List<PlayerUIInfo>();
  }

  public void instantiateUI(List<PlayerBehaviour> players)
  {

    foreach (var player in players)
    {
      Character charInfo = player.GetComponent<PlayerBehaviour>().GetCharacterInfo();
      var thisPlayerInfo = Instantiate(Uiprefab, Vector3.zero, Quaternion.identity);
      //thisPlayerInfo = curPlayerUI.GetComponent<PlayerUIInfo>();
      thisPlayerInfo.HeroImg.sprite = charInfo.UIFace;
      thisPlayerInfo.CooldownFillImg.sprite = charInfo.UICDfill;
      thisPlayerInfo.CooldownFillImg.color = charInfo.Color;
      thisPlayerInfo.HeroBG.color = charInfo.Color;
      thisPlayerInfo.Player = player;
      playersUIs.Add(thisPlayerInfo);
      //curPlayerUI.GetComponent<Image>().color = player.GetComponent<PlayerBehaviour>().GetColor();
      thisPlayerInfo.transform.SetParent(UiPositionObj.transform);
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
    float _cd = playerList[position].GetComponent<PlayerBehaviour>().switchCooldown;
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

  public void UpdateTimer(int time)
  {
    string timeFormat;
    int minutes = time / 60;
    int seconds = time % 60;
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
