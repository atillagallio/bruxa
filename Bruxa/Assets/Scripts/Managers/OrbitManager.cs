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

  private List<PlayerBehaviour> playerList => InGameManager.Players;
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
    orbits = new List<Orbitable>();
    EventManager.OnTryingToGetControl += UsingSwitchButton;
    EventManager.OnFailingToGetControl += FailingToGetControl;
    EventManager.OnPlayerEnteringWitch += GettingWitchControl;
    EventManager.OnPlayerLeavingWitch += LosingWitchControl;
    EventManager.OnPlayerUsingItem += UsingItem;
    EventManager.OnPlayerGettingItem += PlayerGotItem;
  }

  void OnDestroy()
  {
    EventManager.OnTryingToGetControl -= UsingSwitchButton;
    EventManager.OnFailingToGetControl -= FailingToGetControl;
    EventManager.OnPlayerEnteringWitch -= GettingWitchControl;
    EventManager.OnPlayerLeavingWitch -= LosingWitchControl;
    EventManager.OnPlayerUsingItem -= UsingItem;
    EventManager.OnPlayerGettingItem -= PlayerGotItem;
  }

  // Update is called once per frame
  void Update()
  {
  }


  public void PlayerGotItem(PlayerBehaviour player)
  {
    orbits[GetPlayerPos(player)].SetItemOnSphere(player.Spell.spellIcon);
  }

  public void InstantiateOrbits(List<PlayerBehaviour> players)
  {
    //grau
    float dgSpace = 360f / players.Count;
    float currDegree = 0;

    orbits = new List<Orbitable>();
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
      orbComponent.usingItemParticle.gameObject.SetActive(false);


      orbits.Add(orbComponent);
      currDegree += dgSpace;
    }
  }

  private int GetPlayerPos(PlayerBehaviour player)
  {
    return playerList.FindIndex(_player => _player == player);
  }

  public void UsingItem(PlayerBehaviour player)
  {
    print("using Item");
    if (!orbits[GetPlayerPos(player)].IsControllingWitch)
    {
      orbits[GetPlayerPos(player)].UseItemEffect();
    }
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
    GradientColorKey[] auxColorKey = gradBefore.colorKeys;

    foreach (var keyGrad in gradBefore.colorKeys)
    {
      if (keyGrad.color != Color.white && keyGrad.color != Color.black && z <= gradBefore.colorKeys.Length)
      {
        if (z < colors.Count)
        {
          GradientColorKey newColorKey = new GradientColorKey(colors[z], keyGrad.time);
          gradBefore.colorKeys[i].color = colors[z];
          newColorKeyArray[i] = newColorKey;
          z++;
        }
      }
      else
      {
        newColorKeyArray[i] = keyGrad;
      }

      i++;
    }
    if (newColorKeyArray.Length <= 1)
    {
      newColorKeyArray = auxColorKey;
    }
    gradBefore.SetKeys(newColorKeyArray, gradBefore.alphaKeys);
    return gradBefore;

  }
}
