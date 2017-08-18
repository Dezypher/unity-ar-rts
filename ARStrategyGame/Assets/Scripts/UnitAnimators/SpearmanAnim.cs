using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpearmanAnim : MonoBehaviour {

	public float speedThreshold;

	private Animator animator;
	private NavMeshAgent agent;
	private Unit unit;

	// Use this for initialization
	void Start () {
		animator = transform.FindChild("model").gameObject.GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
		unit = GetComponent<Unit>();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(agent.velocity);

		if(agent.velocity.x != 0 || 
			agent.velocity.y != 0 ||
			agent.velocity.z != 0) {
				animator.SetBool("Walking", true);
		} else {
			animator.SetBool("Walking", false);
		}

		if(unit.hasTarget) {
			animator.SetBool("Charging", true);
		} else {
			animator.SetBool("Charging", false);
		}
	}
}
