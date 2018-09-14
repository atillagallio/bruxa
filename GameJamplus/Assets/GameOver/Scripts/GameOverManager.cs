using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct GameOverPlayerData
{
  public PlayerInfo Info;
  public PlayerGameStats GameStats;
}


public class GameOverManager : MonoBehaviour
{

  private List<GameOverPlayerData> players => GameOverPlayerDataContainer.Players;

  public Transform WinnerPlayerReference;
  public Transform LoserPlayerReference;

  [SerializeField]
  private float deltaCorrection;

  [SerializeField]
  private HorizontalLayoutGroup scoresGroup;

  [SerializeField]
  private Text winnerName;

  [SerializeField]
  private PlayerGameOverCard playerCardPrefab;

  [SerializeField]
  private Camera referenceCam;

  [ContextMenu("Update Players")]
  public void SetupGameOverScreen()
  {
    int playerCount = Mathf.Min(players.Count, 4);
    referenceCam = Camera.main;
    var winnerReferenceDirection = WinnerPlayerReference.position - referenceCam.transform.position;
    var loserReferenceDistance = (LoserPlayerReference.position - WinnerPlayerReference.position).magnitude;

    var winnerVerticalSize = deltaCorrection * 2 * winnerReferenceDirection.magnitude * Mathf.Atan(Mathf.Deg2Rad * referenceCam.fieldOfView / 2.0f);
    var winnerHorizontalSize = winnerVerticalSize * referenceCam.aspect;
    var winnerHorizontalDelta = winnerHorizontalSize / (float)(playerCount + 1.0);

    var startHorizontalPosition = WinnerPlayerReference.position - Vector3.right * winnerHorizontalDelta * (((float)playerCount - 1) / 2.0f);

    for (int i = 0; i < playerCount; i++)
    {
      var player = players[i];
      var playerChar = Instantiate(player.Info.Character.GameOverRepresentation);
      playerChar.transform.parent = transform;
      playerChar.transform.position = startHorizontalPosition + i * Vector3.right * winnerHorizontalDelta;

      var uiCard = Instantiate(playerCardPrefab);
      uiCard.SetPlayer(player);
      uiCard.transform.parent = scoresGroup.transform;

      if (!IsWinner(player))
      {
        var direction = (playerChar.transform.position - referenceCam.transform.position).normalized;
        direction = Vector3.ProjectOnPlane(direction, Vector3.up);
        Vector3 pos;
        pos = referenceCam.transform.position + direction * loserReferenceDistance;
        pos.y = LoserPlayerReference.position.y;
        playerChar.transform.position = pos;
      }
      else
      {
        playerChar.SetWinner();
        winnerName.text = player.Info.Character.Nome;
      }
    }

    WinnerPlayerReference.gameObject.SetActive(false);
    LoserPlayerReference.gameObject.SetActive(false);
  }

  bool IsWinner(GameOverPlayerData player)
  {
    return !players.Aggregate(false, (winner, p) => winner || (player.GameStats.Points < p.GameStats.Points));
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
