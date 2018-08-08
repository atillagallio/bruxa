using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {

	private Joystick playerJoystick;
	private Color playerColor;

	public int gameUiPosition;

	public Spell spell;

	//private List<Spells> spells;
	private bool inCooldown = false;
	private bool inblockChangeSkillCooldown = false;
	private int cdTimer = 3;

	private bool takePlayerControlAxis;
	private bool useSpellControl;

	public int points = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	void GetControls (){
		takePlayerControlAxis = Input.GetButtonDown(playerJoystick.input.Fire1);
		useSpellControl = Input.GetButtonDown(playerJoystick.input.Fire3);	
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
			if(!inCooldown && !InGameManager.Instance.spell1Lock){
				if(InGameManager.Instance.changeBlock){
					GameUIManager.Instance.StartBlockedAnimation();
					inCooldown = true;
					Debug.Log("PLAYER UI POS ->" + gameUiPosition);
					GameUIManager.Instance.UpdateUISkillCD(gameUiPosition,GetCDTimer(),0);
					GameUIManager.Instance.UpdateUISkillCD(gameUiPosition,GetCDTimer(),1);
					StartCoroutine(TakeControllCooldown());
					
				}else{
					Debug.Log("player " + playerJoystick.name + "Trying to get control");
					InGameManager.Instance.ChangeCharacterControl(this);
					inCooldown = true;
					inblockChangeSkillCooldown = false;
				}
				
			}else{
				if(!inblockChangeSkillCooldown){
					InGameManager.Instance.UseChangeBlockSkill(this);
					inblockChangeSkillCooldown = true;
					StartCoroutine(BlockChangeSkillCooldown());
				}
			}
		}
		if(useSpellControl && InGameManager.Instance.HasGameStarted()){
			if(spell != null){
				InGameManager.Instance.PlayerUseSpell(this);
			}
		}
	}

	private IEnumerator BlockChangeSkillCooldown(){
		
		yield return new WaitForSecondsRealtime(5f);
		inblockChangeSkillCooldown = false; 
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
