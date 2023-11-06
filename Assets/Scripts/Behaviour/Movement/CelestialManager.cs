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
        [SerializeField] private Transform sunTransform;
        private CelestialObject currentCelestialObject;
        
        public Transform SunTransform => sunTransform;

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
            }
        }
        protected override void Awake()
        {
            base.Awake();
            GetCelestialForList();
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
    }
}
