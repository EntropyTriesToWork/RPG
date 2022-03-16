using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ClickInteractable : MonoBehaviour, IInteractable
{
    public UnityEvent OnClicked;
    public void Interact()
    {
        Debug.Log("Pressed");
        OnClicked.Invoke();
    }
    private void OnMouseDown()
    {
        Interact();
    }
}
