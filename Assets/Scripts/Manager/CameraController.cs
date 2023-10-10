using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum Mode
    {
        ChangeRotation,
        ChangePosition
    }
    public Mode mode;
    public float rotationSpeed;
    public float smoothTime;
    private Vector2 velocity;
    private Vector3 lastMousePosition;
    private bool isRotating = false;

    public float zoomSpeed;
    public float minFOV;
    public float maxFOV;

    private Camera mainCamera;
    private Vector2 initialTouchPos;
    private float initialFOV;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
        initialFOV = mainCamera.fieldOfView;
    }

    private void Update()
    {
        if (mode == Mode.ChangeRotation)
        {
            ChangeRotation();
        }
        else if (mode == Mode.ChangePosition)
        {
            ChangePosition();
        }

        Zoom();
    }

    private void Zoom()
    {
#if UNITY_EDITOR
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

        if (scrollDelta != 0)
        {
            float newFOV = mainCamera.fieldOfView - scrollDelta * zoomSpeed;
            newFOV = Mathf.Clamp(newFOV, minFOV, maxFOV);
            mainCamera.fieldOfView = newFOV;
        }
#else
    if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            float initialDistance = Vector2.Distance(touch1.position, touch2.position);
            float currentDistance = Vector2.Distance(touch1.position, touch2.position);

            float zoomFactor = initialDistance / currentDistance;

            float newFOV = initialFOV / zoomFactor;
            newFOV = Mathf.Clamp(newFOV, minFOV, maxFOV);
            mainCamera.fieldOfView = newFOV;
        }
#endif
    }

    private void ChangeRotation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
            isRotating = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isRotating = false;
        }

        if (isRotating)
        {
            Vector3 deltaMouse = Input.mousePosition - lastMousePosition;

            // Áp dụng smoothing
            velocity.x = Mathf.Lerp(velocity.x, deltaMouse.x, smoothTime);
            velocity.y = Mathf.Lerp(velocity.y, deltaMouse.y, smoothTime);

            transform.Rotate(Vector3.up * velocity.x * rotationSpeed, Space.World);
            transform.Rotate(Vector3.left * velocity.y * rotationSpeed, Space.Self);

            lastMousePosition = Input.mousePosition;
        }
    }

    private void ChangePosition()
    {
        //do something
    }
}
