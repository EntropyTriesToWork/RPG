using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(HealthComponent))]

public abstract class BaseEntity : MonoBehaviour
{
    [FoldoutGroup("Entity")] public Stats baseStats;
    [FoldoutGroup("Entity")] public HUD entityHUD;
    [FoldoutGroup("Entity")] [ReadOnly] public EntityStats entityStats;
    [FoldoutGroup("Entity")] [Button] public void ResetStats()
    {
        entityStats = new EntityStats(baseStats);
    }
    [FoldoutGroup("Entity")] [Button] public void RecalculateStats()
    {
        Debug.LogWarning("Recalculate stats not implemented yet!");
    }
    protected HealthComponent _hc;

    public virtual void Awake()
    {
        ResetStats();
        _hc = GetComponent<HealthComponent>();
        _hc.OnHealthChanged += () => entityHUD.UpdateHealth(_hc.Health, MAXHP);
    }
    #region Stats
    public Stats GetStats()
    {
        return new Stats()
        {
            HP = MAXHP,
            PPOW = PPOW,
            MPOW = MPOW,
            CRIT = CRIT,
            CRITDMG = CRITDMG,
            AS = AS,
            DEF = DEF,
            RES = RES,
            DODG = DODG
        };
    }
    public int MAXHP => Mathf.RoundToInt(entityStats.MaxHealth.Value);
    public float PPOW => entityStats.PhysicalPower.Value + 1;
    public float MPOW => entityStats.MagicPower.Value + 1;
    public float CRIT => entityStats.CritChance.Value;
    public float CRITDMG => entityStats.CritDamage.Value + 2;
    public float AS => entityStats.AttackSpeed.Value;
    public float DEF => entityStats.Defense.Value;
    public float RES => entityStats.Resistance.Value;
    public float DODG => entityStats.DodgeChance.Value;
    #endregion
}
