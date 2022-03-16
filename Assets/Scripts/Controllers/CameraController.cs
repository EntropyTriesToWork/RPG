using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject followObject;
    public float panSpeed = 3f;
    public float rotationSpeed = 2f;
    public float zoomSpeed = 4;
    public Vector2 zoomLimits;
    private float rotationVelocity;
    private bool _cameraRotating;
    private Camera _cam;
    private Vector2 _currentMousePos;
    private Vector2 _lastMousePos;

    private PlayerInput playerInput;
    private void Awake()
    {
        _cameraRotating = false;
        playerInput = new PlayerInput();
        playerInput.Enable();
        playerInput.Camera.Movement.performed += Movement_performed;
        playerInput.Camera.Movement.canceled += Movement_canceled;
        playerInput.Cursor.Position.performed += Position_performed;
        playerInput.Cursor.Position.canceled += Position_canceled;
        _cam = Camera.main;
    }

    private void Position_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _currentMousePos = Vector2.zero;
    }

    private void Position_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _currentMousePos = obj.ReadValue<Vector2>();
    }

    private void Movement_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _cameraRotating = false;
    }

    private void Movement_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _cameraRotating = true;
    }

    void Update()
    {
        if (_cameraRotating)
        {
            rotationVelocity += (_currentMousePos - _lastMousePos).normalized.x * rotationSpeed * Time.deltaTime;
            _lastMousePos = _currentMousePos;
        }
        CalculateRotation();
        CalculateZoom();
    }
    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, followObject.transform.position, 0.01f);
    }

    void CalculateRotation()
    {
        Vector3 rot = _cam.transform.parent.eulerAngles;
        rot.y += rotationVelocity;
        _cam.transform.parent.eulerAngles = rot;
        rotationVelocity = Mathf.Lerp(rotationVelocity, 0f, Time.deltaTime * 30f);
    }

    void CalculateZoom()
    {
        //Camera.main.fieldOfView = Mathf.Clamp(Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed + Camera.main.fieldOfView, zoomLimits.x, zoomLimits.y);
    }
}
