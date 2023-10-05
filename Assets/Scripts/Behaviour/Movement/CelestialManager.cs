using System.Collections.Generic;
using UnityEngine;

namespace Behaviour.Movement
{
    public class CelestialManager : Singleton<CelestialManager>
    {
        public List<CelestialObject> listCelestialObject;
        public float timeStep;
        public GameObject clonePlanet;
        [SerializeField] private Transform celestialParent;
        protected override void Awake()
        {
            base.Awake();
            GetCelestialForList();
            timeStep = Time.fixedDeltaTime;
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
