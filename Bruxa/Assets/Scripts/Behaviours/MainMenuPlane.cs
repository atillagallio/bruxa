using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlane : MonoBehaviour {

	// Use this for initialization
	int i = 1;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log(other);
		if(other.gameObject.tag == "Player"){
			if(i == 0)
				other.transform.GetChild(3).gameObject.SetActive(false);
			else
				other.transform.GetChild(i-1).gameObject.SetActive(false);
			other.transform.GetChild(4).gameObject.GetComponentInChildren<ParticleSystem>().Play();
			StartCoroutine(TurnParticleOff(other.transform.GetChild(4).gameObject.GetComponentInChildren<ParticleSystem>()));
			other.transform.GetChild(i).gameObject.SetActive(true);
			i++;
			if(i>3)
				i = 0;
		}
	}

	private IEnumerator TurnParticleOff(ParticleSystem particle){
		yield return new WaitForSeconds(0.1f);
		particle.Stop();
	}
}
