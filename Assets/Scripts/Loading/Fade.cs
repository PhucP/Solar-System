using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fade : MonoBehaviour
{
    private Image _image => GetComponent<Image>();
    public void FadeIn(float duration, Action actionCallback = null)
    {
        _image.DOFade(1, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => actionCallback?.Invoke());
    }
    
    public void FadeOut(float duration, Action actionCallback = null)
    {
        _image.DOFade(0, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => actionCallback?.Invoke());
    }
}
