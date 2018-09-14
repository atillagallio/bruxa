using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPlayerDataContainer : MonoBehaviour
{

  private List<GameOverPlayerData> _players;
  public static List<GameOverPlayerData> Players => _instance._players;

  private static GameOverPlayerDataContainer _instance;

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

  public static void SetData(List<PlayerBehaviour> players)
  {
    _instance._players = players.Select((p) =>
    {
      return new GameOverPlayerData()
      {
        Info = new PlayerInfo()
        {
          PlayerNumber = p.GameUiPosition,
          Character = p.CharacterInfo
        },
        GameStats = new PlayerGameStats()
        {
          Points = p.Points,
        },
      };
    }).ToList();
  }
}
