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
		}
		if(randomValue == 2){
			spell = new Spell2Forward();
		}
		if(randomValue == 3){
			spell = new Spell3Slow();
		}
		if(randomValue == 4){
			spell = new Spell4Bomb();
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
