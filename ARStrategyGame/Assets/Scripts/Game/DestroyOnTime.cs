using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTime : MonoBehaviour {

	public float timeAlive;
	
	// Update is called once per frame
	void Update () {
		timeAlive -= Time.deltaTime;

		if(timeAlive <= 0) {
			Destroy(gameObject);
		}
	}
}
