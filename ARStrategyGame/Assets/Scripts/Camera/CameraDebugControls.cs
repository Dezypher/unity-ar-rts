using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDebugControls : MonoBehaviour {

	public float moveSpeed;
	public float rushSpeedMultiplier;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float rushSpeed = 1;

		if(Input.GetKey(KeyCode.LeftAlt)) {
			rushSpeed = rushSpeedMultiplier;
		}
		transform.Translate(Input.GetAxis("Horizontal") * moveSpeed * rushSpeed,
							Input.GetAxis("UpDown") * moveSpeed * rushSpeed,
							Input.GetAxis("Vertical") * moveSpeed * rushSpeed);
	}
}
