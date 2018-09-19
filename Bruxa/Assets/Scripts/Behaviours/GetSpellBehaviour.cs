using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSpellBehaviour : MonoBehaviour
{

  public Spell spell;

  public GameObject locker;
  public GameObject forward;
  public GameObject slow;
  public GameObject bomb;
  public GameObject drunk;

  // Use this for initialization
  void Start()
  {
    int randomValue = Random.Range(1, InGameManager.Instance.spellList.Count + 1);
    if (randomValue == 1)
    {
      spell = new Spell1Locker();
      locker.SetActive(true);
    }
    if (randomValue == 2)
    {
      spell = new Spell2Forward();
      forward.SetActive(true);
    }
    if (randomValue == 3)
    {
      spell = new Spell3Slow();
      slow.SetActive(true);
    }
    if (randomValue == 4)
    {
      spell = new Spell4Bomb();
      bomb.SetActive(true);
    }
    if (randomValue == 5)
    {
      spell = new Spell5Drunk();
      drunk.SetActive(true);
    }

  }

  // Update is called once per frame
  void Update()
  {

  }
}
