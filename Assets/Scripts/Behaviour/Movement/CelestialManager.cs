using System.Collections.Generic;
using UnityEngine;

namespace Behaviour.Movement
{
    public class CelestialManager : Singleton<CelestialManager>
    {
        public List<CelestialObject> listCelestialObject;

        [SerializeField] private Transform celestialParent;
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
                if(celestialScript != null)
                {
                    listCelestialObject.Add(celestialScript);
                }
            }
        }
    }
}
