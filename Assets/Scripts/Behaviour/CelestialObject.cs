using Behaviour.Movement;
using Data;
using UnityEngine;

namespace Behaviour
{
    public abstract class CelestialObject : MonoBehaviour
    {
        [SerializeField] protected CelestialObjectData celestialObjectData;
        [SerializeField] protected Vector3 initialForce;
        
        protected Rigidbody Rigidbody => GetComponent<Rigidbody>();
        private static CelestialManager CelestialManager => CelestialManager.Instance;
        
        protected Vector3 force;

        protected virtual void Start() 
        {
            InitDataForObject();
        }
        
        private void FixedUpdate()
        {
            HandleMovement();
        }

        protected abstract void InitDataForObject();

        protected virtual void HandleMovement()
        {
            //method can overwrite by child
            if (celestialObjectData.physic.isRotateAroundItsSelf) RotateAroundItsSelf();
        }

        protected void MoveBelongToGravityForce()
        {
            foreach (var planet in CelestialManager.listCelestialObject)
            {
                if(planet.Equals(this)) continue;
                Vector3 difference = planet.transform.position - this.transform.position;
                Vector3 direction = difference.normalized;
                float distance = difference.magnitude;
                
                // Calculate gravity force between this planet and the other planet
                var newForce =
                    direction * (Constant.G * (this.celestialObjectData.physic.mass * planet.celestialObjectData.physic.mass)) / (distance * distance);

                Rigidbody.AddForce(newForce);
            }
        }
        

        protected virtual void RotateAroundItsSelf()
        {
            Vector3 pos = this.transform.position;
            Vector3 axis = celestialObjectData.infomation.axis;
            float angle = celestialObjectData.physic.rotationSpeed;
            transform.RotateAround(pos, axis, angle * Time.deltaTime);
        }

        protected Vector3 CalculateInitialForce()
        {
            Vector3 tempForce = Vector3.zero;
            foreach (var planet in CelestialManager.listCelestialObject)
            {
                if(planet.Equals(this)) continue;
                var m1 = this.celestialObjectData.physic.mass;
                var m2 = planet.celestialObjectData.physic.mass;
                float r = Vector3.Distance(this.transform.position, planet.transform.position);
                
                this.transform.LookAt(planet.transform);

                tempForce += this.transform.right * Mathf.Sqrt((Constant.G * m2) / r);
            }
            
            return tempForce;
        }
    }
}
