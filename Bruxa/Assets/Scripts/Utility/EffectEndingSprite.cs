using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectEndingSprite : MonoBehaviour {
     Vector3 m_from = new Vector3(0f, 0F, -5.0F);
    Vector3 m_to = new Vector3(0F, 0.0F, 5.0F);
    float m_frequency = 0;
	// Use this for initialization
	void Start () {
		 m_frequency = Random.Range(0.25f,0.75F);
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion from = Quaternion.Euler(this.m_from);
        Quaternion to = Quaternion.Euler(this.m_to);
 
        float lerp = 0.5F * (1.0F + Mathf.Sin(Mathf.PI * Time.realtimeSinceStartup * this.m_frequency));
        this.transform.localRotation = Quaternion.Lerp(from, to, lerp);
	}
}
