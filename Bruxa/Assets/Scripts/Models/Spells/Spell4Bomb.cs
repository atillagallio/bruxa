﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell4Bomb : Spell
{

  // Use this for initialization

  public Spell4Bomb()
  {
    spellName = "Bomb";
    type = 1;
    id = 3;
    spellIcon = Resources.Load<Sprite>("Sprites/orbitablesprites/mina");
  }

  public override void UseSpell()
  {
    InGameManager.Instance.UseSpell4Bomb();
  }
}
