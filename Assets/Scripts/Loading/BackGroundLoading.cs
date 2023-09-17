using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Loading
{
    public class BackGroundLoading : MonoBehaviour
    {
        private Image _image;
        private float _screenHeight;
        private float _offset = 30f;
        private float _moveSpeed = 0.3f;
        private RectTransform _rectTransform;
        private float _imageHeight;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _screenHeight = Screen.height;
            _rectTransform = GetComponent<RectTransform>();
            _imageHeight = _rectTransform.rect.height;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            //move down
            _rectTransform.Translate(Vector3.down * (_moveSpeed * Time.deltaTime));
        }

        private bool BelowTheScreen()
        {
            return (_rectTransform.anchoredPosition.y < - _screenHeight / 2 - _imageHeight / 2);
        }

        private void LateUpdate()
        {
            if (BelowTheScreen())
            {
                //set pos above main camera
                _rectTransform.anchoredPosition = new Vector3(0f, _screenHeight / 2 + _offset + _imageHeight, 0f);
            }
        }
    }
}
