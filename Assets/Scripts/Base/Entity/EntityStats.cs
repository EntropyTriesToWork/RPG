using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EntityStats
{
    public CharacterStat MaxHealth;
    public CharacterStat PhysicalPower; //Physical damage multiplier
    public CharacterStat MagicPower; //Magical damage multiplier;
    public CharacterStat CritChance;
    public CharacterStat CritDamage;
    public CharacterStat MoveSpeed;
    public CharacterStat AttackSpeed;

    public CharacterStat Defense; //Physical damage resistance
    public CharacterStat Resistance; //Magic damage resistance
    public CharacterStat DodgeChance; //Magic damage resistance

    public EntityStats(Stats stats)
    {
        MaxHealth = new CharacterStat { BaseValue = stats.HP };
        PhysicalPower= new CharacterStat { BaseValue = stats.PPOW };
        MagicPower = new CharacterStat { BaseValue = stats.MPOW };
        CritChance = new CharacterStat { BaseValue = stats.CRIT };
        CritDamage = new CharacterStat { BaseValue = stats.CRITDMG };
        MoveSpeed = new CharacterStat { BaseValue = stats.MS };
        AttackSpeed = new CharacterStat { BaseValue = stats.AS };
        Defense = new CharacterStat { BaseValue = stats.DEF };
        Resistance = new CharacterStat { BaseValue = stats.RES };
        DodgeChance = new CharacterStat { BaseValue = stats.DODG };
    }
}
[System.Serializable]
public struct Stats
{
    public int HP;
    public float PPOW;
    public float MPOW;
    public float CRIT;
    public float CRITDMG;
    public float MS;
    public float AS;

    public float DEF;
    public float RES;
    public float DODG;
}