using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell3Slow : Spell
{

  // Use this for initialization

  public Spell3Slow()
  {
    spellName = "Slow";
    type = 0;
    id = 2;
    spellIcon = Resources.Load<Sprite>("Sprites/orbitablesprites/lesma");
  }

  public override void UseSpell()
  {
    InGameManager.Instance.UseSpell3Slow();
  }
}
