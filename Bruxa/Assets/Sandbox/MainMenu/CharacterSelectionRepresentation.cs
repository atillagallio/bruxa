using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterSelectionScreen
{
  [RequireComponent(typeof(Animator))]
  public class CharacterSelectionRepresentation : MonoBehaviour
  {

    private Animator anim;
    void Awake()
    {
      anim = GetComponent<Animator>();
    }

    public void ToggleSelected()
    {
      anim.SetTrigger("ToggleSelect");
    }
  }
}