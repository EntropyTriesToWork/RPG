using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyController : BaseEntity
{
    NavMeshAgent _navAgent;
    private Animator _anim;

    public float attackCooldown = 5;
    private float _attackCD;

    public GameObject target;
    public override void Awake()
    {
        base.Awake();
        _navAgent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _hc.OnDeath += (_) => Destroy(gameObject);

        _navAgent.speed = entityStats.MoveSpeed.Value;

        target = FindObjectOfType<PlayerController>().gameObject;
    }
    void FixedUpdate()
    {
        if (_hc.IsDead) { return; }
        _navAgent.SetDestination(target.transform.position);
        _attackCD -= Time.fixedDeltaTime;
        if (IsTargetWithinRange)
        {
            _anim.SetBool("Chasing", false);
            if (_attackCD <= 0f)
            {
                _anim.SetTrigger("Attack");
                _attackCD = attackCooldown;
            }
        }
        else
        {
            _anim.SetBool("Chasing", true);
        }
    }

    private bool IsTargetWithinRange => Vector3.Distance(target.transform.position, transform.position) < 2f;

    public void Attack()
    {
        if (IsTargetWithinRange)
            target.GetComponent<IDamageable>().TakeDamage(new DamageInfo(10, 0, GetStats()));
    }
}
