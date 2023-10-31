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
    [SerializeField] private Vector3 originPosition;
    [SerializeField] private GroupButtonController informationOfCurrentPlanet;
    
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

    private void FixedUpdate()
    {
        switch (cameraMode)
        {
            case CameraMode.FollowPlanet:
                MoveToPlanet();
                break;
            case CameraMode.ExitFollowPlanet:
                ExitFollowPlanet();
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        switch (cameraMode)
        {
            case CameraMode.ChangeRotation:
                ChangeRotation();
                break;
            default:
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
            if (!informationOfCurrentPlanet.IsShowGroupButton)
            {
                ShowInformationForCurrentPlanetFollowing();
            }
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

    private void ShowInformationForCurrentPlanetFollowing()
    {
        //set information in the panel
        
        //show information panel
        informationOfCurrentPlanet.ShowHideGroupButton();
    }

    private void SetInformationForCurrentPlanet()
    {
        var currentPlanet = celestialManager.CurrentCelestialObject;
        //do something with this information
    }

    private void HideInformationCurrentPlanetFollowing()
    {
        //hide the information panel
        informationOfCurrentPlanet.ShowHideGroupButton();
    }

    public void ExitFollowPlanet()
    {
        //hide information of current planet
        if (informationOfCurrentPlanet.IsShowGroupButton)
        {
            HideInformationCurrentPlanetFollowing();
        }
        
        cameraMode = CameraMode.ExitFollowPlanet;
        //have a position to move back is original position
        var difference = originPosition - mainCamera.transform.position;
        var distance = difference.magnitude;
        var directionToMove = difference.normalized;
        
        //do look at the Sun while move back
        mainCamera.transform.LookAt(sun);
        
        //change state to change rotation when finish the moving
        if (distance < 0.1f)
        {
            cameraMode = CameraMode.ChangeRotation;
        }
        else
        {
            mainCamera.transform.position += directionToMove * cameraSpeed * Time.deltaTime;
        }
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
