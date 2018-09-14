using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
  [SerializeField]
  private GameVariables mainData;
  [SerializeField]
  private GameVariables testData;
  private GameVariables data => !Application.isEditor ? mainData : testData ?? mainData;

  public static GameVariables Data => Instance.data;
}
