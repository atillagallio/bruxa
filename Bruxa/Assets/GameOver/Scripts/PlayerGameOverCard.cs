using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class PlayerGameOverCard : MonoBehaviour
{
  public Text Name;
  public Text Score;

  public void SetPlayer(GameOverPlayerData player)
  {
    Name.text = player.Info.Character.Nome;
    Score.text = player.GameStats.Points + " pts";
  }

  void Update()
  {
  }
}