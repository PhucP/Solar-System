using System;
using Behaviour.Movement;
using Data;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace Behaviour
{
    public abstract class CelestialObject : MonoBehaviour
    {
        [SerializeField] protected CelestialObjectData celestialObjectData;
        [SerializeField] protected Vector3 initialVelocity;
        [SerializeField] private int limitPosition;
        [SerializeField] private bool isStart;
        private Rigidbody Rigidbody => GetComponent<Rigidbody>();
        private static CelestialManager CelestialManager => CelestialManager.Instance;
        protected Vector3 currentVelocity;

        [Header("LINE RENDERER")] [SerializeField]
        private bool isDrawLine;
        private Vector3 _currentLineVelocity;
        private Vector3 _currentLinePosition;
        private int _indexLine;
        private LineRenderer LineRenderer => GetComponent<LineRenderer>();


        protected virtual void Start()
        {
            currentVelocity = initialVelocity;
            LineRenderer.positionCount = limitPosition;
            _indexLine = 0;
            _currentLinePosition = Rigidbody.position;
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
            if (isDrawLine)
            {
                DrawLineRenderer();
            }
        }

        protected abstract void InitDataForObject();

        #region LineRenderer

        private void DrawLineRenderer()
        {
            LineRenderer.SetPosition(_indexLine++, _currentLinePosition);
            _currentLinePosition += _currentLineVelocity;
            if (_indexLine >= limitPosition) _indexLine = 0;

            CalculateLineVelocity(CelestialManager.timeStep);
        }

        private void CalculateLineVelocity(float timeStep)
        {
            foreach (CelestialObject planet in CelestialManager.listCelestialObject)
            {
                if (planet.Equals(this)) continue;

                var vectorDistance = planet.Rigidbody.position - _currentLinePosition;
                float sqrDst = vectorDistance.sqrMagnitude;
                Vector3 forceDir = vectorDistance.normalized;

                float planetMass = planet.celestialObjectData.physic.mass;
                float mass = celestialObjectData.physic.mass;
                Vector3 force = forceDir * (Constant.G * mass * planetMass) / sqrDst;
                Vector3 acceleration = force / mass;
                
                Vector3 perpendicularVector = Vector3.Cross(acceleration, new Vector3(0, 0, 1));
                perpendicularVector.z = 0;

                _currentLineVelocity += (acceleration + perpendicularVector);
                _currentLinePosition = _currentLinePosition.normalized * 10;
            }
        }

        #endregion


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