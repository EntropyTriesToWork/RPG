using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : BaseEntity
{
    NavMeshAgent _navAgent;
    public HUD enemyHUD;

    public GameObject target;
    public override void Awake()
    {
        base.Awake();
        _navAgent = GetComponent<NavMeshAgent>();
        _hc.OnDeath += (_) => Destroy(gameObject);

        target = FindObjectOfType<PlayerController>().gameObject;
    }
    void FixedUpdate()
    {
        _navAgent.SetDestination(target.transform.position);

        if(Vector3.Distance(target.transform.position, transform.position) < 1f)
        {
            target.GetComponent<IDamageable>().TakeDamage(new DamageInfo(1, 0, GetStats()));
        }
    }
}
