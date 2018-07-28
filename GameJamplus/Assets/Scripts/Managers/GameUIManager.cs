using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIManager : Singleton<GameUIManager> {

	public TextMeshProUGUI timerText;
	public List<GameObject> playersUIs;

	// Use this for initialization
	void Start () {
		
	}

	public void instantiateUI(List<GameObject> players){
		int i = 0;
		foreach (GameObject player in players){
			GameObject curPlayerUI = playersUIs[i];
			curPlayerUI.GetComponent<Image>().color = player.GetComponent<PlayerBehaviour>().GetColor();
			playersUIs[i].SetActive(true);
			i++;
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

			StartCoroutine(FillGauge(100f/percentage,playerImg));
		}	

	}

	public IEnumerator FillGauge(float fill, Image playerImg){
		while(playerImg.fillAmount <= 100){
			playerImg.fillAmount += fill/100;
			yield return new WaitForSecondsRealtime(0.1f);
			Debug.Log(fill);
			
			
		}


	}


	
	// Update is called once per frame
	void Update () {
		
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
