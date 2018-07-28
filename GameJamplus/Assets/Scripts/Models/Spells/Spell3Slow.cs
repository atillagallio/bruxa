using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell3Slow : Spell {

	// Use this for initialization

	public Spell3Slow () {
		spellName = "Slow";
		type = 0;
	}

	public override void UseSpell(){
		InGameManager.Instance.spellText.text = spellName;
		InGameManager.Instance.UseSpell3Slow();
	}
}
