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
        [SerializeField] private bool isStart;
        public Rigidbody rigidbody => GetComponent<Rigidbody>();
        private CelestialManager CelestialManager => CelestialManager.Instance;
        protected Vector3 currentVelocity;
        private ForceManager _forceManager;
        private PathHandler _pathHandler => GetComponent<PathHandler>();

        // [Header("VISUALIZE PATH")]
        // [SerializeField] private int limitPosition;
        [SerializeField] private bool isVisualizePath;
        // private Vector3 _currentLinePosition;
        // private Vector3 _currentLineVelocity;
        // private LineRenderer _lineRenderer => GetComponent<LineRenderer>();
        // private GameObject cloneOfThisPlanet;
        //


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

        private void Reset()
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
                currentVelocity += _forceManager.CalculateGravityForce(transform.position);
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

        private void OnMouseDown()
        {
            Debug.Log("here: " + celestialObjectData.infomation.name);
        }

        // #region VisualizePath

        // private void VisualizePath()
        // {
        //     if (cloneOfThisPlanet == null)
        //     {
        //         cloneOfThisPlanet = Instantiate(CelestialManager.clonePlanet, transform.position, quaternion.identity);
        //     }
        //     else cloneOfThisPlanet.transform.position = transform.position;
        //     
        //     var cloneRb = cloneOfThisPlanet.GetComponent<Rigidbody>();
        //     var cloneVelocity = initialVelocity;
        //     
        //     List<Vector3> pathPonits = new List<Vector3>();
        //     for (int i = 0; i < limitPosition; i++)
        //     {
        //         cloneVelocity += CalculateCurrentLineVelocity(cloneRb);
        //         cloneRb.position += cloneVelocity;
        //         Physics.Simulate(Time.fixedDeltaTime);
        //         pathPonits.Add(cloneRb.position);
        //     }
        //
        //     _lineRenderer.enabled = true;
        //     _lineRenderer.positionCount = pathPonits.Count;
        //     _lineRenderer.SetPositions(pathPonits.ToArray());
        // }
        //
        // private Vector3 CalculateCurrentLineVelocity(Rigidbody cloneRb)
        // {
        //     Vector3 tempForce = Vector3.zero;
        //     foreach (CelestialObject planet in CelestialManager.listCelestialObject)
        //     {
        //         if (planet.Equals(this)) continue;
        //
        //         var vectorDistance = planet.rigidbody.position - cloneRb.position;
        //         float sqrDst = vectorDistance.sqrMagnitude;
        //         Vector3 forceDir = vectorDistance.normalized;
        //
        //         float planetMass = planet.celestialObjectData.physic.mass;
        //         float mass = celestialObjectData.physic.mass;
        //         Vector3 force = forceDir * (Constant.G * mass * planetMass) / sqrDst;
        //         Vector3 acceleration = force / mass;
        //
        //         tempForce += acceleration;
        //     }
        //     
        //     return tempForce;
        // }
        //
        // #endregion
    }
}