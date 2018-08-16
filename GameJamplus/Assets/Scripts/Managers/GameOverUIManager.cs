using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinnerInfo
{
  public int number;
  public int points;
}
public class GameOverUIManager : MonoBehaviour
{
  public TextMeshProUGUI playerNameText;
  public TextMesh pointsText;
  public float DelayToRestart;

  public List<SpriteRenderer> witches;

  public Fade fade;

  public WinnerInfo WinnerInfo;

  public List<Sprite> witchesWinners;
  void Start()
  {
    fade.StartImageFade();
    var ed = EndGame.Instance?.WinnerInfo;
    if (ed == null)
    {
      Debug.Log("ERROR: No endgame instance available, using mock values");
      ed = new WinnerInfo();
      ed.number = 0;
      ed.points = 666;
    }
    playerNameText.text = "Player " + (ed.number + 1);
    pointsText.text = ed.points.ToString();
    witches[ed.number].sprite = witchesWinners[ed.number];
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
        EventManager.OnExitGameOverScreen?.Invoke();
      }
      yield return null;
    }
  }
}
