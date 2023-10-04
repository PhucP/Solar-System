using UnityEngine;

namespace Behaviour.Movement
{
    public class Sun : CelestialObject
    {
        protected override void InitDataForObject()
        {
            //throw new System.NotImplementedException();
        }
        
        protected override void FixedUpdate()
        {
            if (celestialObjectData.physic.isRotateAroundItsSelf) RotateAroundItsSelf();
        }
    }
}
