using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Fruit : MonoBehaviour
{
    public GameObject onPickupEffect;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Pickup(collision);

            GameObject effect = Instantiate(onPickupEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.75f);

            Destroy(gameObject);
        }
    }
    public abstract void Pickup(Collider2D collision);
}
