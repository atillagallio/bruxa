using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell1Locker : Spell
{

  // Use this for initialization
  public Spell1Locker()
  {
    spellName = "Locker";
    type = 1;
    spellIcon = Resources.Load<Sprite>("Sprites/pena");
    Debug.Log(spellIcon);
  }

  public override void UseSpell()
  {
    InGameManager.Instance.spellText.text = spellName;
    InGameManager.Instance.UseSpell1Lock();
  }

}
