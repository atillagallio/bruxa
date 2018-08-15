using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : Singleton<EndGame>
{
  public List<PlayerBehaviour> playerList;
  public PlayerBehaviour winner;
  // Use this for initialization
  void Start()
  {
    playerList = new List<PlayerBehaviour>();
    DontDestroyOnLoad(gameObject);
    EventManager.OnExitGameOverScreen += () =>
    {
      Destroy(this.gameObject);
      print("DESTRUCTYION");
    };
  }

  public void FindWinner()
  {
    winner = new PlayerBehaviour();
    foreach (PlayerBehaviour player in playerList)
    {
      if (player.points > winner.points)
        winner = player;
    }
  }
}
