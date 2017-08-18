using UnityEngine;
using System.Collections;

public class CursorLock : MonoBehaviour {
	public bool locked;

	void Start() {
		Cursor.lockState = CursorLockMode.Locked;
		//locked = true;
		Cursor.visible = false;
	}

	void Update() {
		if (Input.GetMouseButton(1)) {
			locked = true;
		} else {
			locked = false;
		}

		if (locked) {
			Cursor.lockState = CursorLockMode.Locked;
		} else {
			Cursor.lockState = CursorLockMode.None;
		}

		//Cursor.visible = (CursorLockMode.Locked != wantedMode);
	}

	public void Lock(bool locked) {
		this.locked = locked;
	}

}
