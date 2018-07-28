using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

	private Joystick playerJoystick;
	private Color playerColor;

	public int gameUiPosition;

	//private List<Spells> spells;
	private bool inCooldown = false;
	private int cdTimer = 5;

	private bool takePlayerControlAxis;

	public int points = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	void GetControls (){
		takePlayerControlAxis = Input.GetButtonDown(playerJoystick.input.Fire1);
	}

	public int GetCDTimer(){
		return cdTimer;
	}
	public Joystick GetJoystick(){
		return playerJoystick;
	}

	public Color GetColor(){
		return playerColor;
	}
	public void SetPlayerInfo(Joystick _joystick, Color _color){
		playerJoystick = _joystick;
		playerColor = _color;
	}
	// Update is called once per frame
	void Update () {
		GetControls();
		CheckButtonPress();
	}

	void CheckButtonPress(){
		if(takePlayerControlAxis && InGameManager.Instance.HasGameStarted()){
			if(!inCooldown){
				Debug.Log("player " + playerJoystick.name + "Trying to get control");
				InGameManager.Instance.ChangeCharacterControl(this);
				inCooldown = true;
				
			}
		}
	}

	public IEnumerator TakeControllCooldown()
	{
		int i = 0;
		while(i < cdTimer){
			yield return new WaitForSecondsRealtime(1f);
			i++;
		}
		inCooldown = false;
	}
}
