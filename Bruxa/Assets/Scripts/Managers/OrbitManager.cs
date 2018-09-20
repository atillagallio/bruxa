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

  [SerializeField]
  private List<Color> testColors;

  // Use this for initialization
  void Start()
  {

    playerList = new List<PlayerBehaviour>();
    orbits = new List<Orbitable>();
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
    testColors = new List<Color>();
    foreach (var player in players)
    {

      Character charInfo = player.GetComponent<PlayerBehaviour>().CharacterInfo;
      var obj = Instantiate(orbitablePrefab, playerPos.position, Quaternion.identity);
      obj.transform.SetParent(playerPos);
      var orbComponent = obj.GetComponent<Orbitable>();
      orbComponent.Phase = currDegree * Mathf.Deg2Rad;
      orbComponent.FaceSpriteRenderer.sprite = charInfo.UIFace;

      var colorModule = orbComponent.trailParticle.colorOverLifetime;

      colorModule.color = GradientColorfy(colorModule, charInfo.Color);

      orbits.Add(orbComponent);
      playerList.Add(player);
      currDegree += dgSpace;
    }
  }

  public Gradient GradientColorfy(ParticleSystem.ColorOverLifetimeModule main, Color newColor)
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

    return SetColorGradient(myNewColorBefore, myNewColorAfter);
  }

  public Gradient SetColorGradient(Color before, Color after)
  {
    Gradient ourGradient = new Gradient();
    ourGradient.SetKeys(
        new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(before, 0.49f), new GradientColorKey(after, 0.98f) },
        new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.87f), new GradientAlphaKey(0f, 1.0f) }
        );
    return ourGradient;
  }
}
