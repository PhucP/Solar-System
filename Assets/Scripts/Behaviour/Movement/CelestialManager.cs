using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Behaviour.Movement
{
    public class CelestialManager : Singleton<CelestialManager>
    {
        public List<CelestialObject> listCelestialObject;
        public float timeStep;
        public GameObject clonePlanet;
        [SerializeField] private Transform celestialParent;
        [SerializeField] private GameObject celestialInformation;
        [SerializeField] private TMP_Text informationText;
        [SerializeField] private Camera secondCamera;
        private CelestialObject currentCelestialObject;
        public CelestialObject CurrentCelestialObject
        {
            get => currentCelestialObject;
            set
            {
                if (currentCelestialObject != null)
                {
                    //do something with old celestial
                }
                if(currentCelestialObject == value) return;
                currentCelestialObject = value;
                ChangeCurrentCelestialObject();
            }
        }
        protected override void Awake()
        {
            base.Awake();
            GetCelestialForList();
            celestialInformation.SetActive(false);
        }

        private void ChangeCurrentCelestialObject()
        {
            //set information for celestial
            informationText.SetText(currentCelestialObject.celestialObjectData.infomation.name);
            celestialInformation.SetActive(true);
        }

        public void HideInformation()
        {
            celestialInformation.SetActive(false);
        }

        private void GetCelestialForList()  
        {
            foreach(Transform celestialTransform in celestialParent)
            {
                CelestialObject celestialScript = celestialTransform.GetComponent<CelestialObject>();
                if(celestialScript != null && celestialScript.gameObject.activeSelf)
                {
                    listCelestialObject.Add(celestialScript);
                }
            }
        }

        private void FixedUpdate()
        {
            if (celestialInformation != null && currentCelestialObject != null && secondCamera != null)
            {
                celestialInformation.transform.position = secondCamera.WorldToScreenPoint(currentCelestialObject.transform.position);
            }
        }
    }
}
