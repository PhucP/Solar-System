using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class PlanetName : MonoBehaviour
{
   public CelestialType celestialType;
   public float defaultCameraPosition;
   
   private Vector3 _lastMousePos;
   private Vector2 _velocity;
   private float _rotationSpeed = 10f;
   private GameObject _observer;

   private bool _isRotating;

   private void Start()
   {
      _observer = PlanetSceneController.Instance.observer;
      _isRotating = false;
      Observer.rotatePlanet += OnRotatePlanet;
   }

   private void FixedUpdate()
   {
      if (_isRotating)
      {
         transform.RotateAround(this.transform.position, Vector3.up, 15f * Time.deltaTime);
      }
   }

   private void OnRotatePlanet()
   {
      _isRotating = !_isRotating;
   }

   private void OnDestroy()
   {
      Observer.rotatePlanet -= OnRotatePlanet;
   }

   void OnMouseDown()
   {
      _lastMousePos = Input.mousePosition;
   }

   void OnMouseDrag()
   {
      Vector3 newMousePos = Input.mousePosition;
      Vector3 delta = newMousePos - _lastMousePos;

      float rotationX = delta.y * _rotationSpeed * Time.deltaTime;
      float rotationY = delta.x * _rotationSpeed * Time.deltaTime;

      Quaternion xQuaternion = Quaternion.AngleAxis(-rotationX, Vector3.right);
      Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.up);

      _observer.transform.localRotation *= xQuaternion * yQuaternion;

      _lastMousePos = newMousePos;
   }

}
