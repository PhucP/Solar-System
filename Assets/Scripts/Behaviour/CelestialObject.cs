using System;
using System.Collections.Generic;
using Behaviour.Movement;
using Data;
using Unity.Mathematics;
using UnityEngine;
namespace Behaviour
{
    public abstract class CelestialObject : MonoBehaviour, IOnClickCelestial
    {
        public CelestialObjectData celestialObjectData;
        [SerializeField] private Vector3 startPosition;
        public Vector3 initialVelocity;
        public bool isStart;
        public Rigidbody rigidbody => GetComponent<Rigidbody>();
        private CelestialManager CelestialManager => CelestialManager.Instance;
        protected Vector3 currentVelocity;
        private ForceManager _forceManager;
        
        [Header("Draw Orbit In Solar Scene")]
        [SerializeField] private bool isOrbitScene;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private int numOfLinePoints;

        protected virtual void Start()
        {
            //Reset();
            Init();
        }

        private void Init()
        {
            if (lineRenderer != null)
            {
                lineRenderer.positionCount = 0;
            }
            if (CelestialManager != null)
            {
                _forceManager = new ForceManager(CelestialManager.listCelestialObject, celestialObjectData);
            }
            
            //draw the orbit if it in Solar Scene
            if (!isOrbitScene)
            {
                DrawCircleOrbit();
            }
        }

        protected virtual void DrawCircleOrbit()
        {
            if (lineRenderer != null)
            {
                lineRenderer.positionCount = numOfLinePoints;
            }
            var ceneter = CelestialManager.SunTransform.position;
            var radius = Vector3.Distance(transform.position, ceneter);
            for (int i = 0; i < numOfLinePoints; i++)
            {
                float angle = (i * 370f / numOfLinePoints) * Mathf.Deg2Rad;
                float x = ceneter.x + radius * Mathf.Cos(angle);
                float z = ceneter.z + radius * Mathf.Sin(angle);
                
                lineRenderer.SetPosition(i, new Vector3(x, ceneter.y, z));
            }
        }

        public void Reset()
        {
            transform.localPosition = startPosition;
            currentVelocity = initialVelocity;
        }
        
        // private void OnValidate()
        // {
        //     Reset();
        // }

        protected virtual void FixedUpdate()
        {
            if (isStart)
            {
                //currentVelocity += CalculateVelocity(CelestialManager.timeStep);
                currentVelocity += _forceManager.CalculateGravityForce(rigidbody, false);
                UpdatePosition(CelestialManager.timeStep);
            }

            if (celestialObjectData.isRotateAroundItsSelf) RotateAroundItsSelf();
        }
        
        private void UpdatePosition(float timeStep)
        {
            rigidbody.position += currentVelocity * timeStep;
        }
        
        protected abstract void InitDataForObject();
        
        protected virtual void RotateAroundItsSelf()
        {
            Vector3 pos = this.transform.position;
            Vector3 axis = celestialObjectData.axis;
            float angle = celestialObjectData.rotationSpeed;
            transform.RotateAround(pos, axis, angle * Time.deltaTime);
        }

        protected void OnMouseDown()
        {
            OnClickCelestial();
        }

        public void OnClickCelestial()
        {
            if (CelestialManager != null)
            {
                CelestialManager.CurrentCelestialObject = this;
            }
        }
    }
}