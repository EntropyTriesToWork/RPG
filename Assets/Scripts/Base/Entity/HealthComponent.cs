using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[DisallowMultipleComponent]
public class HealthComponent : MonoBehaviour, IDamageable
{
    #region Private Variables
    [ReadOnly] [SerializeField] private int health;
    private BaseEntity _entity;
    [ReadOnly] public bool canTakeDamage = true;
    #endregion
    private void Start()
    {
        _entity = GetComponent<BaseEntity>();
        if (_entity != null)
        {
            Health = _entity.MAXHP;
        }
    }

    #region Events
    public Action OnHealthChanged;
    public Action<DamageReport> OnTakeDamage;
    public Action<DamageReport> OnDeath;
    #endregion

    [ReadOnly] public int Health { get => health; set { health = value; OnHealthChanged?.Invoke(); } }

    public DamageReport TakeDamage(DamageInfo damageInfo)
    {
        if (_entity == null) { _entity = GetComponent<BaseEntity>(); }

        DamageReport report = new DamageReport();
        report.attacker = damageInfo.attacker;
        if (!canTakeDamage || GameManager.Instance.gameState != GameManager.GameState.Normal) { report.damageState = DamageReportState.Canceled; return report; }
        if (_entity == null) { Debug.LogError("Missing Entity component but still calling TakeDamage!"); report.damageState = DamageReportState.Canceled; return report; }
        if (UnityEngine.Random.Range(0f, 100f) < _entity.DODG * 100f) { report.damageState = DamageReportState.Dodged; return report; }

        int damage = Mathf.Max(1, Mathf.RoundToInt(damageInfo.damage - _entity.DEF));
        Health -= damage;
        if (IsDead) { report.killed = true; OnDeath?.Invoke(report); }

        report.damage = damage;
        report.damageState = DamageReportState.Normal;
        if (damage > 0) { OnTakeDamage?.Invoke(report); }
        return report;
    }

    public DamageReport TakeDamageWithForce(DamageInfo damageInfo, Vector3 force)
    {
        DamageReport report = TakeDamage(damageInfo);
        if (report.damageState == DamageReportState.Normal)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.AddForce(force, ForceMode2D.Impulse);
            }
        }
        return report;
    }

    #region Getters
    public bool IsDead => Health <= 0;
    #endregion
}