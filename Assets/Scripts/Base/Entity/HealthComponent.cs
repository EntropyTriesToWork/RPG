using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class HealthComponent : IDamageable
{
    #region Private Variables
    private int health;
    #endregion

    #region Events
    public Action<DamageReport> OnTakeDamage;
    #endregion
    [ReadOnly] public int Health { get => health; set => health = value; }

    public DamageReport TakeDamage(DamageInfo damageInfo)
    {
        DamageReport report = new DamageReport();

        //Take damage not implemented yet.

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
}