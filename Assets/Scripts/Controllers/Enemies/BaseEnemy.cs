using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseEnemy : BaseEntity
{
    public LayerMask playerLayer, visionLayers;
    public ForceMode2D movementType;
    public GameObject dropOnDeath;
    public GameObject target;
    public float visionRange = 5f;

    public override void Awake()
    {
        base.Awake();
        target = FindObjectOfType<PlayerController>().gameObject;
    }
    public virtual void FixedUpdate()
    {
        if (_hc.IsDead) { return; }
        if (GameManager.Instance.gameState != GameManager.GameState.Normal) { return; }

        Move();
        Attack();
        EntityOutOfBounds();
        TurnToTarget();
    }

    protected void TurnToTarget()
    {
        transform.localScale = target.transform.position.x > transform.position.x ? new Vector2(-1, 1) : new Vector2(1, 1);
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
        if (GameManager.Instance != null) { GameManager.Instance.enemiesKilled++; }
    }
    public virtual void StompedOn()
    {
        _hc.TakeDamage(new DamageInfo(10, target));
    }
    public override void EntityOutOfBounds()
    {
        if (transform.position.y < -20f) { Destroy(gameObject); }
    }
    #region Getters
    public Vector2 DirectionToTarget => (target.transform.position - transform.position).normalized;
    public bool PlayerVisible()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, DirectionToTarget, visionRange, visionLayers);
        if (hit.collider == null) { return false; }
        if (hit.collider.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }
    public bool IsTouchingPlayer()
    {
        if (Physics2D.OverlapBox(transform.position + (Vector3)_boxCollider.offset, _boxCollider.size, 0, playerLayer) != null) { return true; }
        return false;
    }
    #endregion
}
