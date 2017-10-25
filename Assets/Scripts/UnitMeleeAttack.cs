using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMeleeAttack : MonoBehaviour {

    public int damage;
    public float attackDelay;

    private bool isAttacking;

    private WaitForSeconds attackWait;
    private GameObject target;
    private Animator animController;
    private NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        attackWait = new WaitForSeconds(attackDelay);
        animController = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        isAttacking = false;
	}

    public void attack(GameObject attackTarget)
    {
        if (!isAttacking)
        {
            target = attackTarget;
            isAttacking = true;
            StartCoroutine(performAttack());
        }
    }

    IEnumerator performAttack()
    {
        while (target != null  && agent.remainingDistance < agent.stoppingDistance)
        {
            animController.SetTrigger("attack");
            target.GetComponent<Health>().getDamage(damage);
            yield return attackWait;
        }

        isAttacking = false;
    }
}
