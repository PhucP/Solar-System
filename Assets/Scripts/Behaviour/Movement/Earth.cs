using System.Collections;
using System.Collections.Generic;
using Behaviour;
using UnityEngine;
using UnityEngine.Rendering;

public class Earth : CelestialObject
{
    protected override void InitDataForObject()
    {
        throw new System.NotImplementedException();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //DrawLineRenderer();
    }
}
