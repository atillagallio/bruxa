using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterSelectionScreen
{

  public class CharacterSelectionModel : MonoBehaviour
  {

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private bool stop;

    private float stopAngle;
    private float stopEpsilon = 0.05f;

    public CharacterSelectionRepresentation Character;

    // Use this for initialization
    void Start()
    {
      stopAngle = transform.rotation.y;
    }
    public static float Clamp0360(float eulerAngles)
    {
      float result = eulerAngles - Mathf.CeilToInt(eulerAngles / 360f) * 360f;
      if (result < 0)
      {
        result += 360f;
      }
      return result;
    }

    // Update is called once per frame
    void Update()
    {
      if (stop)
      {
        if (Clamp0360(Mathf.Abs(transform.rotation.y - stopAngle)) < stopEpsilon)
        {
          transform.rotation = Quaternion.AngleAxis(stopAngle, Vector3.up);
          return;
        }
      }
      transform.rotation = Quaternion.AngleAxis(Time.time * rotationSpeed, Vector3.up);
    }

    public void ToggleSelect()
    {
      stop = !stop;
      Character.ToggleSelected();
    }
  }
}