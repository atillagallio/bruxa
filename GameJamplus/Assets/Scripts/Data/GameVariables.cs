using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Data", menuName = "Data/GameVariables", order = 1)]
public class GameVariables : ScriptableObject
{
  public DrunknessType Skill5DrunkType;
  public float InitialParryDelay;
  public int MatchDuration;

  public float ParryDuration;

  [Header("Cooldowns")]
  public float ParryCooldown;
  public float SwitchCooldown;
}