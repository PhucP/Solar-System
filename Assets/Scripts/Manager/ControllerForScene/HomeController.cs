using System;
using Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeController : BaseController
{
    private AudioManager soundManager;
    protected override void Start()
    {
        soundManager = MainController.Instance.soundManager;
        if(soundManager.audioSource.clip != soundManager.mainSound) soundManager.PlaySound(soundManager.mainSound);
        base.Start();
    }
}
