using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerController : BaseEntity
{
    private Animator _animator;
    private PlayerInput _playerInput;
    private Camera _camera;
    private Rigidbody _rb;

    public float MoveSpeed { get => entityStats.MoveSpeed.Value; }
    public float rotationSpeed;
    public float jumpForce;
    public LayerMask groundLayers;

    public bool attacking;
    public Vector2 movementInput;
    public bool grounded;

    private const string RunningState = "Running";
    private const string MovespeedVariable = "MoveSpeed";
    private const string AttackState = "Attack";

    private void OnEnable()
    {
        _playerInput = new PlayerInput();
        _playerInput.Enable();
        _playerInput.PlayerMovement.Movement.performed += playerInput => movementInput = playerInput.ReadValue<Vector2>();
        _playerInput.PlayerMovement.Movement.canceled += playerInput => movementInput = Vector2.zero;
        _playerInput.PlayerMovement.Jump.performed += _ => TryToDodgeRoll();

        _playerInput.Combat.PrimaryAttack.performed += _ => attacking = true;
        _playerInput.Combat.PrimaryAttack.canceled += _ => attacking = false;

        _animator = GetComponent<Animator>();
        _camera = Camera.main;
        _rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        CheckForGround();
        if (attacking)
        {
            Attack();
            return;
        }
        
        if(movementInput.magnitude <= 0.1f)
        {
            _animator.SetBool(RunningState, false);
        }
        else
        {
            TryToMove();
            TryToRotate();
            _animator.SetBool(RunningState, true);
        }
    }

    #region Abilities
    public void Attack()
    {
        if (IsCurrentState("AttacK")) { return; }
        _animator.Play(AttackState);
    }
    #endregion

    #region Movement 
    public void TryToMove()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) { return; }

        _animator.SetFloat(MovespeedVariable, MoveSpeed);
        Vector3 moveVector = _camera.transform.forward * movementInput.y;
        moveVector += _camera.transform.right * movementInput.x;
        moveVector.Normalize();
        moveVector = Vector3.ProjectOnPlane(moveVector, Vector3.up);
        _rb.MovePosition(transform.position + moveVector * Time.fixedDeltaTime * MoveSpeed);
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
    public void TryToDodgeRoll()
    {
        if(IsCurrentState("DodgeRoll") || !grounded) { return; }
        _animator.Play("DodgeRoll");
        _rb.AddForce(Vector3.forward * jumpForce + (Vector3)(movementInput * MoveSpeed), ForceMode.Impulse);
        //_rb.AddForce(_camera.transform.forward * movementInput.y * MoveSpeed, ForceMode.Impulse);
    }
    public void StartDodgeRoll()
    {

    }
    public void EndDodgeRoll()
    {

    }
    #endregion

    #region Logic
    public void CheckForGround()
    {
        if(Physics.OverlapBox(transform.position, new Vector3(0.75f, 0.1f, 0.75f), transform.rotation, groundLayers).Length > 0)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
            _rb.AddForce(Physics.gravity);
        }
        _animator.SetBool("Grounded", grounded);
    }
    private RaycastHit RayCastUnderMouse(LayerMask layer)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData, Mathf.Infinity, layer);
        return hitData;
    }
    public bool IsCurrentState(string stateName)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(stateName)) { return true; }
        return false;
    }
    #endregion
}