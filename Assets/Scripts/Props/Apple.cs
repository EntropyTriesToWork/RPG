using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : Fruit
{
    public override void Pickup(Collider2D collision)
    {
        PlayerController pc = collision.GetComponent<PlayerController>();
        pc.GainExp();

        collision.GetComponent<HealthComponent>().Heal(10);
    }
}
