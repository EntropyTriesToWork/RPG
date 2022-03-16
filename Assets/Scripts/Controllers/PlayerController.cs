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
    public float jumpForce;
    public LayerMask groundLayers;

    public CharacterStat moveSpeed;
    public LayerMask interactablesLayer;

    public Vector2 movementInput;
    public bool grounded;

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
    void FixedUpdate()
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
        CheckForGround();
    }
    #region Movement 
    public void TryToMove()
    {
        _animator.SetFloat(msVar, MoveSpeed);
        Vector3 moveVector = _camera.transform.forward * movementInput.y;
        moveVector += _camera.transform.right * movementInput.x;
        moveVector.Normalize();
        moveVector = Vector3.ProjectOnPlane(moveVector, Vector3.up);
        _rb.MovePosition(transform.position + moveVector * Time.fixedDeltaTime * MoveSpeed * 5);
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
        if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") || !grounded) { return; }
        _animator.Play("Jump");
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        _rb.AddForce(_camera.transform.forward * movementInput.y * MoveSpeed, ForceMode.Impulse);
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

    public void TryToInteract()
    {

    }
    #endregion
}