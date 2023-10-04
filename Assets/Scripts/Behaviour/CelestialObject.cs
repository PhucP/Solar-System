using System;
using Behaviour.Movement;
using Data;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace Behaviour
{
    public abstract class CelestialObject : MonoBehaviour
    {
        [SerializeField] protected CelestialObjectData celestialObjectData;
        [SerializeField] private Vector3 startPosition;
        [SerializeField] protected Vector3 initialVelocity;
        [SerializeField] private int limitPosition;
        [SerializeField] private bool isStart;
        private Rigidbody Rigidbody => GetComponent<Rigidbody>();
        private static CelestialManager CelestialManager => CelestialManager.Instance;
        protected Vector3 currentVelocity;

        protected virtual void Start()
        {
            Reset();
        }

        private void Reset()
        {
            transform.localPosition = startPosition;
            currentVelocity = initialVelocity;
        }

        private void OnValidate()
        {
            Reset();
        }

        private void UpdateVelocity(float timeStep)
        {
            foreach (CelestialObject planet in CelestialManager.listCelestialObject)
            {
                if (planet.Equals(this)) continue;

                var vectorDistance = planet.Rigidbody.position - Rigidbody.position;
                float sqrDst = vectorDistance.sqrMagnitude;
                Vector3 forceDir = vectorDistance.normalized;

                float planetMass = planet.celestialObjectData.physic.mass;
                float mass = celestialObjectData.physic.mass;
                Vector3 force = forceDir * (Constant.G * mass * planetMass) / sqrDst;
                Vector3 acceleration = force / mass;

                currentVelocity += acceleration * timeStep;
            }
        }

        private void UpdatePosition(float timeStep)
        {
            Rigidbody.position += currentVelocity * timeStep;
        }

        protected virtual void FixedUpdate()
        {
            if (isStart)
            {
                UpdateVelocity(CelestialManager.timeStep);
                UpdatePosition(CelestialManager.timeStep);
            }

            if (celestialObjectData.physic.isRotateAroundItsSelf) RotateAroundItsSelf();
        }

        protected abstract void InitDataForObject();
        
        protected virtual void RotateAroundItsSelf()
        {
            Vector3 pos = this.transform.position;
            Vector3 axis = celestialObjectData.infomation.axis;
            float angle = celestialObjectData.physic.rotationSpeed;
            transform.RotateAround(pos, axis, angle * Time.deltaTime);
        }

        protected Vector3 CalculateInitialForce()
        {
            Vector3 resForce = Vector3.zero;
            foreach (var planet in CelestialManager.listCelestialObject)
            {
                if (planet.Equals(this)) continue;
                var m2 = planet.celestialObjectData.physic.mass;
                float r = Vector3.Distance(this.transform.position, planet.transform.position);

                this.transform.LookAt(planet.transform);

                resForce += this.transform.right * Mathf.Sqrt((Constant.G * m2) / r);
            }

            return resForce;
        }
    }
}