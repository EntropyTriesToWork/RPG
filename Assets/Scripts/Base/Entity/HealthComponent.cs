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
        if (_entity == null) { Debug.LogError("Missing Entity component but still calling TakeDamage!"); report.damageState = DamageReportState.Canceled; return report; }
        if(UnityEngine.Random.Range(0f, 100f) < _entity.DODG * 100f) { report.damageState = DamageReportState.Dodged; return report; }
        //Take damage not implemented yet.
        int p = Mathf.RoundToInt(damageInfo.physicalAttack / (1 + (_entity.DEF / 100f)) * damageInfo.stats.PPOW);
        int m = Mathf.RoundToInt(damageInfo.magicAttack / (1 + (_entity.RES / 100f)) * damageInfo.stats.MPOW);

        Health -= p + m;
        if (IsDead) { report.killed = true; OnDeath?.Invoke(report); }

        report.damage = p + m;

        if(p + m > 0) { OnTakeDamage?.Invoke(report); }
        return report;
    }

    public DamageReport TakeDamageWithForce(DamageInfo damageInfo, Vector3 force)
    {
        DamageReport report = TakeDamage(damageInfo);
        if(report.damageState == DamageReportState.Normal)
        {
            //Add force to body.
        }
        return report;
    }

    #region Getters
    public bool IsDead => Health <= 0;
    #endregion
}