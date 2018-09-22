using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell2Forward : Spell
{

  // Use this for initialization

  public Spell2Forward()
  {
    spellName = "Forward";
    type = 0;
    id = 1;
    spellIcon = Resources.Load<Sprite>("Sprites/orbitablesprites/gatopreto");
  }

  public override void UseSpell()
  {
    InGameManager.Instance.UseSpell2Forward();
  }
}
