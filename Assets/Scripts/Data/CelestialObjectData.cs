using System;
using UnityEditor.Rendering;
using UnityEngine;
using Object = System.Object;

namespace Data
{
    [CreateAssetMenu(fileName = "Celestial Object Data")]
    public class CelestialObjectData : ScriptableObject
    {
        public string nameOfCelestialObject;
        public float mass;
        public float rotationSpeed;
        public bool isRotateAroundItsSelf;
        public Vector3 axis;

        public InformationShowed informationShowed;
    }
    
    [Serializable]
    public class InformationShowed
    {
        public string massString;
        public string equatorialDiameter;
        public string meanDistFromSun;
        public string rotationPeriod;
        public string solarOrbitPeriod;
        public string surfaceGravity;
        public string surfaceTemperature;
        [TextArea(20, 5)]public string description;
    }
}

