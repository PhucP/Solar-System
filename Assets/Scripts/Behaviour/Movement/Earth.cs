using System.Collections;
using System.Collections.Generic;
using Behaviour;
using UnityEngine;

public class Earth : CelestialObject
{
    protected override void Start()
    {
        base.Start();
        initialForce = CalculateInitialForce();
        Rigidbody.velocity += CalculateInitialForce();
    }

    protected override void InitDataForObject()
    {
        
    }
    
    protected override void HandleMovement()
    {
        base.HandleMovement();
        MoveBelongToGravityForce();
    }
}
