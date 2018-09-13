using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCharacterRepresentation : MonoBehaviour
{

  [SerializeField]
  Animator anim;

  public void SetWinner(bool v = true)
  {
    anim.SetBool("win", v);
  }
  // Use this for initialization
  void Start()
  {
    anim.SetFloat("seed", Random.value);
  }

  // Update is called once per frame
  void Update()
  {
  }
}
