using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DrunknessType
{
  Direction,
  Orientation,
}
public class Spell5Drunk : Spell
{

  // Use this for initialization

  public Spell5Drunk()
  {
    spellName = "Drunkness";
    type = 0;
    id = 4;
  }

  public override void UseSpell()
  {
    InGameManager.Instance.spellText.text = spellName;
    InGameManager.Instance.UseSpell5Drunk();
  }
}
