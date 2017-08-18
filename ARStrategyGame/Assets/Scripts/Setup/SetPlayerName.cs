using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerName : MonoBehaviour {

	public InputField playerNameField;

	public void SetName() {
		PlayerSettings.Instance.playerName = playerNameField.text;
	}
}
