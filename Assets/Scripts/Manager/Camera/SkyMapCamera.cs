using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyMapCamera : MonoBehaviour
{
    private Vector3 lastMousePosition;
    private bool isRotating = false;
    private Vector2 velocity;
    private void Update()
    {
        ChangeRotation();
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
            velocity.x = Mathf.Lerp(velocity.x, deltaMouse.x, 0.5f);
            velocity.y = Mathf.Lerp(velocity.y, deltaMouse.y, 0.5f);

            transform.Rotate(Vector3.down * velocity.x * 0.05f, Space.World);
            transform.Rotate(Vector3.right * velocity.y * 0.05f, Space.Self);

            lastMousePosition = Input.mousePosition;
        }
    }
}
