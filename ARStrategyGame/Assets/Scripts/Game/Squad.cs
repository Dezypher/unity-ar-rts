using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Squad : MonoBehaviour {

	public enum Formation {Random, Line, Triangle};

	public Formation formation;
	public List<Unit> units;
	public Transform targetGizmo;
	public float minDistance;
	public Vector3 targetPos;

	public Transform squadIcon;

	public int maxInLine;

	public float unitSize = 1;

	public bool isDead = false;

	void Start() {
		targetGizmo.gameObject.SetActive(false);
		targetPos = Vector3.zero;

		GetChildUnits();
		SetSquadOnUnits();
	}

	public void GetChildUnits() {
		for(int i = 0; i < transform.childCount; i++) {
			units.Add(transform.GetChild(i).GetComponent<Unit>());
		}
	}

	void Update() {
		if(!isDead && units.Count > 0) {
			float sumX = 0;
			float sumY = 0;
			float sumZ = 0;

			for(int i = 0; i < units.Count; i++) {
				sumX += units[i].transform.position.x;
				sumY += units[i].transform.position.y;
				sumZ += units[i].transform.position.z;
			}

			Vector3 avgPos = new Vector3(
				sumX/units.Count,
				sumY/units.Count,
				sumZ/units.Count	
			);

			if(Vector3.Distance(targetPos, avgPos) <= minDistance) {
				targetGizmo.gameObject.SetActive(false);
			}

			squadIcon.position = avgPos;
		} else {
			if(targetGizmo.gameObject.activeSelf) {
				targetGizmo.gameObject.SetActive(false);
			}

			if(squadIcon.gameObject.activeSelf) {
				squadIcon.gameObject.SetActive(false);
			}
		}
	}

	public void SetSquadOnUnits() {
		for(int i = 0; i < units.Count; i++) {
			units[i].squad = this;
		}
	}

	public void SetTarget(Vector3 pos) {
		/*
		Make a better algo for clumping units, for now will
		give units random positions in a circle
		*/

		float radius = Mathf.Log(units.Count * 2, 2);

		Vector3 direction = pos - squadIcon.position;
 		Vector3 left = Vector3.Cross(direction, Vector3.up).normalized;
		//Debug.Log(left);
		//Debug.DrawLine(pos, pos + (left * 3), Color.white, 100, false);
		//Debug.DrawLine(targetPos, pos, Color.green, 100, false);

		for(int i = 0; i < units.Count; i++) {
			if(!units[i].isDead) {
				if(formation == Formation.Random) {
					// Place them randomly in a circle
					Vector3 random = (Random.insideUnitCircle * radius);
					units[i].MoveTo(new Vector3(
						pos.x + random.x,
						pos.y + 0.01f,
						pos.z + random.y	
					));
				}else if(formation == Formation.Line) {
					// Form them in a line
					int column = i%maxInLine;
					int row = i/maxInLine;
					int unitsInCurrentRow = (units.Count - (maxInLine * row));
					if(unitsInCurrentRow >= maxInLine)
						unitsInCurrentRow = maxInLine;

					int numRows = units.Count/maxInLine + 1;
					if(unitsInCurrentRow == 0)
						unitsInCurrentRow = maxInLine;
					//Debug.Log(unitsInCurrentRow);
					Vector3 dirUnit = direction.normalized;

					float xDiff = ((float)(unitsInCurrentRow-1))/-2 * unitSize + column * unitSize;
					float zDiff = ((float)(numRows - 1))/-2 * unitSize + row * unitSize;

					//Debug.Log("xDiff = " + xDiff + ", zDiff = " + zDiff + ", numRows = " + numRows + ", unitsInCurrentRow = " + unitsInCurrentRow + ", row = " + row + ", column = " + column);

					units[i].MoveTo((pos - (dirUnit * zDiff)) + (left * xDiff));
				}
			}
		}

		targetGizmo.gameObject.SetActive(true);
		targetGizmo.position = new Vector3(
				pos.x,
				pos.y + 0.01f,
				pos.z	
			);

		targetPos = pos;
	}

	public void TeleportTarget(Vector3 pos) {
		/*
		Make a better algo for clumping units, for now will
		give units random positions in a circle
		*/

		float radius = Mathf.Log(units.Count * 2, 2);

		Vector3 direction = Vector3.forward;
 		Vector3 left = Vector3.Cross(direction, Vector3.up).normalized;
		//Debug.Log(left);
		//Debug.DrawLine(pos, pos + (left * 3), Color.white, 100, false);
		//Debug.DrawLine(targetPos, pos, Color.green, 100, false);

		for(int i = 0; i < units.Count; i++) {
			if(!units[i].isDead) {
				if(formation == Formation.Random) {
					// Place them randomly in a circle
					Vector3 random = (Random.insideUnitCircle * radius);
					units[i].MoveTo(new Vector3(
						pos.x + random.x,
						pos.y + 0.01f,
						pos.z + random.y	
					));
				}else if(formation == Formation.Line) {
					// Form them in a line
					int column = i%maxInLine;
					int row = i/maxInLine;
					int unitsInCurrentRow = (units.Count - (maxInLine * row));
					if(unitsInCurrentRow >= maxInLine)
						unitsInCurrentRow = maxInLine;

					int numRows = units.Count/maxInLine + 1;
					if(unitsInCurrentRow == 0)
						unitsInCurrentRow = maxInLine;
					//Debug.Log(unitsInCurrentRow);
					Vector3 dirUnit = direction.normalized;

					float xDiff = ((float)(unitsInCurrentRow-1))/-2 * unitSize + column * unitSize;
					float zDiff = ((float)(numRows - 1))/-2 * unitSize + row * unitSize;

					//Debug.Log("xDiff = " + xDiff + ", zDiff = " + zDiff + ", numRows = " + numRows + ", unitsInCurrentRow = " + unitsInCurrentRow + ", row = " + row + ", column = " + column);

					units[i].transform.position = (pos - (dirUnit * zDiff)) + (left * xDiff);
				}
			}
		}

		targetPos = pos;
	}

	public void RemoveUnit(Unit unit) {
		units.Remove(unit);

		if(units.Count <= 0) {
			isDead = true;
		}
	}
}