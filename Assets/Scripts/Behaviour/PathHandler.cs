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
    [SerializeField] private float offsetForce;
    private Vector3 _currentLinePosition;
    private Vector3 _currentLineVelocity;
    private CelestialManager _celestialManager => CelestialManager.Instance;
    private LineRenderer _lineRenderer => GetComponent<LineRenderer>();
    private GameObject cloneOfThisPlanet;
    private CelestialObject _celestialObject => GetComponent<CelestialObject>();

    private Vector3 oldMousePos;
    private Vector3 tempPos;
    

    public void VisualizePath()
    {
        var cloneRb = cloneOfThisPlanet.GetComponent<Rigidbody>();
        var cloneVelocity = _celestialObject.initialVelocity;
        ForceManager forceManager = new ForceManager(_celestialManager.listCelestialObject, _celestialObject.celestialObjectData);
            
        List<Vector3> pathPonits = new List<Vector3>();
        for (int i = 0; i < limitPosition; i++)
        {
            cloneVelocity += forceManager.CalculateGravityForce(cloneRb);
            cloneRb.position += cloneVelocity;
            Physics.Simulate(Time.fixedDeltaTime);
            pathPonits.Add(cloneRb.position);
        }

        _lineRenderer.enabled = true;
        _lineRenderer.positionCount = pathPonits.Count;
        _lineRenderer.SetPositions(pathPonits.ToArray());
    }

    private void OnMouseDown()
    {
        if(cloneOfThisPlanet != null) ResetClonePos();
        else
        {
            cloneOfThisPlanet = Instantiate(_celestialManager.clonePlanet, transform.position, quaternion.identity);
        }
        oldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tempPos = oldMousePos;
    }
    
    private void ResetClonePos()
    {
        Debug.Log(transform.position);
        _lineRenderer.positionCount = 0;
        _lineRenderer.enabled = false;
        cloneOfThisPlanet.transform.position = transform.position;
    }

    private void OnMouseDrag()
    {
        Vector3 newMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (tempPos != newMousePos)
        {
            ResetClonePos();
            tempPos = newMousePos;
            Vector2 deltaMousePos = (newMousePos - oldMousePos) * offsetForce;

            Debug.Log(deltaMousePos);
            _celestialObject.initialVelocity = deltaMousePos;

            VisualizePath();
        }
    }
    
    private void OnMouseUp()
    {
        StopVisualizePath();
    }

    private void StopVisualizePath()
    {
        _celestialObject.IsStart = true;
    }
}
