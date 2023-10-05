// using System.Collections;
// using System.Collections.Generic;
// using Behaviour;
// using Behaviour.Movement;
// using UnityEngine;
//
// public class PathHandler : MonoBehaviour
// {
//     [SerializeField] private int limitPosition;
//     [SerializeField] private bool isVisualizePath;
//     private Vector3 _currentLinePosition;
//     private Vector3 _currentLineVelocity;
//     private LineRenderer _lineRenderer;
//     private GameObject cloneOfThisPlanet;
//     private Rigidbody Rigidbody => GetComponent<Rigidbody>();
//     private CelestialManager CelestialManager => CelestialManager.Instance;
//     
//     private Vector3 CalculateCurrentLineVelocity()
//     {
//         Vector3 tempForce = Vector3.zero;
//         foreach (CelestialObject planet in CelestialManager.listCelestialObject)
//         {
//             if (planet.Equals(this)) continue;
//
//             var vectorDistance = planet.transform.position - _currentLinePosition;
//             float sqrDst = vectorDistance.sqrMagnitude;
//             Vector3 forceDir = vectorDistance.normalized;
//
//             float planetMass = planet.celestialObjectData.physic.mass;
//             float mass = celestialObjectData.physic.mass;
//             Vector3 force = forceDir * (Constant.G * mass * planetMass) / sqrDst;
//             Vector3 acceleration = force / mass;
//
//             tempForce += acceleration;
//         }
//             
//         return tempForce;
//     }
// }
