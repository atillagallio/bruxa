[System.Serializable]
public class JoinedPlayerData
{
  public readonly Rewired.Player RewiredPlayer;
  public readonly Character CharacterInfo;

  public JoinedPlayerData(Rewired.Player rewiredPlayer, Character characterInfo)
  {
    RewiredPlayer = rewiredPlayer;
    CharacterInfo = characterInfo;
  }
}