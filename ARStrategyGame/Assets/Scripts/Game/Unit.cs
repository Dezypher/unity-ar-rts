using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour {

    public enum UnitType {Spear, Axe, Knight};

    public int teamID;
    public UnitType unitType;
    public int currentHealth;
    public int maxHealth;
    public float maxStrayDistance;
    public DamageModifier[] damageModifiers;
    public float attackInterval;
    public float attackRange;
    public float attackDelay;
    public int damage;
    public bool isAttacking = false;
    public bool hasTarget = false;
    public Animator animator;
    public bool isDead = false;
    public Squad squad;
    public Rigidbody[] ragdollParts;
    public GameObject[] disableOnDeath;

    private float overrideTime = 2;
    private Vector3 origTarget;
    private Unit enemyTarget;
    private NavMeshAgent agent;
    private float currOverrideTime = 0;
    private float currAttackInterval = 0;
    
    // Use this for initialization
	void Start () {
        this.currentHealth = this.maxHealth;
        animator = transform.FindChild("model").GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        origTarget = transform.position;

        ActivateRagdoll(false);
    }
    
    public void ActivateRagdoll(bool activate) {
        for(int i = 0; i < ragdollParts.Length; i++) {
            ragdollParts[i].velocity = Vector3.zero;
            ragdollParts[i].detectCollisions = activate;
            ragdollParts[i].useGravity = activate;
        }

        GetComponent<Rigidbody>().detectCollisions = !activate;
    }

    void Update() {
        if(!isDead) {
            if(Vector3.Distance(transform.position, origTarget) > maxStrayDistance || !hasTarget) {
                agent.SetDestination(origTarget);
                hasTarget = false;
                enemyTarget = null;
                currOverrideTime = overrideTime;
            }

            if(Vector3.Distance(transform.position, origTarget) <= maxStrayDistance) {
                currOverrideTime = 0;
            }

            if(hasTarget) {
                if(Vector3.Distance(transform.position, enemyTarget.transform.position) < attackRange) {
                    isAttacking = true;
                    agent.isStopped = true;

                    //Rotate unit towards enemy
                    Vector3 targetDir = enemyTarget.transform.position - transform.position;
                    float step = agent.speed * Time.deltaTime;
                    Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
                    transform.rotation = Quaternion.LookRotation(newDir);
                } else {
                    isAttacking = false;
                    agent.SetDestination(enemyTarget.transform.position);
                    agent.isStopped = false;
                }

                if(enemyTarget.isDead) {
                    agent.SetDestination(origTarget);
                    hasTarget = false;
                    enemyTarget = null;
                    isAttacking = false;
                    agent.isStopped = false;
                }
            }

            if(isAttacking && currAttackInterval <= 0) {
                StartCoroutine(Attack());
            }

            if(isAttacking && currAttackInterval > 0) {
                currAttackInterval -= Time.deltaTime;

                if(currAttackInterval <= 0) {
                    currAttackInterval = 0;
                }
            }

            if(currOverrideTime > 0) {
                currOverrideTime -= Time.deltaTime;
                if(currOverrideTime < 0)
                    currOverrideTime = 0;
            }
        }
    }

    void OnTriggerStay(Collider collider) {
        //Attack enemy unit
        //Check whether collided unit is remote if(gameObject is remote)
        if(!isDead && currOverrideTime <= 0 && collider.tag == "Unit") {
            Unit collidingUnit = collider.GetComponent<Unit>();

            //Debug.Log("CollidingUnit: " + collidingUnit.teamID + " this unit: " + teamID);
            if(collidingUnit.teamID != teamID) {
                if(!hasTarget) {
                    hasTarget = true;
                    agent.destination = collider.transform.position;
                    enemyTarget = collidingUnit;
                } else {
                    float newTargetDistance = 
                        Vector3.Distance(transform.position, collidingUnit.transform.position);
                    float oldTargetDistance = 
                        Vector3.Distance(transform.position, enemyTarget.transform.position);

                    if(newTargetDistance < oldTargetDistance) {
                        agent.destination = collider.transform.position;
                        enemyTarget = collidingUnit;
                    }
                }
            }
        }
    }

    public bool GetDamaged (int amt, UnitType fromUnitType) {
        if(!isDead) {
            //Process weaknesses
            for(int i = 0; i < damageModifiers.Length; i++) {
                if(damageModifiers[i].unitType == fromUnitType) {
                    amt = (int) ((float)amt*damageModifiers[i].multiplier);
                }
            }

            currentHealth -= amt;
            
            if(currentHealth <= 0) {
                currentHealth = 0;
                Die();
            }

            GameObject hitEffect = Instantiate(Resources.Load("Effects/HitEffect", typeof(GameObject))) as GameObject;
            hitEffect.transform.position = 
                new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        }

        return isDead;
    }

    public void Die() {
        gameObject.tag = "DeadUnit";
        isDead = true;
        if(squad != null)
            squad.RemoveUnit(this);

        for(int i = 0; i < disableOnDeath.Length; i++) {
            disableOnDeath[i].SetActive(false);

            GameObject deathEffect = Instantiate(Resources.Load("Effects/SmokePuffEffect", typeof(GameObject))) as GameObject;
            deathEffect.transform.position = 
                    new Vector3(
                        disableOnDeath[i].transform.position.x, 
                        disableOnDeath[i].transform.position.y + 0.5f, 
                        disableOnDeath[i].transform.position.z
                        );
        }
        StartCoroutine(DeathAnim());
    }

    public IEnumerator DeathAnim() {
        agent.enabled = false;
        animator.enabled = false;
        ActivateRagdoll(true);
    
        yield return new WaitForSeconds(5);

        GameObject deathEffect = Instantiate(Resources.Load("Effects/SmokePuffEffect", typeof(GameObject))) as GameObject;
        deathEffect.transform.position = 
                new Vector3(transform.position.x, transform.position.y + 0.15f, transform.position.z);
        gameObject.SetActive(false);
    }

    public IEnumerator Attack() {
        currAttackInterval = attackInterval;
        if(animator != null)
            animator.SetTrigger("Attack");
        
        yield return new WaitForSeconds(attackDelay);

        if(!isDead) {
            if(enemyTarget != null) {
                hasTarget = !enemyTarget.GetDamaged(damage, unitType);
            } else {
                hasTarget = false;
            }

            if(!hasTarget && agent.enabled)  {
                isAttacking = false;
                agent.SetDestination(origTarget);
                agent.isStopped = false;
                hasTarget = false;
                enemyTarget = null;
            }
        }
    }

    public void MoveTo(Vector3 target) {
        agent.SetDestination(target);
        origTarget = target;
        hasTarget = false;
        enemyTarget = null;
        agent.isStopped = false;
        currOverrideTime = overrideTime;
    }
}

[System.Serializable]
public class DamageModifier {
    public Unit.UnitType unitType;
    public float multiplier;
}