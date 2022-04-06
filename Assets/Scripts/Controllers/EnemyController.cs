using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : BaseEntity
{
    NavMeshAgent _navAgent;

    public GameObject target;
    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
    }
    void FixedUpdate()
    {
        _navAgent.SetDestination(target.transform.position);
    }
}
