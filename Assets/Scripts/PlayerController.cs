using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public LayerMask groundLayer, interactablesLayer;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    public GameObject clickEffect;
    public ParticleSystem walkEffect;
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hitData = RayCastUnderMouse(groundLayer);

            if (hitData.collider != null)
            {
                Instantiate(clickEffect, hitData.point, Quaternion.Euler(hitData.normal));
                Debug.Log(hitData.point);
                _navMeshAgent.SetDestination(hitData.point);
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitData = RayCastUnderMouse(interactablesLayer);
            if(hitData.collider != null)
            {
                hitData.collider.GetComponent<IInteractable>().Interact();
            }
        }

        bool isRunning = _navMeshAgent.remainingDistance >= 0.5f;
        _animator.SetBool("Running", isRunning);
        if (isRunning && !walkEffect.isPlaying) { walkEffect.Play(); }
        else if(!isRunning) { walkEffect.Stop(); }
    }

    private RaycastHit RayCastUnderMouse(LayerMask layer)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData, Mathf.Infinity, layer);
        return hitData;
    }

    public void TryToInteract()
    {

    }
}
