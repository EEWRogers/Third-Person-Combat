using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    [SerializeField] float chaseRange = 10f;
    [SerializeField] float turnSpeed = 50f;
    [SerializeField] float maxAttackAngle = 25f;

    PlayerHealth player;
    bool isProvoked = false;
    bool isParriable = false;
    [SerializeField] ParticleSystem parryParticles;

    NavMeshAgent navMeshAgent;
    PlayerHealth playerHealth;
    EnemyWeapon enemyWeapon;
    Animator animator;

    public EnemyState currentState;

    void Awake() 
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyWeapon = GetComponentInChildren<EnemyWeapon>();
        player = FindObjectOfType<PlayerHealth>();

        SetState(EnemyState.Idle);
    }

    void Update() 
    {
        TransitionToChaseState();
        TransitionToAttackState();

        if (isProvoked)
        {
            FacePlayer();
        }

        HandleState();
    }

    void SetState(EnemyState newState)
    {
        currentState = newState;
    }

    void HandleState()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
            //implement idle behaviour
            break;

            case EnemyState.Chase:
            StopAttackingPlayer();
            ChasePlayer();
            break;

            case EnemyState.Ready:
            break;

            case EnemyState.Attack:
            BeParried();
            AttackPlayer();
            break;

            case EnemyState.Dodge:
            //implement dodge behaviour
            break;

            case EnemyState.Block:
            //implement block behaviour
            break;
        }
    }

    void TransitionToChaseState()
    {
        if (DistanceToPlayer() <= chaseRange && currentState != EnemyState.Chase)
        {
            SetState(EnemyState.Chase);
        }
    }

    void TransitionToAttackState()
    {
        if (DistanceToPlayer() <= navMeshAgent.stoppingDistance)
        {
            SetState(EnemyState.Attack);
        }
    }

    void ChasePlayer()
    {
        isProvoked = true;
        navMeshAgent.SetDestination(player.transform.position);
    }

    void StayAtSafeDistance()
    {
        if (DistanceToPlayer() < navMeshAgent.stoppingDistance)
        {
            
        }
    }

    void FacePlayer()
    {
        Vector3 direction = (player.transform.position - transform.position);
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation((direction.normalized));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
    }

    void AttackPlayer()
    {
        float angle = Vector3.Angle(transform.forward, DirectionOfPlayer());

        if (angle < maxAttackAngle)
        {
            animator.SetTrigger("attack");
        }

        else
        {
            StopAttackingPlayer();
        }
    }

    void StopAttackingPlayer()
    {
        animator.SetTrigger("stopAttack");
    }

    void BeParried()
    {
        if (isParriable && player.IsBlocking)
        {
            animator.SetTrigger("parried");
            Debug.Log("Attack parried!");
            parryParticles.Play();
            player.GainParryInvulnerability();

            DisableParryWindow();
        }
    }

    float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }

    Vector3 DirectionOfPlayer()
    {
        return player.transform.position - transform.position;
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
