using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class BaseEntity : MonoBehaviour
{
    [FoldoutGroup("Entity")] public Stats baseStats;
    [FoldoutGroup("Entity")] [ReadOnly] public EntityStats entityStats;
    [FoldoutGroup("Entity")] [Button] public void ResetStats()
    {
        entityStats = new EntityStats(baseStats);
    }
    [FoldoutGroup("Entity")] [Button] public void RecalculateStats()
    {
        Debug.LogWarning("Recalculate stats not implemented yet!");
    }
    public void Start()
    {
        ResetStats();
    }
}
