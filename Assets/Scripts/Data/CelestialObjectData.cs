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
            public float mass;
            public float speed;
            public float rotationSpeed;
            public bool isRotateAroundItsSelf;
        }

        [System.Serializable]
        public class Size
        {
            public string shape;
            public float radius;
        }

        [System.Serializable]
        public class Infomation
        {
            public string name;
            public float age;
            public Vector3 axis;
            public float distanceToSun;
        }
    }
}

