using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pineapple : Fruit
{
    public override void Pickup(Collider2D collision)
    {
        PlayerController pc = collision.GetComponent<PlayerController>();
        //pc.GetComponent<HealthComponent>().TakeDamage(new DamageInfo(5, null));
        pc.PickupFruit();
        pc.dashCooldown = 0;
        pc.TryToDash(new UnityEngine.InputSystem.InputAction.CallbackContext());
    }
}
