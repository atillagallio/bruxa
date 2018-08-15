using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverUIManager : MonoBehaviour
{
  public TextMeshProUGUI playerNameText;
  public TextMesh pointsText;
  public float DelayToRestart;

  public List<SpriteRenderer> witches;

  public Fade fade;

  public List<Sprite> witchesWinners;
  void Start()
  {
    fade.StartImageFade();
    PlayerBehaviour ed = EndGame.Instance?.winner;
    if (ed == null)
    {
      Debug.Log("ERROR: No endgame instance available, using mock values");
      ed = new PlayerBehaviour();
      ed.gameUiPosition = 0;
      ed.points = 666;
    }
    playerNameText.text = "Player " + (ed.gameUiPosition + 1);
    pointsText.text = ed.points.ToString();
    witches[ed.gameUiPosition].sprite = witchesWinners[ed.gameUiPosition];
    StartCoroutine(DoUpdate());
  }

  IEnumerator DoUpdate()
  {
    yield return new WaitForSeconds(DelayToRestart);
    while (true)
    {
      if (Input.GetButtonDown("Submit"))
      {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
      }
      yield return null;
    }
  }
}
