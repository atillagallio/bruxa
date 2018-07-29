using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour {

	public TextMeshPro playerCount;
	public List<GameObject> controllers;

	public int numberOfControllers = 0;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void TurnOffAllControllers(){
		foreach (GameObject controller in controllers){
			controller.SetActive(false);
		}
	}

	void Update () {
		int i= 0;
		if(Input.GetButtonDown("Submit")){
			UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay 1");
		}
		if(numberOfControllers != Input.GetJoystickNames().Length){
			TurnOffAllControllers();
			foreach (string name in Input.GetJoystickNames()){
				controllers[i].SetActive(true);
				i++;
			}
			playerCount.text = i + " Players";
		}	 
	}
}
