using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Weapon weapon;

    public void WeaponSwingStart()
    {
        weapon.StartCasting();
    }
    public void WeaponSwingEnd()
    {
        weapon.EndCasting();
    }
    public void WeaponDealDamage(IDamageable damageable)
    {
        damageable.TakeDamage(new DamageInfo(10 + Random.Range(-3, 3), 0, GetComponent<BaseEntity>().GetStats()));
    }
}
