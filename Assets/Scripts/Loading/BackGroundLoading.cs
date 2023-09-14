using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Loading
{
    public class BackGroundLoading : MonoBehaviour
    {
        public List<Image> listImage;
        private float _screenHight;
        [FormerlySerializedAs("_screenWeight")] public float screenWeight;

        private void Start()
        {
            
        }
    }
}
