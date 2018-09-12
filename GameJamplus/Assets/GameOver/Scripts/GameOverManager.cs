using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct GameOverPlayerData
{
  public PlayerInfo PlayerInfo;
  public PlayerGameStats GameStats;
}


public class GameOverManager : MonoBehaviour
{

  public List<GameOverPlayerData> players;

  public Transform WinnerPlayerReference;
  public Transform LoserPlayerReference;

  [SerializeField]
  private float deltaCorrection;

  [SerializeField]
  private PlayerGameOverCard playerCardPrefab;

  [ContextMenu("Update Players")]
  public void SetupGameOverScreen()
  {
    int playerCount = Mathf.Min(players.Count, 4);
    var cam = Camera.main;
    var winnerReferenceDirection = WinnerPlayerReference.position - cam.transform.position;
    var loserReferenceDistance = (LoserPlayerReference.position - cam.transform.position).magnitude;

    var winnerVerticalSize = deltaCorrection * 2 * winnerReferenceDirection.magnitude * Mathf.Atan(Mathf.Deg2Rad * cam.fieldOfView / 2.0f);
    var winnerHorizontalSize = winnerVerticalSize * cam.aspect;
    var winnerHorizontalDelta = winnerHorizontalSize / (float)(playerCount + 1.0);

    var startHorizontalPosition = WinnerPlayerReference.position - Vector3.right * winnerHorizontalDelta * (((float)playerCount - 1) / 2.0f);

    for (int i = 0; i < playerCount; i++)
    {
      var player = players[i];
      var playerChar = Instantiate(player.PlayerInfo.Character.GameOverRepresentation);
      playerChar.transform.parent = transform;
      playerChar.transform.position = startHorizontalPosition + i * Vector3.right * winnerHorizontalDelta;
      if (!IsWinner(player))
      {
        print(player.PlayerInfo.Character.Name + " is not the winner");
        var direction = (playerChar.transform.position - cam.transform.position).normalized;
        playerChar.transform.position = cam.transform.position + direction * loserReferenceDistance;
      }
      else
      {
        print(player.PlayerInfo.Character.Name + " is the winner");
      }
    }

    WinnerPlayerReference.gameObject.SetActive(false);
    LoserPlayerReference.gameObject.SetActive(false);
  }

  bool IsWinner(GameOverPlayerData player)
  {
    return !players.Aggregate(false, (winner, p) => winner || (player.GameStats.points < p.GameStats.points));
  }

  // Use this for initialization
  void Start()
  {
    SetupGameOverScreen();
  }

  // Update is called once per frame
  void Update()
  {

  }
}
