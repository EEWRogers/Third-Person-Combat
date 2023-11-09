using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float chaseRange = 10f;
    [SerializeField] float turnSpeed = 50f;
    [SerializeField] float maxAttackAngle = 25f;

    PlayerHealth player;
    Vector3 directionOfPlayer;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    bool isParriable = false;

    NavMeshAgent navMeshAgent;
    PlayerHealth playerHealth;
    EnemyWeapon enemyWeapon;
    Animator animator;


    void Awake() 
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyWeapon = GetComponentInChildren<EnemyWeapon>();
        player = FindObjectOfType<PlayerHealth>();
    }

    void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, player.transform.position);
        directionOfPlayer = player.transform.position - transform.position;

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
        float angle = Vector3.Angle(transform.forward, directionOfPlayer);
        FacePlayer();

        if (distanceToTarget >= navMeshAgent.stoppingDistance)
        {
            ChasePlayer();
        }
        else
        {
            if (angle < maxAttackAngle)
            {
                AttackPlayer();
            }
        }
    }

    void ChasePlayer()
    {
        navMeshAgent.SetDestination(player.transform.position);
        animator.SetTrigger("stopAttack");
    }

    void AttackPlayer()
    {
        BeParried();
        animator.SetTrigger("attack");
    }

    void FacePlayer()
    {
        Vector3 direction = (player.transform.position - transform.position);
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation((direction.normalized));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
    }

    void BeParried()
    {
        if (isParriable && player.IsBlocking)
        {
            animator.SetTrigger("parried");
            Debug.Log("Attack parried!");
            player.GainParryInvulnerability();

            DisableParryWindow();
        }
    }

    void EnableWeapon()
    {
        enemyWeapon.weaponCollider.enabled = true;
    }

    void DisableWeapon()
    {
        enemyWeapon.weaponCollider.enabled = false;
    }

    void EnableParryWindow()
    {
        if (!player.IsBlocking)
        {
            isParriable = true;
        }
    }

    void DisableParryWindow()
    {
        isParriable = false;
    }
}
