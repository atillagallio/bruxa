using UnityEngine;
using System.Collections;

public enum GameEndConditionMode
{
  time,
  score,
  both,
}

[System.Serializable]
public struct GameEndCondition
{
  public GameEndConditionMode Mode;
  public float MatchDuration;
  public float ScoreToWin;
}

[CreateAssetMenu(fileName = "Data", menuName = "Data/GameVariables", order = 1)]
public class GameVariables : ScriptableObject
{
  [Header("End Condition")]
  public GameEndCondition EndCondition;

  [Header("Parry")]
  public float InitialParryDelay;
  public float ParryDuration;

  [Header("Cooldowns")]
  public float ParryCooldown;
  public float SwitchCooldown;

  [Header("Misc")]
  public DrunknessType Skill5DrunkType;



}