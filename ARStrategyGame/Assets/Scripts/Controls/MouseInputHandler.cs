using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class MouseInputHandler : MonoBehaviour {

	public const int UNIT_TYPE_SWORD = 1;
	public const int UNIT_TYPE_AXE = 2;
	public const int UNIT_TYPE_SPEAR = 3;

	public Camera playerCamera;

	public Sprite[] icons;
	public Vector2[] iconSizes;
	
	public Image cursorIcon;
	public bool canClick = true;
	public bool useCursor = true;
	public bool tapped = false;

	public Squad squadSword;
	public Squad squadAxe;
	public Squad squadSpear;

	public Button swordButton;
	public Button axeButton;
	public Button spearButton;
	public int unitTypeSelected;

	// Update is called once per frame
	void Update () {
		Squad squad = squadSword;

		switch(unitTypeSelected) {
			case UNIT_TYPE_SWORD:
			squad = squadSword;
			break;
			case UNIT_TYPE_AXE:
			squad = squadAxe;
			break;
			case UNIT_TYPE_SPEAR:
			squad = squadSpear;
			break;

		}

		if(canClick) {
			RaycastHit hit = new RaycastHit();
			bool hasHit = false;
			Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
			int layerMask = LayerMask.GetMask("Terrain");

			if(Input.mousePosition.y > (Screen.height * 0.3f)) {
				if (useCursor && Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
					hasHit = true;
				} else if (!useCursor && Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, Mathf.Infinity, layerMask)) {
					hasHit = true;
				} else {
					cursorIcon.sprite = icons[0];
					cursorIcon.rectTransform.localScale = iconSizes[0];
				}

				if(hasHit) {
					if(hit.transform.tag == "Terrain") {
						cursorIcon.sprite = icons[1];
						cursorIcon.rectTransform.localScale = iconSizes[1];

						if(useCursor && (Input.GetButtonDown("Fire1") || Input.touchCount > 0)) {
							squad.SetTarget(hit.point);
						} else if (!useCursor && tapped) {
							tapped = false;
							squad.SetTarget(hit.point);
						}
					}
				}
			}
 		} else {
			cursorIcon.sprite = icons[0];
			cursorIcon.rectTransform.localScale = iconSizes[0];
		}

	}

	public void MoveCenter() {
		Squad squad = squadSword;

		switch(unitTypeSelected) {
			case UNIT_TYPE_SWORD:
			squad = squadSword;
			break;
			case UNIT_TYPE_AXE:
			squad = squadAxe;
			break;
			case UNIT_TYPE_SPEAR:
			squad = squadSpear;
			break;
		}

		RaycastHit hit = new RaycastHit();
		bool hasHit = false;
		Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
		int layerMask = LayerMask.GetMask("Terrain");

			if (useCursor && Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
				hasHit = true;
			} else if (!useCursor && Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, Mathf.Infinity, layerMask)) {
				hasHit = true;
			} else {
				cursorIcon.sprite = icons[0];
				cursorIcon.rectTransform.localScale = iconSizes[0];
			}

			if(hasHit) {
				if(hit.transform.tag == "Terrain") {
					cursorIcon.sprite = icons[1];
					cursorIcon.rectTransform.localScale = iconSizes[1];

					if(useCursor && (Input.GetButtonDown("Fire1") || Input.touchCount > 0)) {
						squad.SetTarget(hit.point);
					} else if (!useCursor && tapped) {
						tapped = false;
						squad.SetTarget(hit.point);
					}
				}
			}
	}
	
	public void Tap() {
		tapped = true;
	}

	public void SetCanClick(bool canClick) {
		this.canClick = canClick;
	}

	public void SetSelectedUnitType(int type) {
		unitTypeSelected = type;

		swordButton.interactable = true;
		axeButton.interactable = true;
		spearButton.interactable = true;
		
		switch(type) {
			case UNIT_TYPE_SWORD:
			swordButton.interactable = false;
			break;
			case UNIT_TYPE_AXE:
			axeButton.interactable = false;
			break;
			case UNIT_TYPE_SPEAR:
			spearButton.interactable = false;
			break;
		}
	}
}
