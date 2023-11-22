using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Manager;
using UnityEngine.UI;
using UnityEngine;

public class PlanetSceneController : BaseController
{
   public static PlanetSceneController Instance; 
   
   [SerializeField] private StringVariable nameStringVariable;
   [SerializeField] private float offset;
   [SerializeField] private List<GameObject> listPlanet;
   [SerializeField] private Text nameOfPlanet;
   [SerializeField] private Toggle rotateToggle;
   public GameObject observer;

   private int _currentPlanetIndex;
   private GameObject _currentPlanet;
   private Camera _mainCamera;
   private bool _canMove;

   private void Awake()
   {
      //singleton initialization
      if (Instance != null)
      {
         Destroy(Instance);
         return;
      }

      Instance = this;
      
      //create planet
      var visitPlanet = ReadStringVariable();
      var newPlanet = Instantiate(visitPlanet, Vector3.zero, Quaternion.identity);
      _currentPlanet = newPlanet;
      _mainCamera = Camera.main;
      _canMove = true;
      rotateToggle.isOn = false;
   }

   public void OnClickRotate(bool isRotate)
   {
      Observer.rotatePlanet?.Invoke();
   }

   protected override void Start()
   {
      base.Start();

      _mainCamera.transform.DOMoveZ(listPlanet[_currentPlanetIndex].GetComponent<PlanetName>().defaultCameraPosition, 0.5f);
      nameOfPlanet.text = (listPlanet[_currentPlanetIndex].GetComponent<PlanetName>().celestialType.ToString());
   }

   private void MoveToNewPlanet(bool isRight)
   {
      rotateToggle.isOn = false;
      _canMove = false;
      var newPosition = isRight
         ? _currentPlanet.transform.position + _mainCamera.transform.right * offset
         : _currentPlanet.transform.position - _mainCamera.transform.right * offset;

      _currentPlanetIndex = isRight
         ? _currentPlanetIndex+1
         : _currentPlanetIndex-1;
      if (_currentPlanetIndex < 0) _currentPlanetIndex = listPlanet.Count - 1;
      if (_currentPlanetIndex > listPlanet.Count - 1) _currentPlanetIndex = 0;
      
      var newPlanet = Instantiate(listPlanet[_currentPlanetIndex], newPosition, Quaternion.identity);
      var newPlanetName = newPlanet.GetComponent<PlanetName>();
      
      //do move camera to new planet
      var newCameraPosition = newPlanet.transform.position;
      _mainCamera.transform.DOLocalMove(new Vector3(0, 0, newPlanetName.defaultCameraPosition), 0.5f);
      observer.transform
         .DOMove(newCameraPosition, 0.5f)
         .OnComplete(() =>
         {
            _canMove = true;
            
            Destroy(_currentPlanet);
            _currentPlanet = newPlanet;
            nameOfPlanet.text = (newPlanetName.celestialType.ToString());
         });
   }

   public void SwitchPlanet(bool isRight)
   {
      if(!_canMove) return;
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
