using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]

public abstract class BaseEntity : MonoBehaviour
{
    [FoldoutGroup("Entity")] public Stats baseStats;
    [FoldoutGroup("Entity")] public float damageImmunityTime;
    [FoldoutGroup("Entity")] public HUD entityHUD;
    [FoldoutGroup("Entity")] [ReadOnly] public EntityStats entityStats;
    [FoldoutGroup("Entity")]
    [Button]
    public void ResetStats()
    {
        entityStats = new EntityStats(baseStats);
    }
    [FoldoutGroup("Entity")]
    [Button]
    public void RecalculateStats()
    {
        Debug.LogWarning("Recalculate stats not implemented yet!");
    }

    protected BoxCollider2D _boxCollider;
    protected Rigidbody2D _rb;
    protected Animator _animator;
    protected HealthComponent _hc;
    protected SpriteRenderer _sr;

    [FoldoutGroup("Entity")] [ReadOnly] public bool stunned;

    #region States
    public virtual void Awake()
    {
        ResetStats();
        _hc = GetComponent<HealthComponent>();
        _sr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();

        if (entityHUD != null) { _hc.OnHealthChanged += () => entityHUD.UpdateHealth(_hc.Health, MAXHP); }
        _hc.OnTakeDamage += TakeDamage;
        _hc.OnDeath += Death;
    }
    public virtual void TakeDamage(DamageReport report)
    {
        _hc.canTakeDamage = false;
        stunned = true;
        StartCoroutine(DelayedAction(damageImmunityTime, () => _hc.canTakeDamage = true));
        StartCoroutine(DelayedAction(0.5f, () => stunned = false));

        _animator.Play(HitState);

        if (report.attacker != null)
        {
            Vector2 dir = transform.position - report.attacker.transform.position;
            _rb.AddForce((dir.normalized + Vector2.up) * 10f, ForceMode2D.Impulse);
        }
    }
    public abstract void Attack();
    public abstract void Move();
    public abstract void Death(DamageReport report);

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Traps"))
        {
            if (_hc.canTakeDamage)
            {
                _hc.TakeDamage(new DamageInfo(10, null));
                _rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
            }
        }
    }
    #endregion

    #region Stats
    public Stats GetStats()
    {
        return new Stats()
        {
            HP = MAXHP,
            ATK = ATK,
            CRIT = CRIT,
            CRITDMG = CRITDMG,
            AS = AS,
            DEF = DEF,
            DODG = DODG
        };
    }
    public int MAXHP => Mathf.RoundToInt(entityStats.MaxHealth.Value);
    public int ATK => Mathf.RoundToInt(entityStats.Attack.Value);
    public float CRIT => entityStats.CritChance.Value;
    public float CRITDMG => entityStats.CritDamage.Value + 2;
    public float MS => Mathf.Max(0.1f, entityStats.MoveSpeed.Value);
    public float AS => entityStats.AttackSpeed.Value;
    public float DEF => entityStats.Defense.Value;
    public float DODG => entityStats.DodgeChance.Value;
    #endregion

    public IEnumerator DelayedAction(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }

    protected const string RunningState = "Running";
    protected const string AttackState = "Attack";
    protected const string JumpState = "Jump";
    protected const string FallingState = "Falling";
    protected const string HitState = "Hit";
}
