using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;
using System;

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

  public Shader grayscale;

  [SerializeField]
  private UIScore UIScore;

  // Use this for initialization
  void Start()
  {
    playersUIs = new List<PlayerUIInfo>();
    EventManager.OnPlayerLeavingWitch += LeavingWitchUIColor;
    EventManager.OnPlayerEnteringWitch += EnteringWitchUIColor;
  }

  void OnDestroy()
  {
    EventManager.OnPlayerLeavingWitch -= LeavingWitchUIColor;
    EventManager.OnPlayerEnteringWitch -= EnteringWitchUIColor;
  }

  public void InstantiateUI(List<PlayerBehaviour> players)
  {

    foreach (var player in players)
    {
      Character charInfo = player.CharacterInfo;
      var thisPlayerInfo = Instantiate(Uiprefab, Vector3.zero, Quaternion.identity);
      thisPlayerInfo.HeroImg.sprite = charInfo.UIFace;
      thisPlayerInfo.HeroImg.material = new Material(grayscale);
      thisPlayerInfo.HeroImg.material.SetFloat("_Grayscale", 1);
      thisPlayerInfo.CooldownFillImg.sprite = charInfo.UICDfill;
      //thisPlayerInfo.CooldownFillImg.color = charInfo.Color;
      //thisPlayerInfo.HeroBG.color = charInfo.Color;
      thisPlayerInfo.Player = player;
      playersUIs.Add(thisPlayerInfo);
      //curPlayerUI.GetComponent<Image>().color = player.GetComponent<PlayerBehaviour>().GetColor();
      thisPlayerInfo.transform.SetParent(UiPositionObj.transform);
    }
    playerList = players;

    UIScore.Players = players;
  }

  public void SetSkill(int pos, string skillName, int id = 0)
  {
    playersUIs[pos].GetComponent<PlayerUIInfo>().ItemImg.sprite = spriteList[id];

    if (skillName == "")
    {
      playersUIs[pos].GetComponent<PlayerUIInfo>().ItemImg.sprite = noSkillUISprite;
    }
  }

  public void EnteringWitchUIColor(PlayerBehaviour player)
  {
    int playerpos = playerList.FindInstanceID(player);
    playersUIs[playerpos].HeroBG.color = player.CharacterInfo.Color;
    playersUIs[playerpos].HeroImg.material.SetFloat("_Grayscale", 0);
  }

  public void LeavingWitchUIColor(PlayerBehaviour player)
  {
    int playerpos = playerList.FindInstanceID(player);
    playersUIs[playerpos].HeroBG.color = Color.white;
    playersUIs[playerpos].HeroImg.material.SetFloat("_Grayscale", 1);
  }

  public void UpdateUISkillCD(int position)
  {
    Image playerImg = playersUIs[position].GetComponent<PlayerUIInfo>().CooldownFillImg;
    //Image playerImg = playersUIs[position].GetComponent<Image>();
    float _cd = playerList[position].GetComponent<PlayerBehaviour>().SwitchCooldown;
    if (_cd > GameDataManager.Data.SwitchCooldown)
    {
      playerImg.fillAmount = 1;
      playersUIs[position].CooldownFillImg.color = playerList[position].GetComponent<PlayerBehaviour>().CharacterInfo.Color;
    }
    else
    {
      playerImg.fillAmount = _cd / GameDataManager.Data.SwitchCooldown;
      playersUIs[position].CooldownFillImg.color = Color.white;
    }
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
