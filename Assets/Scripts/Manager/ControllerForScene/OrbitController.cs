using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour;
using Behaviour.Movement;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Task = System.Threading.Tasks.Task;

public class OrbitController : MonoBehaviour
{
    [SerializeField] private Fade fade;
    [SerializeField] private Slider changeView;
    [SerializeField] private CelestialObject planet;
    [SerializeField] private GameObject groupButton;
    [SerializeField] private RectTransform bottomButton;
    [SerializeField] private List<Sprite> listSpriteGroupButton;
    [SerializeField] private Image buttonShowHide;
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
        isShowGroupButton = false;
    }
    
    public void BackToHome()
    {
        if (!fade.gameObject.activeSelf)
        {
            fade.gameObject.SetActive(true);
        }
        fade.FadeIn(() =>
        {
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

        // float currentStep = CelestialManager.Instance.timeStep;
        // if(Math.Abs(time - currentStep) < 0.1) return;
        //
        // float step = (time - currentStep) / 10;
        // for (int i = 0; i < 10; i++)
        // {
        //     time += step;
        //     await Task.Delay(200);
        //     CelestialManager.Instance.timeStep = time;
        // }
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

    public void ShowHideGroupButton()
    {
        if (isShowGroupButton)
        {
            DoHideGroupButton();
        }
        else DoShowGroupButton();
    }

    private void DoHideGroupButton()
    {
        groupButton.GetComponent<RectTransform>()
            .DOAnchorPos(new Vector3(0, -bottomButton.sizeDelta.y, 0), 0.25f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                isShowGroupButton = !isShowGroupButton;
                buttonShowHide.sprite = listSpriteGroupButton[0];
            });
    }

    private void DoShowGroupButton()
    {
        groupButton.GetComponent<RectTransform>()
            .DOAnchorPos(Vector3.zero, 0.25f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                isShowGroupButton = !isShowGroupButton;
                buttonShowHide.sprite = listSpriteGroupButton[1]; 
            });
    }
}
