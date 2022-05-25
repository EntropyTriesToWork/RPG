using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orange : Fruit
{
    public override void Pickup(Collider2D collision)
    {
        PlayerController pc = collision.GetComponent<PlayerController>();
        pc.GainExp();
        pc.jumps++;
        pc.jumps = Mathf.Clamp(pc.jumps, 0, pc.jumpCount);
    }
}
