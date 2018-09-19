using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Spell{

	public Sprite spellIcon;
	public string spellName;
	public int type;

	public int id;

	public abstract void UseSpell();

}
