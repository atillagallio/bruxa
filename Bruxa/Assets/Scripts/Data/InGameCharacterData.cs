using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "InGameCharacterData", menuName = "Data/InGameCharacter", order = 1)]
public class InGameCharacterData : ScriptableObject
{
  public float HitForce;
  public float ForwardSpeed;
  public float RotationRadius;
  public float Gravity;
  public float SlowSpellVel;
  public float BombExplosionImpulse;
  public float BombStunTime;
  public float BombRotationSpeed;
  public bool BombIsMathematical;
}