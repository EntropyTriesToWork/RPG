using UnityEngine;

public interface IDamageable
{
    public DamageReport TakeDamage(DamageInfo damageInfo);
    public void Heal(int healAmount);
}
[System.Serializable]
public struct DamageInfo
{
    public int damage;
    public bool dodgeable;
    public bool lethal;
    public GameObject attacker;

    public DamageInfo(int damage, GameObject attacker, bool dodgeable = true, bool lethal = true)
    {
        this.damage = damage;
        this.dodgeable = dodgeable;
        this.lethal = lethal;
        this.attacker = attacker;
    }
}
[System.Serializable]
public struct DamageReport
{
    public DamageReportState damageState;
    public int damage;
    public bool killed;
    public GameObject attacker;
}
public enum DamageReportState
{
    Normal,
    Dodged,
    Blocked,
    Canceled,
}