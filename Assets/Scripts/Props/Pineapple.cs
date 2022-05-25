using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pineapple : Fruit
{
    public override void Pickup(Collider2D collision)
    {
        PlayerController pc = collision.GetComponent<PlayerController>();
        pc.GetComponent<HealthComponent>().TakeDamage(new DamageInfo(5, null));
        pc.GainExp();
        pc.Dash();
    }
}
