using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersContainer : MonoBehaviour
{

  private static PlayersContainerjk _instance;
  void Awake()
  {
    if (_instance == null)
    {
      DontDestroyOnLoad(gameObject);
      _instance = this;
    }
    else if (_instance != this)
    {
      Destroy(gameObject);
    }
  }

  public void SetPlayingPlayers()
  {

  }
}
