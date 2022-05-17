using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public GameObject onPickupEffect;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Pickup();

            collision.GetComponent<HealthComponent>().Health += 10;
        }
    }
    public void Pickup()
    {
        GameObject effect = Instantiate(onPickupEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.75f);

        Destroy(gameObject);
    }
}
