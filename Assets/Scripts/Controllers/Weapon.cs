using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Weapon : MonoBehaviour
{
    public bool active = false;
    public WeaponController weaponController;

    [Button] public void UpdateCollider()
    {

    }

    public void StartCasting()
    {
        active = true;
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

        if(damageable != null)
        {
            weaponController.WeaponDealDamage(damageable);
        }
    }
}
