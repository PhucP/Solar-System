using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour;
using Behaviour.Movement;
using Data;
using DG.Tweening;
using I2.Loc;
using Manager;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

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

    [SerializeField] private Vector3 originRotationOfObserver;
    [SerializeField] private float stepMove;
    [SerializeField] private Transform sun;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private CameraMode cameraMode;
    [SerializeField] private Vector3 originPosition;
    [SerializeField] private GroupButtonController informationOfCurrentPlanet;
    [SerializeField] private Light additionalLight;
    [SerializeField] private float zoomSpeed = 3f;

    [Header("Information To Show")] [SerializeField]
    private TMP_Text nameOfCurrentPlanet;
    [SerializeField] private Localize localizeName;
    [SerializeField] private TMP_Text massString;
    [SerializeField] private Localize localizeMass;
    [SerializeField] private TMP_Text equatorialDiameter;
    [SerializeField] private Localize localizeEquatorialDiameter;
    [SerializeField] private TMP_Text meanDistFromSun;
    [SerializeField] private Localize localizeMeanDisFromSun;
    [SerializeField] private TMP_Text rotationPeriod;
    [SerializeField] private Localize localizeRotationPeriod;
    [SerializeField] private TMP_Text solarOrbitPeriod;
    [SerializeField] private Localize localizeSolarOrbitPeriod;
    [SerializeField] private TMP_Text surfaceGravity;
    [SerializeField] private Localize localizeSurfaceGravity;
    [SerializeField] private TMP_Text surfaceTemperature;
    [SerializeField] private Localize localizeSurfaceTemperature;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Localize localizeDescription;
    
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;

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
        additionalLight.intensity = 0;
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

        var newPosition = currentCelestialObject.transform.position + direction * 7.5f;

        //change direction if current object is Sun
        var nameOfSun = sun.GetComponent<CelestialObject>().celestialObjectData.nameOfCelestialObject;
        var nameOfCurrentObject = currentCelestialObject.celestialObjectData.nameOfCelestialObject;
        if (nameOfSun.Equals(nameOfCurrentObject))
        {
            direction = (mainCamera.transform.position - sun.position).normalized;
            newPosition = currentCelestialObject.transform.position + direction * 12f;
        }

        var difference = newPosition - mainCamera.transform.position;
        var distance = difference.magnitude;
        var directionToMove = difference.normalized;

        mainCamera.transform.LookAt(currentCelestialObject.transform);
        if (distance < 0.175f)
        {
            mainCamera.transform.position += difference;
            if (!informationOfCurrentPlanet.IsShowGroupButton)
            {
                HideShowAllCelestialDoNotFollow(false);
                ShowInformationForCurrentPlanetFollowing();
                Observer.showHideButtonVisit?.Invoke(true);
            }
        }
        else mainCamera.transform.position += directionToMove * cameraSpeed * Time.deltaTime;
    }

    public void OnClickVisited()
    {
        mainCamera.transform.DOLookAt(celestialManager.CurrentCelestialObject.transform.position, 0.5f)
            .SetEase(Ease.Linear)
            .OnComplete(() => { cameraMode = CameraMode.FollowPlanet; });
    }

    private void ShowInformationForCurrentPlanetFollowing()
    {
        //set information in the panel
        SetInformationForCurrentPlanet();
        //show information panel
        informationOfCurrentPlanet.ShowHideGroupButton();
        additionalLight.DOIntensity(0.5f, 0.5f);
    }

    private void SetInformationForCurrentPlanet()
    {
        var currentPlanet = celestialManager.CurrentCelestialObject;
        var planetData = currentPlanet.celestialObjectData;
        
        //initial planet information
        planetData.Inittialize();
        
        // nameOfCurrentPlanet.SetText(planetData.showName.info);
        // massString.SetText("Mass: " + planetData.showMass.info);
        // equatorialDiameter.SetText("Equatorial Diameter: " + planetData.showEquatorialDiameter.info);
        // meanDistFromSun.SetText("Mean Distance From Sun: " + planetData.showMeanDisFromSun.info);
        // rotationPeriod.SetText("Rotation Period: " + planetData.showRotationPeriod.info);
        // solarOrbitPeriod.SetText("Solar Orbit Period: " + planetData.showSolarOrbitPeriod.info);
        // surfaceGravity.SetText("Surface Gravity: " + planetData.showSurfaceGravity.info);
        // surfaceTemperature.SetText("Surface Temperature: " + planetData.showSurfaceTemperature.info);
        // description.SetText(planetData.showDescription.longInfo);
        
        RefreshLocalization(planetData);
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(verticalLayoutGroup.GetComponent<RectTransform>());
    }

    private void RefreshLocalization(CelestialObjectData data)
    {
        localizeName.SetTerm(data.showName.term);
        localizeMass.SetTerm(data.showMass.term);
        localizeEquatorialDiameter.SetTerm(data.showEquatorialDiameter.term);
        localizeMeanDisFromSun.SetTerm(data.showMeanDisFromSun.term);
        localizeRotationPeriod.SetTerm(data.showRotationPeriod.term);
        localizeSolarOrbitPeriod.SetTerm(data.showSolarOrbitPeriod.term);
        localizeSurfaceGravity.SetTerm(data.showSurfaceGravity.term);
        localizeSurfaceTemperature.SetTerm(data.showSurfaceTemperature.term);
        localizeDescription.SetTerm(data.showDescription.term);
        
    }

    private void HideInformationCurrentPlanetFollowing()
    {
        //hide the information panel
        informationOfCurrentPlanet.ShowHideGroupButton();
    }

    public void ExitFollowPlanet()
    {
        Observer.showHideButtonVisit?.Invoke(false);

        //hide information of current planet
        if (informationOfCurrentPlanet.IsShowGroupButton)
        {
            HideShowAllCelestialDoNotFollow(true);
            this.transform.DORotate(originRotationOfObserver, 0.5f);
            cameraMode = CameraMode.ExitFollowPlanet;
            HideInformationCurrentPlanetFollowing();
            additionalLight.DOIntensity(0f, 0.5f);
        }

        //have a position to move back is original position
        var difference = originPosition - mainCamera.transform.position;
        var distance = difference.magnitude;
        var directionToMove = difference.normalized;

        //do look at the Sun while move back
        mainCamera.transform.LookAt(sun);

        //change state to change rotation when finish the moving
        if (distance < 0.175f)
        {
            cameraMode = CameraMode.ChangeRotation;
            mainCamera.transform.DOLocalMove(originPosition, 1f);
            mainCamera.transform
                .DOLocalRotate(Vector3.zero, 1f)
                .OnComplete(() => { Observer.showHideAllInformation?.Invoke(true); });
        }
        else
        {
            mainCamera.transform.position += directionToMove * cameraSpeed * Time.deltaTime;
        }
    }

    private void Zoom()
    {
        if (MainController.Instance.optionPopup.gameObject.activeSelf) return;
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
                mainCamera.transform.position -= zoomModifier * direction;
            }

            if (touchesPrePosDifference < touchesCurPosDifference)
            {
                mainCamera.transform.position += zoomModifier * direction;
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
            var newPos = mainCamera.transform.position + direction * scrollDelta * stepMove;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, newPos, smoothTime);
        }
    }

    private void ChangeRotation()
    {
        if (MainController.Instance.optionPopup.gameObject.activeSelf) return;
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

    private void HideShowAllCelestialDoNotFollow(bool isShow)
    {
        foreach (var celestial in celestialManager.listCelestialObject)
        {
            if (celestial != celestialManager.CurrentCelestialObject)
            {
                celestial.gameObject.SetActive(isShow);
            }
            else
            {
                var lineRenderer = celestial.GetComponent<LineRenderer>().enabled = isShow;
            }
        }
    }
}