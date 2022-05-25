using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melon : Fruit
{
    public override void Pickup(Collider2D collision)
    {
        PlayerController pc = collision.GetComponent<PlayerController>();
        pc.GainExp();
        Rigidbody2D rb = pc.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, 30f), ForceMode2D.Impulse);
    }
}
