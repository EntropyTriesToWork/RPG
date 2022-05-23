using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy_Slime : BaseEntity
{
    public LayerMask playerLayer;
    public float attackCooldown = 5;
    public ForceMode2D movementType;

    public GameObject dropOnDeath;

    public GameObject target;
    public override void Awake()
    {
        base.Awake();
        target = FindObjectOfType<PlayerController>().gameObject;
    }
    void FixedUpdate()
    {
        if (_hc.IsDead) { return; }
        if (GameManager.Instance.gameState != GameManager.GameState.Normal) { return; }

        Attack();

        if (transform.position.y < -10f) { _hc.TakeDamage(new DamageInfo(99999, null)); }
    }

    private bool TargetWithingAttackRange => Vector3.Distance(target.transform.position, transform.position) < 2f;

    public override void Move()
    {
        if (GameManager.Instance.gameState != GameManager.GameState.Normal) { return; }
        Vector2 dir = target.transform.position.x > transform.position.x ? new Vector2(-1, 1) : new Vector2(1, 1);
        _rb.AddForce(Vector2.Scale(dir, -Vector2.right) * MS, movementType);
        transform.localScale = dir;
    }

    public override void Attack()
    {
        if (IsTouchingPlayer())
        {
            target.GetComponent<IDamageable>().TakeDamage(new DamageInfo(ATK, gameObject));
        }
    }
    public override void Death(DamageReport report)
    {
        StartCoroutine(DelayedAction(0.25f, () => Instantiate(dropOnDeath, transform.position, Quaternion.identity)));
        Destroy(gameObject, 0.5f);
    }
    public bool IsTouchingPlayer()
    {
        if (Physics2D.OverlapBox(transform.position + (Vector3)_boxCollider.offset, _boxCollider.size, 0, playerLayer) != null) { return true; }
        return false;
    }
}
