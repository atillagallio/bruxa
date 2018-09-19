using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSprite : MonoBehaviour
{

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

    transform.LookAt(Camera.main.transform);
    transform.Rotate(0, 180, 0);
  }
}
