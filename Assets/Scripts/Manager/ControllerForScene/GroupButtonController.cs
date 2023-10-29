using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GroupButtonController : MonoBehaviour
{
    [SerializeField] private GameObject groupButton;
    [SerializeField] private RectTransform bottomButton;
    [SerializeField] private Image buttonShowHide;
    [SerializeField] private List<Sprite> listSpriteGroupButton;
    
    private bool isShowGroupButton;

    private void Start()
    {
        isShowGroupButton = false;
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
