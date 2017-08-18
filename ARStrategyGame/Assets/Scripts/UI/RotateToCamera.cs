using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCamera : MonoBehaviour {

	public Camera cameraToFollow;
	public bool getMainCam = true;

	void Awake() {
		if(!getMainCam) {
			cameraToFollow = GameObject.Find("PlayerCamera").GetComponent<Camera>();
		}
	}

	// Update is called once per frame
	void Update () {
		if(getMainCam)
			transform.LookAt(Camera.main.gameObject.transform);
		else {
			transform.LookAt(cameraToFollow.gameObject.transform);
		}
	}
}
