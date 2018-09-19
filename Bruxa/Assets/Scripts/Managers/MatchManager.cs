using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
  public bool GameStarted
  {
    get; private set;
  }

  public bool GameFinished
  {
    get; private set;
  }

  public float Countdown { get; private set; }
  public float MatchDuration { get; private set; }

  private List<PlayerBehaviour> players => InGameManager.Instance.players;

  private bool startedCountdown;
  public void StartMatch()
  {
    if (!startedCountdown)
    {
      StartCoroutine(MatchCountDown());
      startedCountdown = true;
    }
  }

  private IEnumerator MatchCountDown()
  {
    Countdown = 3.0f;
    while (Countdown > 0)
    {
      yield return null;
      Countdown -= Time.deltaTime;
      if (Countdown <= 1)
      {
        GameStarted = true;
      }
    }

    StartCoroutine(MatchTimer());
  }

  private IEnumerator MatchTimer()
  {
    GameEndCondition endCondition = GameDataManager.Data.EndCondition;
    MatchDuration = 0;

    Func<bool> TestForDuration = () => MatchDuration > endCondition.MatchDuration;
    Func<bool> TestForScore = () => players.Aggregate(false, (test, p) => test || p.GetComponent<PlayerBehaviour>().Points >= endCondition.ScoreToWin);
    Func<bool> GameOver = () => true;
    switch (endCondition.Mode)
    {
      case GameEndConditionMode.score:
        GameOver = TestForScore;
        break;
      case GameEndConditionMode.time:
        GameOver = TestForDuration;
        break;
      case GameEndConditionMode.both:
        GameOver = () => TestForDuration() || TestForScore();
        break;
      default:
        print("WEIRD CASE!!!");
        break;
    }

    while (!GameOver())
    {
      yield return null;
      MatchDuration += Time.deltaTime;
      GameUIManager.Instance.UpdateTimer((int)MatchDuration);
    }

    GameFinished = true;
    GameOverPlayerDataContainer.SetData(players);
    UnityEngine.SceneManagement.SceneManager.LoadScene(GameOverSceneName);
    //endGameCanvas.GetComponentInChildren<TextMeshProUGUI>().text = txt;
  }

  [SerializeField]
  private string GameOverSceneName;
}