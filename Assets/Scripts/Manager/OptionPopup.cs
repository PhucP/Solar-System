using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPopup : BasePopup
{
    [SerializeField] private Slider soundVolume;

    private void OnEnable()
    {
        soundVolume.value = MainController.Instance.soundManager.audioSource.volume;
    }
}
