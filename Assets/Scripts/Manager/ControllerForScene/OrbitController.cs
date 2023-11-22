using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OrbitController : BaseController
{
    [SerializeField] private Slider changeView;
    [SerializeField] private CelestialObject planet;
    [SerializeField] private List<float> timeSpeed;
    //[SerializeField] private TMP_Text changeSpeedText;

    private int _currentTimeSpeed;
    private bool _isShowGroupButton;
    private float _originValue;
    private float _orignSize;
    protected override void Start()
    {
        base.Start();
        
        _currentTimeSpeed = 1;
        ViewSpeed();
        _originValue = changeView.value;
        _orignSize = Camera.main.orthographicSize;
    }

    public void ChangeViewCamera()
    {
        float newValue = changeView.value / _originValue * _orignSize;
        if(newValue < 1) newValue = 1;
        Camera.main.DOOrthoSize(newValue, 0.2f).SetEase(Ease.Linear);
    }

    public void ChangeSpeedCelestial()
    {
        _currentTimeSpeed = (_currentTimeSpeed + 1) % timeSpeed.Count;
        ViewSpeed();
    }

    private void ViewSpeed()
    {
        float time = timeSpeed[_currentTimeSpeed];
        //changeSpeedText.SetText(time.ToString());
        Time.timeScale = time;
    }

    public void StartSimulation()
    {
        planet.Reset();
        planet.isStart = true;
    }

    public void RestartSimulation()
    {
        planet.isStart = false;
        planet.GetComponent<PathHandler>().ResetClonePos();
        planet.Reset();
    }
}
