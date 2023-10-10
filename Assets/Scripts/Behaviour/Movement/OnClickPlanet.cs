using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour;
using Behaviour.Movement;
using Data;
using TMPro;
using UnityEngine;

public class OnClickPlanet : MonoBehaviour
{
    [SerializeField] private GameObject circleChoice;
    [SerializeField] private TMP_Text planetNameText;

    private void Start()
    {
        circleChoice.SetActive(false);
    }
    private void OnMouseDown()
    {
        CelestialObject planet = GetComponent<CelestialObject>();
        SetNameForPlanet(planet);
    }
    
    private void SetNameForPlanet(CelestialObject planet)
    {
        if(circleChoice.activeSelf) return;
        
        circleChoice.SetActive(true);
        var oldCelestial = CelestialManager.Instance.currentCelestialObject;
        if (oldCelestial != null)
        {
            oldCelestial.GetComponent<OnClickPlanet>().HideNamePlanet();
        }
        CelestialManager.Instance.currentCelestialObject = planet;
        planetNameText.SetText(planet.celestialObjectData.infomation.name);
    }

    private void HideNamePlanet()
    {
        circleChoice.SetActive(false);
    }

    public void OnCLickButtonVisited()
    {
        // move to the current planet
    }
}
