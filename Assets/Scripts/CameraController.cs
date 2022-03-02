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
    private bool isRotating;
    private float velocity;
    private float zoomVelocity;

    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            isRotating = true;
        }
        else
        {
            isRotating = false;
        }
        if (isRotating == true)
        {
            RotateCameraTarget();
        }
        CalculateRotation();
        CalculateZoom();

        transform.position = Vector3.Lerp(transform.position, followObject.transform.position, 0.5f);
    }

    void CalculateRotation()
    {
        Vector3 rot = Camera.main.transform.parent.eulerAngles;
        rot.y += velocity;
        Camera.main.transform.parent.eulerAngles = rot;
        velocity = Mathf.Lerp(velocity, 0f, Time.deltaTime * rotationSpeed);
    }

    void CalculateZoom()
    {
        Camera.main.fieldOfView = Mathf.Clamp(Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed + Camera.main.fieldOfView, zoomLimits.x, zoomLimits.y);
    }
    void RotateCameraTarget()
    {
        velocity += Input.GetAxis("Mouse X") * 3f;
    }
}
