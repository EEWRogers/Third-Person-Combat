using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent navMeshAgent;

    void Start() 
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update() 
    {
        navMeshAgent.SetDestination(target.position);
    }
}