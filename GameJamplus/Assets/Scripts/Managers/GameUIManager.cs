﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIManager : Singleton<GameUIManager> {

	public TextMeshProUGUI timerText;
	public List<GameObject> playersUIs;
	public Sprite noSkillUISprite;

	public GameObject blockedTextObj;
	public List<Sprite> spriteList;

	private IEnumerator timerCr;

	// Use this for initialization
	void Start () {
		
	}

	public void instantiateUI(List<GameObject> players){
		int i = 0;
		foreach (GameObject player in players){
			GameObject curPlayerUI = playersUIs[i];
			//curPlayerUI.GetComponent<Image>().color = player.GetComponent<PlayerBehaviour>().GetColor();
			playersUIs[i].SetActive(true);
			i++;
		}
	}

	public void SetSkill(int pos, string skillName, int id = 0){
		playersUIs[pos].GetComponentInChildren<TextMeshProUGUI>().text = skillName;
		playersUIs[pos].transform.GetChild(2).GetComponent<Image>().sprite = spriteList[id];
		 if(skillName == ""){
		 	playersUIs[pos].transform.GetChild(2).GetComponent<Image>().sprite = noSkillUISprite;
		 }
	}
	public void UpdateUISkillCD(int position,int timer, int type){
		Image playerImg = playersUIs[position].GetComponent<Image>();

		if(type == 0){
			
			playerImg.fillAmount = 0;
		}
		if(type == 1)
		{
			
			float percentage = timer*10f;

			if(timerCr != null){
				StopCoroutine(timerCr);
			}
			timerCr = FillGauge(100f/percentage,playerImg);

			StartCoroutine(timerCr);
		}	

	}

	public IEnumerator FillGauge(float fill, Image playerImg){
		while(playerImg.fillAmount < 1){
			playerImg.fillAmount += fill/100;
			yield return new WaitForSecondsRealtime(0.1f);	
			
		}
	}


	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartBlockedAnimation()
	{
		StartCoroutine(BlockedAnimationCorroutine());
	}

	private IEnumerator BlockedAnimationCorroutine(){
		blockedTextObj.SetActive(true);
		yield return new WaitForSeconds(1f);
		blockedTextObj.SetActive(false);
	}

	public void UpdateTimer(int time)
	{
		string timeFormat;
		int minutes = time/60;
		int seconds = time % 60;
		string secondsAux = "";
		string minutesAux = "";
		if(seconds < 10)
			secondsAux = "0";
		if(minutes < 10)
			minutesAux = "0";

		timeFormat = minutesAux + minutes + ":" + secondsAux + seconds;
		timerText.text = timeFormat;


	}
}
