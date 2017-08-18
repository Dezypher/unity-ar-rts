using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpacityOnCamera : MonoBehaviour {


	public float nearDistance;
	public float minDistance;
	public Transform playerCamera;

	void Start() {
		playerCamera = GameObject.Find("PlayerCamera").transform;
	}

	void Update () {
		float cameraDistance = Vector3.Distance(transform.position, playerCamera.position);
		float newAlpha = 1;

		if(cameraDistance <= minDistance) {
			newAlpha = 0;
		} else if(cameraDistance <= nearDistance) {
			newAlpha = cameraDistance/nearDistance;
		}
		
		Color oldColor = GetComponent<Renderer>().material.color;
		GetComponent<Renderer>().material.color = new Color(oldColor.r, oldColor.g, oldColor.b, newAlpha);
	}
}
