using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : Singleton<HomeController>
{
    [Header("Fades")]
    [SerializeField] private Fade fade;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        fade.FadeOut( () => fade.gameObject.SetActive(false));
    }

    public void LoadScene(string sceneName)
    {
        if (!fade.gameObject.activeSelf)
        {
            fade.gameObject.SetActive(true);
        }
        fade.FadeIn(() =>
        {
            SceneManager.LoadScene(sceneName); 
        });
    }
    
}
