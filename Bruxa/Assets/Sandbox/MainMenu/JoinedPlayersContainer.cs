using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class JoinedPlayersContainer : MonoBehaviour
{

  private static JoinedPlayersContainer _instance;

  [SerializeField]
  private List<JoinedPlayerData> _players;

  public static List<JoinedPlayerData> Players => _instance._players;
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

    GetDefaultPlayers();
  }

  void GetDefaultPlayers()
  {
    var characters = Resources.LoadAll<Character>("Characters");
    _players = new List<JoinedPlayerData>();
    _players.Add(new JoinedPlayerData(ReInput.players.GetPlayer(0), characters[0]));
    _players.Add(new JoinedPlayerData(ReInput.players.GetPlayer(1), characters[1]));
  }

  public static void SetPlayingPlayers(List<JoinedPlayerData> joinedPlayers)
  {
    _instance._players = joinedPlayers;
  }
}
