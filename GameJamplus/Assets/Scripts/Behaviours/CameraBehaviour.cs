using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraBehaviour : MonoBehaviour
{


  private float cameraDifferenceToPlayersY;
  private float cameraDifferenceToPlayersZ;

  private Vector3 cameraDirection;
  public float cameraLerp;
  public float cameraDistance;

  public Transform playerPos;
  // Use this for initialization
  void Start()
  {
    cameraDirection = (transform.position - playerPos.position).normalized;
  }

  // Update is called once per frame
  void Update()
  {
    Vector3 newPos = new Vector3(playerPos.position.x, playerPos.position.y + cameraDifferenceToPlayersY,
      playerPos.position.z + cameraDifferenceToPlayersZ);

    this.transform.position = playerPos.position + cameraDirection * cameraDistance;

  }
}
