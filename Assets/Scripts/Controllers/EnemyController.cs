using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    NavMeshAgent _navAgent;

    public GameObject target;
    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        _navAgent.SetDestination(target.transform.position);
    }
}
