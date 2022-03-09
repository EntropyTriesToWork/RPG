using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private PlayerInput _playerInput;
    private Camera _camera;
    private Rigidbody _rb;

    public float MoveSpeed { get => moveSpeed.Value; }
    public float rotationSpeed;

    public CharacterStat moveSpeed;
    public LayerMask groundLayer, interactablesLayer;

    public Vector2 movementInput;

    private const string runVar = "Running";
    private const string msVar = "MoveSpeed";

    private void OnEnable()
    {
        _playerInput = new PlayerInput();
        _playerInput.Enable();
        _playerInput.PlayerMovement.Movement.performed += playerInput => movementInput = playerInput.ReadValue<Vector2>();
        _playerInput.PlayerMovement.Movement.canceled += playerInput => movementInput = Vector2.zero;
        _playerInput.PlayerMovement.Jump.performed += _ => TryToJump();

        _animator = GetComponent<Animator>();
        _camera = Camera.main;
        _rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if(movementInput.magnitude <= 0.1f)
        {
            _animator.SetBool(runVar, false);
        }
        else
        {
            TryToMove();
            TryToRotate();
            _animator.SetBool(runVar, true);
        }
    }
    #region Movement 
    public void TryToMove()
    {
        _animator.SetFloat(msVar, MoveSpeed);
        Vector3 moveVector = _camera.transform.forward * movementInput.y;
        moveVector += _camera.transform.right * movementInput.x;
        moveVector.Normalize();

        moveVector *= MoveSpeed * 4;
        moveVector = Vector3.ProjectOnPlane(moveVector, Vector3.up);
        _rb.velocity = moveVector;
    }
    public void TryToRotate()
    {
        Vector3 dirVector = _camera.transform.forward * movementInput.y;
        dirVector += _camera.transform.right * movementInput.x;
        dirVector.Normalize();
        dirVector.y = 0;

        Quaternion targetRot = Quaternion.LookRotation(dirVector);
        Quaternion rot = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
        transform.rotation = rot;
    }
    public void TryToJump()
    {
        moveSpeed.AddModifier(new StatModifier(1, StatModType.Flat));
    }
    #endregion

    #region Logic
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
    #endregion
}
