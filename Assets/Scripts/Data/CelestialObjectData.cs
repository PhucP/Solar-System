using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Celestial Object Data")]
    public class CelestialObjectData : ScriptableObject
    {
        public Infomation infomation;
        public Physic physic;
        public Size size;
    
        [System.Serializable]
        public class Physic
        {
            public double mass;
            public float speed;
            public float rotationSpeed;
        }

        [System.Serializable]
        public class Size
        {
            public string shape;
            public double radius;
        }

        [System.Serializable]
        public class Infomation
        {
            public string name;
            public double age;
            public Vector3 axis;
        }
    }
}

