using UnityEngine;
using CharacterSelectionScreen;

[CreateAssetMenu(fileName = "New Character", menuName = "Characters/New Character", order = 1)]
public class Character : ScriptableObject
{
  public string Nome;
  new public string name => Nome;
  public Color Color;
  public InGameCharacterRepresentation InGameRepresentation;
  public GameOverCharacterRepresentation GameOverRepresentation;

  [Header("Character Selection")]
  public CharacterSelectionRepresentation CharacterSelectionRepresentation;

  [Header("UI Imgs")]
  public Sprite UIFace;
  public Sprite UICDfill;
  public Sprite UIBG;
}