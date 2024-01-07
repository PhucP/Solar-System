using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour;
using Behaviour.Movement;
using Manager;
using TMPro;
using UnityEngine;

public class InfoInSolarScene : MonoBehaviour
{
    [SerializeField] private Transform targetCelestial;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject panelText;

    private void Awake()
    {
        //register with the observer
        Observer.showHideAllInformation += OnShowHideInformation;
        
        MoveWithTarget();
    }

    private void FixedUpdate()
    {
        MoveWithTarget();
    }

    private void MoveWithTarget()
    {
        this.transform.position = Camera.main.WorldToScreenPoint(targetCelestial.position);
    }

    public void OnClickInformation()
    {
        //move to this celestial
        CelestialManager.Instance.CurrentCelestialObject = targetCelestial.GetComponent<CelestialObject>();
        Camera.main.GetComponentInParent<CameraController>().OnClickVisited();
        Observer.showHideAllInformation?.Invoke(false);
    }

    private void OnShowHideInformation(bool isShow)
    {
        panelText.gameObject.SetActive(isShow);
    }

    private void OnDestroy()
    {
        //Unregister with the observer
        Observer.showHideAllInformation -= OnShowHideInformation;
    }
}
