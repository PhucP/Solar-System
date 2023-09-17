using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Audio Source")]
    public class AudioSource : ScriptableObject
    {
        public AudioClip mainSound;
    }
}
