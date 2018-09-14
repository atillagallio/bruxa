using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Characters/New Character", order = 1)]
public class Character : ScriptableObject
{
  public string Nome;
  public Color Color;
  public GameObject InGameRepresentation;
  public GameOverCharacterRepresentation GameOverRepresentation;
  public GameObject OrbitObject;

  [Header("UI Imgs")]
  public Sprite UIFace;
  public Sprite UICDfill;
  public Sprite UIBG;
}