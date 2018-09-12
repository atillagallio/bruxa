using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Characters/New Character", order = 1)]
public class Character : ScriptableObject
{
  public string Name;
  public Color Color;
  public GameObject InGameRepresentation;
  public GameOverCharacterRepresentation GameOverRepresentation;
}