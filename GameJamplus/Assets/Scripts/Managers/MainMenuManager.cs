using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour {

	public TextMeshProUGUI playerCount;
	public List<GameObject> controllers;

	public int numberOfControllers = 0;
	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
		int i= 0;
		if(numberOfControllers != Input.GetJoystickNames().Length){
			foreach (string name in Input.GetJoystickNames()){
				controllers[i].SetActive(true);
				i++;
			}
		}	 
	}
}
