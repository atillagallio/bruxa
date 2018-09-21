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
  ExplosionGlow,
  DestroySphere,

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
    EventManager.OnPlayerLeavingWitch += LosingWitchControl;
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

      var explosionColorModule = orbComponent.explosionParticle.colorOverLifetime;

      List<Color> colorList = GetColors(explosionColorModule, charInfo.Color);
      foreach (ParticleSystem ptSystem in orbComponent.GetComponentsInChildren<ParticleSystem>())
      {
        var destroySphereModule = ptSystem.colorOverLifetime;
        destroySphereModule.color = GradientReplace(destroySphereModule.color.gradient, colorList);

      }

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
    orbits[GetPlayerPos(player)].IsControllingWitch = true;
    orbits[GetPlayerPos(player)].GetWitchControl();
  }

  public void LosingWitchControl(PlayerBehaviour player)
  {
    orbits[GetPlayerPos(player)].IsControllingWitch = false;
    orbits[GetPlayerPos(player)].cdTime = 0;
  }

  public void FailingToGetControl(PlayerBehaviour player)
  {
    orbits[GetPlayerPos(player)].GetBlockedByWitch();
  }

  public List<Color> GetColors(ParticleSystem.ColorOverLifetimeModule main, Color newColor)
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

    return new List<Color> {
      myNewColorBefore,
      myNewColorAfter,
      };
  }
  public Gradient GradientReplace(Gradient gradBefore, List<Color> colors)
  {

    int i = 0;
    int z = 0;
    GradientColorKey[] newColorKeyArray = new GradientColorKey[gradBefore.colorKeys.Length];
    foreach (var keyGrad in gradBefore.colorKeys)
    {
      if (keyGrad.color != Color.white && z <= gradBefore.colorKeys.Length)
      {
        GradientColorKey newColorKey = new GradientColorKey(colors[z], keyGrad.time);
        gradBefore.colorKeys[i].color = colors[z];
        z++;
        newColorKeyArray[i] = newColorKey;
      }
      else
      {
        newColorKeyArray[i] = keyGrad;
      }

      i++;
    }
    gradBefore.SetKeys(newColorKeyArray, gradBefore.alphaKeys);
    return gradBefore;

  }
}
