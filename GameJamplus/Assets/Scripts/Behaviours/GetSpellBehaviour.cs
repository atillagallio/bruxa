using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSpellBehaviour : MonoBehaviour {

	public Spell spell;

	// Use this for initialization
	void Start () {
		int randomValue = Random.Range(1,InGameManager.Instance.spellList.Count+1);
		if(randomValue == 1){
			spell = new Spell1Locker();
			gameObject.transform.GetChild(1).gameObject.SetActive(true);
		}
		if(randomValue == 2){
			spell = new Spell2Forward();
			gameObject.transform.GetChild(0).gameObject.SetActive(true);
		}
		if(randomValue == 3){
			spell = new Spell3Slow();
			gameObject.transform.GetChild(3).gameObject.SetActive(true);
		}
		if(randomValue == 4){
			spell = new Spell4Bomb();
			gameObject.transform.GetChild(2).gameObject.SetActive(true);
		}
		if(randomValue == 5){
			spell = new Spell5Drunk();
			gameObject.transform.GetChild(4).gameObject.SetActive(true);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
