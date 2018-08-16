using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : Singleton<EndGame>
{
  public List<PlayerBehaviour> playerList;

  public WinnerInfo WinnerInfo;
  // Use this for initialization
  void Start()
  {
    playerList = new List<PlayerBehaviour>();
    DontDestroyOnLoad(gameObject);
    EventManager.OnExitGameOverScreen += () =>
    {
      Destroy(this.gameObject);
    };
  }

  public void FindWinner()
  {
    var winner = playerList[0];
    foreach (PlayerBehaviour player in playerList)
    {
      if (player.points > winner.points)
        winner = player;
    }
    WinnerInfo = new WinnerInfo()
    {
      points = winner.points,
      number = winner.gameUiPosition
    };
  }
}
