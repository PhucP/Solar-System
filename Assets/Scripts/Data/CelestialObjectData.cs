using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "Celestial Object Data")]
public class CelestialObjectData : ScriptableObject
{
    [Header("Physic")]
    public Physic physic;

    [Header("Sizze")]
    public Size size;
}

public class Physic
{
    public double mass;
    public float speed;
}

public class Size
{

}
