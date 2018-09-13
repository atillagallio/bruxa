using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class PlayerGameOverCard : MonoBehaviour
{
  public Text Name;
  public Text Score;

  public void SetPlayer(GameOverPlayerData player)
  {
    Name.text = player.PlayerInfo.Character.Nome;
    Score.text = player.GameStats.points + " pts";
  }

  void Update()
  {
  }
}