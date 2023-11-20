using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public Slot fromSlot;
    public Slot toSlot;
    
    // Animation Settings
    public float totalDuration;
    public float height;
    public Ease ease;

    private int _resX;
    private int _resY;
    private int _hZ;
    
    private void Awake()
    {
        if (PlayerPrefs.HasKey("resX"))
        {
            _resX = PlayerPrefs.GetInt("resX");
            _resY = PlayerPrefs.GetInt("resY");
            _hZ = PlayerPrefs.GetInt("hZ");
        }
        else
        {
            PlayerPrefs.SetInt("resX", Screen.currentResolution.width);
            PlayerPrefs.SetInt("resY", Screen.currentResolution.height);
            PlayerPrefs.SetInt("hZ", Screen.currentResolution.refreshRate);
            _resX = PlayerPrefs.GetInt("resX");
            _resY = PlayerPrefs.GetInt("resY");
            _hZ = PlayerPrefs.GetInt("hZ");

        }

        Screen.SetResolution((int)(_resX * 0.75f),(int)(_resY * 0.75f), FullScreenMode.FullScreenWindow);
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }
    
}
