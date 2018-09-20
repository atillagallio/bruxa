using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitManager : Singleton<OrbitManager>
{

  private List<PlayerBehaviour> playerList;
  private List<Orbitable> orbits;
  [Header("Player Info")]
  public Transform playerPos;

  [Header("Prefab")]
  public GameObject orbitablePrefab;

  [Header("Orbit Variables")]
  public float radius;
  public float rotationSpeed;

  public float NoiseVelocity;
  public float NoiseAmplitude;

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
    foreach (var player in players)
    {

      Character charInfo = player.GetComponent<PlayerBehaviour>().CharacterInfo;
      var obj = Instantiate(orbitablePrefab, playerPos.position, Quaternion.identity);
      obj.transform.SetParent(playerPos);
      obj.GetComponent<Orbitable>().playerPos = playerPos;
      obj.GetComponent<Orbitable>().Phase = currDegree * Mathf.Deg2Rad;
      orbits.Add(obj.GetComponent<Orbitable>());
      playerList.Add(player);
      currDegree += dgSpace;
    }
  }
}
