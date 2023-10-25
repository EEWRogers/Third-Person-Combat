using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRange = 10f;
    [SerializeField] float turnSpeed = 5f;

    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;

    NavMeshAgent navMeshAgent;
    Animator animator;

    void Start() 
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (isProvoked)
        {
            EngageTarget();
        }

        if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }
    }

    void EngageTarget()
    {
        FacePlayer();

        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            ChasePlayer();
        }
        else
        {
            AttackPlayer();
        }
    }

    void ChasePlayer()
    {
        navMeshAgent.SetDestination(target.position);
    }

    void AttackPlayer()
    {
        animator.SetTrigger("attack");
    }

    void FacePlayer()
    {
        Vector3 direction = (target.position - transform.position);
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation((direction.normalized));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
    }
}
