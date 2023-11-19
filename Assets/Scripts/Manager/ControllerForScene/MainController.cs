using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : Singleton<MainController>
{
    [SerializeField] private int targetFrame;
    protected override void Awake()
    {
        base.Awake();
        
        //set target fps for devices
        Application.targetFrameRate = targetFrame;
    }
}
