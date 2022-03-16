using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class CollisionInteractable : MonoBehaviour, IInteractable
{
    public UnityEvent CollisionEnter, CollisionStay, CollisionExit;
    public void Interact()
    {
        Debug.Log("Pressed");
    }
    private void OnCollisionEnter(Collision collision)
    {
        CollisionEnter.Invoke();   
    }
    private void OnCollisionStay(Collision collision)
    {
        CollisionStay.Invoke();
    }
    private void OnCollisionExit(Collision collision)
    {
        CollisionExit.Invoke();
    }
}
