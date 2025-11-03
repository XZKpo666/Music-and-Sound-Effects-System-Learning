using System;
using System.Collections.Generic;
using UnityEngine;

public class VideoManager : MonoBehaviour, IGameService
{

    public int Framerate;
    public bool IsOnVSync;
    public int DisplayMode;
    public int ResolutionIndex;
    public Resolution[] Resolutions;
    public List<string> ResolutionOptions = new List<string>();
    private Resolution _currentResolution;
    private Resolution _oldResolution;
    private FullScreenMode _currentDisplayMode;
    private List<Resolution> _currentResolutions = new List<Resolution>();
    private RefreshRate _currentRefreshRate = new RefreshRate {};

    private void OnEnable()
    {
        ServiceLocator.Instance.RegisterService(this, false);
    }

    private void OnDisable()
    {
        ServiceLocator.Instance.RemoveService<VideoManager>(false);
    }

    private void Start()
    {
        LoadResolutionsOptions();
        LoadVideoSettings();
    }

    private void LoadVideoSettings()
    {
        //獲取使用者資料並設定
        if (PlayerPrefs.HasKey("VSyncCount"))
            SetVSync(PlayerPrefs.GetInt("VSyncCount") == 1);
        else
            SetVSync(true); //預設開啟垂直同步

        if (PlayerPrefs.HasKey("Framerate"))
            ChangeFrameRate(PlayerPrefs.GetInt("Framerate"));
        else
            ChangeFrameRate(60); //預設60幀
            
        if (PlayerPrefs.HasKey("DisplayMode"))
            ChangeDisplayMode(PlayerPrefs.GetInt("DisplayMode"));
        else
            ChangeDisplayMode(0); //預設全螢幕    

        if (PlayerPrefs.HasKey("ResolutionIndex"))
            SetResolution(PlayerPrefs.GetInt("ResolutionIndex"));
        else
            SetResolution(Resolutions.Length - 1); //預設最高解析度
            
        ChangeResolution();
    }

    public void SetVSync(bool isOn)
    {
        IsOnVSync = isOn;
        //設定垂直同步
        QualitySettings.vSyncCount = IsOnVSync ? 1 : 0;
        //儲存使用者資料
        PlayerPrefs.SetInt("VSyncCount", IsOnVSync ? 1 : 0);
    }

    public void ChangeFrameRate(int framerate)
    {
        Framerate = framerate;
        Application.targetFrameRate = Framerate;
        PlayerPrefs.SetInt("Framerate", Framerate);
    }

    public void ChangeDisplayMode(int displaymode)
    {
        switch (displaymode)
        {
            case 0:
                _currentDisplayMode = FullScreenMode.FullScreenWindow;
                break;
            case 1:
                _currentDisplayMode = FullScreenMode.MaximizedWindow;
                break;
            case 2:
                _currentDisplayMode = FullScreenMode.Windowed;
                break;
        }
        DisplayMode = displaymode;
        PlayerPrefs.SetInt("DisplayMode", displaymode);
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex >= 0 && resolutionIndex < Resolutions.Length)
        {
            _oldResolution = _currentResolution;
            _currentResolution = Resolutions[resolutionIndex];
            ResolutionIndex = resolutionIndex;
            PlayerPrefs.SetInt("ResolutionIndex", ResolutionIndex);
        }
    }

    public void LoadResolutionsOptions()
    {
        Resolutions = Screen.resolutions;
        int maxWidth = Screen.currentResolution.width;
        int maxHeight = Screen.currentResolution.height;
        foreach (Resolution res in Resolutions)
        {
            if (res.width < 800) continue;
            if (res.width * 9 != res.height * 16) continue;
            if (res.width > maxWidth || res.height > maxHeight) continue;
            string option = res.width + " x " + res.height;
            if (!ResolutionOptions.Contains(option))
            {
                ResolutionOptions.Add(option);
                _currentResolutions.Add(res);
            }
        }
        Resolutions = _currentResolutions.ToArray();
    }

    public void ChangeResolution()
    {
        Screen.SetResolution(_currentResolution.width, _currentResolution.height, _currentDisplayMode, _currentRefreshRate);
    }

    public void RevertResolution()
    {
        _currentResolution = _oldResolution;
        ChangeResolution();
    }
}
