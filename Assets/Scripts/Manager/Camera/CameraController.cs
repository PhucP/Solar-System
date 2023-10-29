using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Movement;
using DG.Tweening;
using UnityEngine;

public enum CameraMode
{
    ChangeRotation,
    FollowPlanet,
    ExitFollowPlanet
}

public class CameraController : MonoBehaviour
{
    public float rotationSpeed;
    public float smoothTime;
    [SerializeField] private float stepMove;
    [SerializeField] private Transform sun;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private CameraMode cameraMode;
    private Vector2 velocity;
    private Vector3 lastMousePosition;
    private bool isRotating = false;

    private Camera mainCamera;
    private Vector2 initialTouchPos;
    private CelestialManager celestialManager => CelestialManager.Instance;

    private void Start()
    {
        cameraMode = CameraMode.ChangeRotation;
        mainCamera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        switch (cameraMode)
        {
            case CameraMode.ChangeRotation:
                ChangeRotation();
                break;
            case CameraMode.FollowPlanet:
                MoveToPlanet();
                break;
            default:
                ExitVisited();
                break;
        }
        
        Zoom();
    }

    private void MoveToPlanet()
    {
        var currentCelestialObject = celestialManager.CurrentCelestialObject;
        var direction = (sun.position - currentCelestialObject.transform.position).normalized;
        var newPosition = currentCelestialObject.transform.position + direction * 10f;

        var difference = newPosition - mainCamera.transform.position;
        var distance = difference.magnitude;
        var directionToMove = difference.normalized;
        
        mainCamera.transform.LookAt(currentCelestialObject.transform);
        if (distance < 0.1f)
        {
            mainCamera.transform.position += difference;
        }
        else mainCamera.transform.position += directionToMove * cameraSpeed * Time.deltaTime;
    }

    public void OnClickVisited()
    {
        mainCamera.transform.DOLookAt(celestialManager.CurrentCelestialObject.transform.position, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                cameraMode = CameraMode.FollowPlanet;
            });
        
        celestialManager.HideCelestialInformation();
    }

    public void ExitVisited()
    {
        var originalPosition = new Vector3(0 , 0, -100f);
        mainCamera.transform.DOLookAt(sun.transform.position, 2.5f)
            .SetEase(Ease.Linear);
        mainCamera.transform.DOLocalMove(originalPosition, 2.5f)
            .SetEase(Ease.Linear);
    }

    private void Zoom()
    {
#if !UNITY_EDITOR
    if (Input.touchCount == 2)
    {
        Touch firstTouch = Input.GetTouch(0);
        Touch secondTouch = Input.GetTouch(1);

        var firstTouchPrePos = firstTouch.position - firstTouch.deltaPosition;
        var secondTouchPrePos = secondTouch.position - secondTouch.deltaPosition;

        var touchesPrePosDifference = (firstTouchPrePos - secondTouchPrePos).magnitude;
        var touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

        var zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomSpeed;
        var direction = (sun.position - mainCamera.gameObject.transform.position).normalized;

        if (touchesPrePosDifference > touchesCurPosDifference)
        {
            mainCamera.transform.position += zoomModifier * direction;
        }

        if (touchesPrePosDifference < touchesCurPosDifference)
        {
            mainCamera.transform.position -= zoomModifier * direction;
        }
        
            // Touch touch1 = Input.GetTouch(0);
            // Touch touch2 = Input.GetTouch(1);
            //
            // float initialDistance = Vector2.Distance(touch1.position, touch2.position);
            // float currentDistance = Vector2.Distance(touch1.position, touch2.position);
            //
            // float zoomFactor = initialDistance / currentDistance;
            //
            // float newFOV = initialFOV / zoomFactor;
            // newFOV = Mathf.Clamp(newFOV, minFOV, maxFOV);
            // mainCamera.fieldOfView = newFOV;
            
            return;
        }
#endif
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

        if (scrollDelta != 0)
        {
            // float newFOV = mainCamera.fieldOfView - scrollDelta * zoomSpeed;
            // newFOV = Mathf.Clamp(newFOV, minFOV, maxFOV);
            // mainCamera.fieldOfView = newFOV;

            var direction = (sun.position - mainCamera.transform.position).normalized;
            var newPos =  mainCamera.transform.position + direction * scrollDelta * stepMove;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, newPos, smoothTime);
        }
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

    private void ChangeViewInOrbitMode()
    {
        
    }
}
