using Behaviour.Movement;
using Data;
using UnityEngine;

namespace Behaviour
{
    public abstract class CelestialObject : MonoBehaviour
    {
        [SerializeField] protected CelestialObjectData celestialObjectData;
        [SerializeField] protected bool isRotateAroundItsSelf;
    
        protected Vector3 force;
        protected Rigidbody Rigidbody => GetComponent<Rigidbody>();
        protected CelestialManager celestialManager;

        private void Awake() 
        {

        }

        private void Start() 
        {
            InitDataForObject();
        }

        private void Update()
        {
            HandleMovement();
        }


        private void FixedUpdate()
        {
            force = CalculateForce();
        }

        protected virtual void InitDataForObject()
        {
            //method can overwrite by child
        }

        protected abstract Vector3 CalculateForce();

        protected virtual void HandleMovement()
        {
            //method can overwrite by child
            if (isRotateAroundItsSelf) RotateAroundItsSelf();
        }

        protected virtual void RotateAroundItsSelf()
        {
            Vector3 pos = this.transform.position;
            Vector3 axis = celestialObjectData.infomation.axis;
            float angle = celestialObjectData.physic.rotationSpeed;
            if (angle == null) angle = 30;
            if (axis == null) axis = Vector3.up;
            transform.RotateAround(pos, axis, angle * Time.deltaTime);
        }
    }
}
