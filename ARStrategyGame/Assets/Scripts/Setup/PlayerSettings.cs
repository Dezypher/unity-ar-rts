using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour {

	public static PlayerSettings Instance;
	
	public string hostIP = "localhost";
	public string playerName;
	public int numSword = 0;
	public int numAxe = 0;
	public int numSpear = 0;

	// Use this for initialization
	void Start () {
		if(Instance != null)
			DontDestroyOnLoad(gameObject);
		else Instance = this;
	}
}
