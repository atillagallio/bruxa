using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {


	private float cameraDifferenceToPlayersY;
	private float cameraDifferenceToPlayersZ;
	public float cameraLerp = 3f;
	public Transform playerPos;
	// Use this for initialization
	void Start () {
		cameraDifferenceToPlayersY = this.transform.position.y - playerPos.position.y;
		cameraDifferenceToPlayersZ = this.transform.position.z - playerPos.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPos = new Vector3(playerPos.position.x, playerPos.position.y+ cameraDifferenceToPlayersY, 
			playerPos.position.z+ cameraDifferenceToPlayersZ);

			this.transform.position = Vector3.Lerp(this.transform.position, newPos, cameraLerp * Time.deltaTime);
		
	}
}
