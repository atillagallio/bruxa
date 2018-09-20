using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSV
{
  public float h;
  public float s;
  public float v;

  public void PrintHSV()
  {
    Debug.Log($"HSV({h} , {s} , {v})");
  }
}

public enum ParticleType
{
  SphereTrail,
  Explosion,
  ExplosionGlow

}
public class OrbitManager : Singleton<OrbitManager>
{

  private List<PlayerBehaviour> playerList;
  private List<Orbitable> orbits;
  [Header("Player Info")]
  public Transform playerPos;

  [Header("Prefab")]
  public GameObject orbitablePrefab;

  [Header("Orbit Variables")]
  public float Radius;
  public float RotationSpeed;
  public float SphereY;
  public float NoiseVelocity;
  public float NoiseAmplitude;

  [Header("Orbit Gameplay Variables")]
  public float orbSpawnTimer;
  public float orbControlAnimationTimer;


  // Use this for initialization
  void Start()
  {

    playerList = new List<PlayerBehaviour>();
    orbits = new List<Orbitable>();

    EventManager.OnTryingToGetControl += UsingSwitchButton;
    EventManager.OnFailingToGetControl += FailingToGetControl;
    EventManager.OnPlayerEnteringWitch += GettingWitchControl;
  }

  // Update is called once per frame
  void Update()
  {


  }



  public void InstantiateOrbits(List<PlayerBehaviour> players)
  {
    //grau
    float dgSpace = 360f / players.Count;
    float currDegree = 0;

    foreach (var player in players)
    {

      Character charInfo = player.GetComponent<PlayerBehaviour>().CharacterInfo;
      var obj = Instantiate(orbitablePrefab, playerPos.position, Quaternion.identity);
      obj.transform.SetParent(playerPos);
      var orbComponent = obj.GetComponent<Orbitable>();
      orbComponent.Phase = currDegree * Mathf.Deg2Rad;
      orbComponent.FaceSpriteRenderer.sprite = charInfo.UIFace;

      var trailColorModule = orbComponent.trailParticle.colorOverLifetime;
      trailColorModule.color = GradientColorfy(trailColorModule, charInfo.Color, ParticleType.SphereTrail);
      var explosionColorModule = orbComponent.explosionParticle.colorOverLifetime;
      explosionColorModule.color = GradientColorfy(explosionColorModule, charInfo.Color, ParticleType.Explosion);
      var explosionGlowClorModule = orbComponent.explosionGlowParticle.colorOverLifetime;
      explosionGlowClorModule.color = GradientColorfy(explosionColorModule, charInfo.Color, ParticleType.ExplosionGlow);

      orbits.Add(orbComponent);
      playerList.Add(player);
      currDegree += dgSpace;
    }
  }

  private int GetPlayerPos(PlayerBehaviour player)
  {
    return playerList.FindIndex(_player => _player == player);
  }
  public void UsingSwitchButton(PlayerBehaviour player)
  {
    orbits[GetPlayerPos(player)].MoveToWitch();
  }

  public void GettingWitchControl(PlayerBehaviour player)
  {
    orbits[GetPlayerPos(player)].GetWitchControl();
  }
  public void FailingToGetControl(PlayerBehaviour player)
  {
    orbits[GetPlayerPos(player)].GetBlockedByWitch();
  }
  public Gradient GradientColorfy(ParticleSystem.ColorOverLifetimeModule main, Color newColor, ParticleType pt)
  {

    HSV myColor = new HSV();
    HSV colorBefore = new HSV();
    HSV colorAfter = new HSV();
    Color.RGBToHSV(main.color.gradient.colorKeys[2].color, out colorAfter.h, out colorAfter.s, out colorAfter.v);
    Color.RGBToHSV(main.color.gradient.colorKeys[1].color, out colorBefore.h, out colorBefore.s, out colorBefore.v);
    Color.RGBToHSV(newColor, out myColor.h, out myColor.s, out myColor.v);
    float[] hueDif = { colorAfter.h - colorBefore.h, colorAfter.s - colorBefore.s, colorAfter.v - colorBefore.v };
    Color myNewColorBefore = Color.HSVToRGB(myColor.h, colorBefore.s, colorBefore.v);
    Color myNewColorAfter = Color.HSVToRGB(myColor.h + hueDif[0], colorAfter.s, colorAfter.v);

    return SetColorGradient(myNewColorBefore, myNewColorAfter, pt);
  }

  public Gradient SetColorGradient(Color before, Color after, ParticleType pt)
  {
    Gradient ourGradient = new Gradient();
    if (pt == ParticleType.Explosion || pt == ParticleType.SphereTrail)
    {
      ourGradient.SetKeys(
          new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(before, 0.49f), new GradientColorKey(after, 0.98f) },
          new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.87f), new GradientAlphaKey(0f, 1.0f) }
          );
    }
    if (pt == ParticleType.ExplosionGlow)
    {
      ourGradient.SetKeys(
      new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(before, 0.49f), new GradientColorKey(after, 0.98f) },
      new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1.0f) }
      );
    }
    return ourGradient;
  }
}
