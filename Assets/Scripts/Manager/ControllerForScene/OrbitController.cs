using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OrbitController : MonoBehaviour
{
    [SerializeField] private Fade fade;
    [SerializeField] private Slider changeView;
    [SerializeField] private CelestialObject planet;
    [SerializeField] private List<float> timeSpeed;
    [SerializeField] private TMP_Text changeSpeedText;

    private int currentTimeSpeed;
    private bool isShowGroupButton;
    private float originValue;
    private float orignSize;
    private void Start()
    {
        currentTimeSpeed = 1;
        ViewSpeed();
        fade.FadeOut( () => fade.gameObject.SetActive(false));
        originValue = changeView.value;
        orignSize = Camera.main.orthographicSize;
    }
    
    public void BackToHome()
    {
        if (!fade.gameObject.activeSelf)
        {
            fade.gameObject.SetActive(true);
        }
        fade.FadeIn(() =>
        {
            //reset time scale
            Time.timeScale = 1;
            SceneManager.LoadScene(Constant.HOME_SCENE_NAME); 
        });
    }

    public void ChangeViewCamera()
    {
        float newValue = changeView.value / originValue * orignSize;
        if(newValue < 1) newValue = 1;
        Camera.main.DOOrthoSize(newValue, 0.2f).SetEase(Ease.Linear);
    }

    public void ChangeSpeedCelestial()
    {
        currentTimeSpeed = (currentTimeSpeed + 1) % timeSpeed.Count;
        ViewSpeed();
    }

    private void ViewSpeed()
    {
        float time = timeSpeed[currentTimeSpeed];
        changeSpeedText.SetText(time.ToString());
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
