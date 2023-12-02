using System.Collections;
using System.Collections.Generic;
using Manager;
using Unity.VisualScripting;
using UnityEngine;

public class MainController : Singleton<MainController>
{
    [Header("Settings")]
    [SerializeField] private int targetFrame;

    [Header("Popup")] 
    public BasePopup optionPopup;
    
    [Header("Sound")]
    public AudioManager soundManager;
    
    protected override void Awake()
    {
        base.Awake();
        
        //set target fps for devices
        Application.targetFrameRate = targetFrame;
    }

    public void ShowHideOptionPopup(bool isShow)
    {
        optionPopup.gameObject.SetActive(isShow);
    }
}
