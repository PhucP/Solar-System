using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour;
using Behaviour.Movement;
using Unity.Mathematics;
using UnityEngine;

public class PathHandler : MonoBehaviour
{
    [SerializeField] private int limitPosition;
    [SerializeField] private float space;
    private Vector3 _currentLinePosition;
    private Vector3 _currentLineVelocity;
    private CelestialManager _celestialManager => CelestialManager.Instance;
    private LineRenderer _lineRenderer => GetComponent<LineRenderer>();
    private GameObject cloneOfThisPlanet;
    private CelestialObject _celestialObject => GetComponent<CelestialObject>();
    

    public void VisualizePath()
    {
        if (cloneOfThisPlanet == null)
        {
            cloneOfThisPlanet = Instantiate(_celestialManager.clonePlanet, _celestialObject.transform.position, quaternion.identity);
        }
        
        cloneOfThisPlanet.transform.position = _celestialObject.transform.position;
            
        var cloneRb = cloneOfThisPlanet.GetComponent<Rigidbody>();
        var cloneVelocity = _celestialObject.initialVelocity;
        ForceManager forceManager = new ForceManager(_celestialManager.listCelestialObject, _celestialObject.celestialObjectData);
            
        List<Vector3> pathPonits = new List<Vector3>();
        for (int i = 0; i < limitPosition; i++)
        {
            cloneVelocity += forceManager.CalculateGravityForce(cloneRb.position);
            cloneRb.position += cloneVelocity;
            Physics.Simulate(Time.fixedDeltaTime);
            pathPonits.Add(cloneRb.position);
        }

        _lineRenderer.enabled = true;
        _lineRenderer.positionCount = pathPonits.Count;
        _lineRenderer.SetPositions(pathPonits.ToArray());
    }
}
