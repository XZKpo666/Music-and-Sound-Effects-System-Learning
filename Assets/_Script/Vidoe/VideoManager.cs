using UnityEngine;

public class VideoManager : MonoBehaviour, IGameService
{

    public int Framerate;
    public bool IsOnVSync;
    public int DisplayMode;

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
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
            case 3:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
        PlayerPrefs.SetInt("DisplayMode", displaymode);
    }
}
