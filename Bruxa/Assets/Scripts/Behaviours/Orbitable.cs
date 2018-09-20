using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbitable : MonoBehaviour
{

  public Transform playerPos;
  private float radius;
  private float rotationSpeed;
  public float Phase
  {
    get { return _phase; }
    set
    {
      StartCoroutine(PhaseLerp(value));
    }
  }
  [SerializeField]
  private float _phase;
  // Use this for initialization

  private float randomSeed;
  private float noiseVelocity;
  private float noiseAmplitude;
  void Start()
  {
    var orbitManager = OrbitManager.Instance;
    radius = orbitManager.radius;
    rotationSpeed = orbitManager.rotationSpeed;
    noiseVelocity = orbitManager.NoiseVelocity;
    noiseAmplitude = orbitManager.NoiseAmplitude;

    randomSeed = Random.Range(0f, 1f);
  }

  // Update is called once per frame
  void Update()
  {

    var obtTransform = gameObject.transform;
    obtTransform.position = playerPos.position + new Vector3(Noise(radius) * Mathf.Cos(rotationSpeed * Time.time + _phase), 0, radius * Mathf.Sin(rotationSpeed * Time.time + _phase));

  }

  float Noise(float radius)
  {
    float value = Mathf.PerlinNoise(Time.time * noiseVelocity, randomSeed);
    value = (value - 0.5f) * 2f;
    return radius + noiseAmplitude * value;

  }

  [ContextMenu("Set Phase")]
  void ChangePhase()
  {
    Phase = TargetPhase;
  }
  public float TargetPhase;

  IEnumerator PhaseLerp(float targetPhase)
  {
    //print(targetPhase);
    var time = 1f;

    return CoroutineHelpers.InterpolateByTime(time, (k =>
    {
      //print(_phase);
      _phase = Mathf.Lerp(0, targetPhase, k);
    }));

  }

}
