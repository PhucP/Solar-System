using System;
using System.Collections.Generic;
using Behaviour.Movement;
using Data;
using Unity.Mathematics;
using UnityEngine;
namespace Behaviour
{
    public abstract class CelestialObject : MonoBehaviour
    {
        public CelestialObjectData celestialObjectData;
        [SerializeField] private Vector3 startPosition;
        public Vector3 initialVelocity;
        public bool isStart;
        public Rigidbody rigidbody => GetComponent<Rigidbody>();
        private CelestialManager CelestialManager => CelestialManager.Instance;
        protected Vector3 currentVelocity;
        private ForceManager _forceManager;

        protected virtual void Start()
        {
            Reset();
            Init();
            
        }

        private void Init()
        {
            if (CelestialManager != null)
            {
                _forceManager = new ForceManager(CelestialManager.listCelestialObject, celestialObjectData);
            }
        }

        public void Reset()
        {
            transform.localPosition = startPosition;
            currentVelocity = initialVelocity;
        }

        private void OnValidate()
        {
            Reset();
        }

        protected virtual void FixedUpdate()
        {
            if (isStart)
            {
                //currentVelocity += CalculateVelocity(CelestialManager.timeStep);
                currentVelocity += _forceManager.CalculateGravityForce(rigidbody, false);
                UpdatePosition(CelestialManager.timeStep);
            }

            if (celestialObjectData.physic.isRotateAroundItsSelf) RotateAroundItsSelf();
        }
        
        private void UpdatePosition(float timeStep)
        {
            rigidbody.position += currentVelocity * timeStep;
        }
        
        protected abstract void InitDataForObject();
        
        protected virtual void RotateAroundItsSelf()
        {
            Vector3 pos = this.transform.position;
            Vector3 axis = celestialObjectData.infomation.axis;
            float angle = celestialObjectData.physic.rotationSpeed;
            transform.RotateAround(pos, axis, angle * Time.deltaTime);
        }
    }
}