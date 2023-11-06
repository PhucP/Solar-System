using System.Collections;
using System.Collections.Generic;
using Behaviour;
using Behaviour.Movement;
using UnityEngine;
using UnityEngine.Rendering;

public class Earth : CelestialObject
{
    [SerializeField] private float orbitSpeed;
    private Transform sunTransform;
    protected override void Start()
    {
        base.Start();
        sunTransform = CelestialManager.Instance.SunTransform;
    }
    
    protected override void InitDataForObject()
    {
        throw new System.NotImplementedException();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //using rotate around instead 
        transform.RotateAround(sunTransform.position, Vector3.up, orbitSpeed * Time.deltaTime);
    }
}
