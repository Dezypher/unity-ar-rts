using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AxemanAnim : MonoBehaviour {

	public float speedThreshold;

	private Animator animator;
	private NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		animator = transform.FindChild("model").gameObject.GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
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
	}
}
