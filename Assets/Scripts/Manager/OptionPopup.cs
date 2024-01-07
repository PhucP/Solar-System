using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

public class OptionPopup : BasePopup
{
    [SerializeField] private Slider soundVolume;
    [SerializeField] private Dropdown dropdownLanguage;

    private void Awake()
    {
        SetupCurrentLanguage();
    }

    private void OnEnable()
    {
        soundVolume.value = MainController.Instance.soundManager.audioSource.volume;
        SetupCurrentLanguage();
    }

    public void OnChangeLanguage()
    {
        var language = dropdownLanguage.value;
        switch (language)
        {
            case 0:
                LocalizationManager.CurrentLanguage = "English";
                break;
            case 1:
                LocalizationManager.CurrentLanguage = "Vietnamese";
                break;
            case 2:
                LocalizationManager.CurrentLanguage = "Japanese";
                break;
            case 3:
                LocalizationManager.CurrentLanguage = "korean";
                break;
            case 4:
                LocalizationManager.CurrentLanguage = "French";
                break;
            default:
                break;
        }
    }

    private void SetupCurrentLanguage()
    {
        var currentLanguage = LocalizationManager.CurrentLanguage.ToString();
        switch (currentLanguage)
        {
            case "English":
                dropdownLanguage.value = 0;
                break;
            case "Vietnamese":
                dropdownLanguage.value = 1;
                break;
            case "Japanese":
                dropdownLanguage.value = 2;
                break;
            case "Korean":
                dropdownLanguage.value = 3;
                break;
            case "French":
                dropdownLanguage.value = 4;
                break;
        }
    }
}
