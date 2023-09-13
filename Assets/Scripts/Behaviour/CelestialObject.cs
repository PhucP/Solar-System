using Behaviour.Movement;
using UnityEngine;

namespace Behaviour
{
    public abstract class CelestialObject : MonoBehaviour
    {
        [SerializeField] protected CelestialObjectData celestialObjectData;
        [SerializeField] protected bool isRotateAroundItsSelf;
    
        private Vector3 _force;
        private Rigidbody _rigidbody => GetComponent<Rigidbody>();
        private CelestialManager _celestialManager;

        private void Awake() 
        {

        }

        private void Start() 
        {
            InitDataForObject();
        }

        private void Update()
        {
            _force = CalculateForce();
        }


        private void FixedUpdate()
        {
            HandleMovement();
        }

        protected virtual void InitDataForObject()
        {
            //method can overwrite by child
        }

        protected abstract Vector3 CalculateForce();

        protected virtual void HandleMovement()
        {
            //method can overwrite by child
            _rigidbody.velocity = _force * 5f;
        }
    }
}
