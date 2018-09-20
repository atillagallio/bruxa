using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterSelectionScreen
{
  internal class CharacterSelectionElement : MonoBehaviour
  {
    [EnumFlag]
    public SlotState StatesToDisplay;
  }
}
