using UnityEngine;

namespace Behaviour.Movement
{
    public class Sun : CelestialObject
    {
        protected override Vector3 CalculateForce()
        {
            return Vector3.forward;
        }

        protected override void HandleMovement()
        {
            base.HandleMovement();
        }
    }
}
