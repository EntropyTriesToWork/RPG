using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class OutOfBounds : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<BaseEntity>().EntityOutOfBounds();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<BaseEntity>().EntityOutOfBounds();
    }
}
