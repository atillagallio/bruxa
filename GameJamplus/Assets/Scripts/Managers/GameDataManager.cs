using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
  [SerializeField]
  private GameVariables data;

  public GameVariables Data => data;
}
