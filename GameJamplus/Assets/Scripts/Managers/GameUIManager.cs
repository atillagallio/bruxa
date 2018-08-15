using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameUIManager : Singleton<GameUIManager>
{

    public TextMeshProUGUI timerText;
    public List<GameObject> playersUIs;
    public Sprite noSkillUISprite;

    public GameObject blockedTextObj;
    public List<Sprite> spriteList;

    private IEnumerator timerCr;

    private List<GameObject> playerList;


    // Use this for initialization
    void Start()
    {

    }

    public void instantiateUI(List<GameObject> players)
    {
        int i = 0;
        foreach (GameObject player in players)
        {
            GameObject curPlayerUI = playersUIs[i];
            curPlayerUI.GetComponent<Image>().color = player.GetComponent<PlayerBehaviour>().GetColor();
            playersUIs[i].SetActive(true);
            i++;
        }
        playerList = players;
    }

    public void SetSkill(int pos, string skillName, int id = 0)
    {
        playersUIs[pos].GetComponentInChildren<TextMeshProUGUI>().text = skillName;
        playersUIs[pos].transform.GetChild(2).GetComponent<Image>().sprite = spriteList[id];
        if (skillName == "")
        {
            playersUIs[pos].transform.GetChild(2).GetComponent<Image>().sprite = noSkillUISprite;
        }
    }

    public void UpdateUISkillCD(int position)
    {
        Image playerImg = playersUIs[position].GetComponent<Image>();
        float _cd = playerList[position].GetComponent<PlayerBehaviour>().switchCooldown;
        if (_cd > ConfigurationTestBruxaManager.Instance.switchCooldown)
            playerImg.fillAmount = 1;
        else
            playerImg.fillAmount = _cd / ConfigurationTestBruxaManager.Instance.switchCooldown;

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
