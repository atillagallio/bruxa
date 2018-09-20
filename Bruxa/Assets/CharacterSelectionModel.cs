using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionModel : MonoBehaviour
{

  [SerializeField]
  private float rotationSpeed;
  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    transform.rotation = Quaternion.AngleAxis(Time.time * rotationSpeed, Vector3.up);
  }
}
