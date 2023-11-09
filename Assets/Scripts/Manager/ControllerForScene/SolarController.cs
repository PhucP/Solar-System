using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Movement;
using Manager;
using UnityEngine;

public class SolarController : BaseController
{
    [SerializeField] private GameObject buttonVisit;
    [SerializeField] private StringVariable nameOfCelestialVisited;

    private void Awake()
    {
        Observer.showHideButtonVisit += OnShowHideButtonVisit;
    }

    public void VisitPlanet()
    {
        //loading scene planet with planet variables is name of it
        var visitedCelestialData = CelestialManager.Instance.CurrentCelestialObject.celestialObjectData;
        nameOfCelestialVisited.stringValue = visitedCelestialData.nameOfCelestialObject;
        LoadScene(Constant.PLANETS_SCENE_NAME);
    }

    private void OnDestroy()
    {
        Observer.showHideButtonVisit -= OnShowHideButtonVisit;
    }

    private void OnShowHideButtonVisit(bool isShow)
    {
        buttonVisit.SetActive(isShow);
    }
}
