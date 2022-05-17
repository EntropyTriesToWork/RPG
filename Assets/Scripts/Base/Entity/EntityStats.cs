using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EntityStats
{
    public CharacterStat MaxHealth;
    public CharacterStat Attack;
    public CharacterStat CritChance;
    public CharacterStat CritDamage;
    public CharacterStat MoveSpeed;
    public CharacterStat AttackSpeed;

    public CharacterStat Defense;
    public CharacterStat DodgeChance;

    public EntityStats(Stats stats)
    {
        MaxHealth = new CharacterStat { BaseValue = stats.HP };
        Attack = new CharacterStat { BaseValue = stats.ATK };
        CritChance = new CharacterStat { BaseValue = stats.CRIT };
        CritDamage = new CharacterStat { BaseValue = stats.CRITDMG };
        MoveSpeed = new CharacterStat { BaseValue = stats.MS };
        AttackSpeed = new CharacterStat { BaseValue = stats.AS };
        Defense = new CharacterStat { BaseValue = stats.DEF };
        DodgeChance = new CharacterStat { BaseValue = stats.DODG };
    }
}
[System.Serializable]
public struct Stats
{
    public int HP;
    public float ATK;
    public float CRIT;
    public float CRITDMG;
    public float MS;
    public float AS;

    public float DEF;
    public float DODG;
}