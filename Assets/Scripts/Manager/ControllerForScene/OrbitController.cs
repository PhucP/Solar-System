using System.Collections;
using System.Collections.Generic;
using Behaviour;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OrbitController : MonoBehaviour
{
    [SerializeField] private Fade fade;
    [SerializeField] private Slider changeView;
    [SerializeField] private CelestialObject planet;
    [SerializeField] private GameObject groupButton;
    [SerializeField] private RectTransform bottomButton;
    [SerializeField] private List<Sprite> listSpriteGroupButton;
    [SerializeField] private Image buttonShowHide;
    [SerializeField] private List<float> sizeZoom;

    private int currentSizeZoom;
    private bool isShowGroupButton;
    private float originValue;
    private float orignSize;
    private void Start()
    {
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
