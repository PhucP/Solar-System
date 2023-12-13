using System;
using System.Collections.Generic;
using System.Reflection;
using I2.Loc;
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

        [Header("Information To Show")] public InformationToShow showName;
        public InformationToShow showMass;
        public InformationToShow showEquatorialDiameter;
        public InformationToShow showMeanDisFromSun;
        public InformationToShow showRotationPeriod;
        public InformationToShow showSolarOrbitPeriod;
        public InformationToShow showSurfaceGravity;
        public InformationToShow showSurfaceTemperature;
        public InformationToShow showDescription;

        public void Inittialize()
        {
            List<InformationToShow> listInfo = new List<InformationToShow>()
            {
                showName, showMass, showEquatorialDiameter,
                showMeanDisFromSun, showRotationPeriod, showSolarOrbitPeriod,
                showSurfaceGravity, showSurfaceTemperature, showDescription
            };

            SetTerm(listInfo);
#if UNITY_EDITOR
            AddTerm(listInfo);
#endif
        }

        private void AddTerm(List<InformationToShow> listInfo)
        {
            foreach (var informationToShow in listInfo)
            {
                var term = informationToShow.term;
                if (!LocalizationManager.Sources[0].ContainsTerm(term))
                {
                    LocalizationManager.Sources[0].AddTerm(term, eTermType.Text);
                    var termData = LocalizationManager.Sources[0].GetTermData(term);
                    termData.SetTranslation(0, informationToShow.isLongText ? informationToShow.longInfo : informationToShow.info);
                }
            }
        }

        private void SetTerm(List<InformationToShow> listInfo)
        {
            foreach (var informationToShow in listInfo)
            {
                informationToShow.term = $"txt_{nameOfCelestialObject}_{informationToShow.nameInfo}";
            }
        }
    }

    [Serializable]
    public class InformationToShow
    {
        public bool isLongText;
        public string nameInfo;
        [ConditionalShow("isLongText", false)] public string info;

        [ConditionalShow("isLongText", true), TextArea(20, 20)]
        public string longInfo;

        [HideInInspector] public string term;
    }
}