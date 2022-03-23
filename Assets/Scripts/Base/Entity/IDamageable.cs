using UnityEngine;

public interface IDamageable
{
    public DamageReport TakeDamage(DamageInfo damageInfo);
    public DamageReport TakeDamageWithForce(DamageInfo damageInfo, Vector3 force);
}
[System.Serializable]
public struct DamageInfo
{
    public int physicalAttack;
    public int magicAttack;
    public Stats stats;
    public bool dodgeable;
    public bool lethal;

    public DamageInfo(int pATK, int mATK, Stats stats, bool dodgeable = true, bool lethal = true)
    {
        physicalAttack = pATK;
        magicAttack = mATK;
        this.stats = stats;
        this.dodgeable = dodgeable;
        this.lethal = lethal;
    }
}
[System.Serializable]
public struct DamageReport
{
    public DamageReportState damageState;
}
public enum DamageReportState
{
    Normal,
    Dodged,
    Blocked,
    Canceled,
}