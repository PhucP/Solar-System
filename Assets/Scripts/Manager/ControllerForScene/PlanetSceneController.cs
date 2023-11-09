using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Manager;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlanetSceneController : BaseController
{
   [SerializeField] private StringVariable nameStringVariable;
   [SerializeField] private Transform celestialParent;
   [SerializeField] private float offset;
   [SerializeField] private List<GameObject> listPlanet;
   [SerializeField] private TMP_Text nameOfPlanet;

   private int _currentPlanetIndex;
   private GameObject _currentPlanet;
   private Camera _mainCamera;

   private void Awake()
   {
      //create planet
      var visitPlanet = ReadStringVariable();
      var newPlanet = Instantiate(visitPlanet, Vector3.zero, Quaternion.identity);
      _currentPlanet = newPlanet;
      _mainCamera = Camera.main;
   }

   protected override void Start()
   {
      base.Start();

      _mainCamera.transform.DOMoveZ(listPlanet[_currentPlanetIndex].GetComponent<PlanetName>().defaultCameraPosition, 0.5f);
      nameOfPlanet.SetText(listPlanet[_currentPlanetIndex].GetComponent<PlanetName>().celestialType.ToString());
   }

   private void MoveToNewPlanet(bool isRight)
   {
      var newPosition = isRight
         ? _currentPlanet.transform.position + new Vector3(offset, 0, 0)
         : _currentPlanet.transform.position - new Vector3(offset, 0, 0);

      _currentPlanetIndex = isRight
         ? _currentPlanetIndex+1
         : _currentPlanetIndex-1;
      if (_currentPlanetIndex < 0) _currentPlanetIndex = listPlanet.Count - 1;
      if (_currentPlanetIndex > listPlanet.Count - 1) _currentPlanetIndex = 0;
      
      var newPlanet = Instantiate(listPlanet[_currentPlanetIndex], newPosition, Quaternion.identity);
      var newPlanetName = newPlanet.GetComponent<PlanetName>();
      
      //do move camera to new planet
      var newCameraPosition = newPlanet.transform.position;
      _mainCamera.transform
         .DOMove(new Vector3(newCameraPosition.x, newCameraPosition.y, newPlanetName.defaultCameraPosition), 0.5f)
         .OnComplete(() =>
         {
            Destroy(_currentPlanet);
            _currentPlanet = newPlanet;
            nameOfPlanet.SetText(newPlanetName.celestialType.ToString());
         });
   }

   public void SwitchPlanet(bool isRight)
   {
      MoveToNewPlanet(isRight);
   }

   private GameObject ReadStringVariable()
   {
      if (nameStringVariable == null)
         return listPlanet[0];
      var nameOfVisitCelestial = nameStringVariable.stringValue;
      for (int i = 0; i < listPlanet.Count; i++)
      {
         if (listPlanet[i].GetComponent<PlanetName>().celestialType.ToString().Equals(nameOfVisitCelestial))
         {
            _currentPlanetIndex = i;
            return listPlanet[i];
         }
      }

      return listPlanet[0];
   }
}
