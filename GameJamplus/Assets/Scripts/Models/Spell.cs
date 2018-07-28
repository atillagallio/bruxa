using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Spell{

	public Image spellIcon;
	public string spellName;
	public int type;

	public abstract void UseSpell();

}
