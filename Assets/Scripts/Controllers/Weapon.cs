using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Weapon : MonoBehaviour
{
    public bool active = false;
    public WeaponController weaponController;
    private List<IDamageable> targets;

    [Button]
    public void UpdateCollider()
    {

    }

    public void StartCasting()
    {
        active = true;
        targets = new List<IDamageable>();
    }
    public void EndCasting()
    {
        active = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!active) { return; }
        if (!other.CompareTag("Enemy")) { return; }
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (targets.Contains(damageable)) { return; }
        if (damageable != null)
        {
            weaponController.WeaponDealDamage(damageable);
            targets.Add(damageable);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!active) { return; }
        if (!other.CompareTag("Enemy")) { return; }
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (targets.Contains(damageable)) { return; }
        if (damageable != null)
        {
            weaponController.WeaponDealDamage(damageable);
            targets.Add(damageable);
        }
    }
}
