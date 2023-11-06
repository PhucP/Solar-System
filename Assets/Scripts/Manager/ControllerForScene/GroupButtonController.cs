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
    [SerializeField] private RectTransform verticalPanel;
    [SerializeField] private bool isHideVerticalPanel;
    
    private bool isShowGroupButton;

    public bool IsShowGroupButton
    {
        get => isShowGroupButton;
        set => isShowGroupButton = value;
    }

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
        
        isShowGroupButton = !isShowGroupButton;
    }

    private void DoHideGroupButton()
    {
        var offset = isHideVerticalPanel ? verticalPanel.sizeDelta.y + bottomButton.sizeDelta.y : bottomButton.sizeDelta.y;
        groupButton.GetComponent<RectTransform>()
            .DOAnchorPos(new Vector3(0, -offset, 0), 0.25f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
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
                buttonShowHide.sprite = listSpriteGroupButton[1]; 
            });
    }
}
