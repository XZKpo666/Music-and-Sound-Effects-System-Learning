using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoSettings : MonoBehaviour
{
    [SerializeField]
    private Button _applyButton;

    [SerializeField]
    private Slider _framerateSlider;

    [SerializeField]
    private InputField _framerateInputField;

    [SerializeField]
    private Toggle _vSyncToggle;

    [SerializeField]
    private Dropdown _displayModeDropdown;

    [SerializeField]
    private Dropdown _resolutionDropdown;

    private VideoManager _videoManager;
    private bool _isOnVSync;
    private int _framerate;
    private int _displayMode;
    private int _resolutionIndex;
    private Resolution[] _resolutions;
    private List<string> _resolutionOptions = new List<string>();
    private List<Resolution> _currentHzResolutions = new List<Resolution>();

    private void Start()
    {
        _videoManager = ServiceLocator.Instance.GetService<VideoManager>();
        LoadVideoSettings();
    }

    private void OnEnable()
    {
        _framerateSlider.onValueChanged.AddListener(DraggingFramerateSlider);
        _framerateInputField.onEndEdit.AddListener(FramerateInputField);
        _vSyncToggle.onValueChanged.AddListener(ToggleVSync);
        _displayModeDropdown.onValueChanged.AddListener(ChangeDisplayModeDropdown);
        _applyButton.onClick.AddListener(ApplyVideoSettings);
        _resolutionDropdown.onValueChanged.AddListener(OnResolutionChange);
    }

    private void OnDisable()
    {
        _framerateSlider.onValueChanged.RemoveListener(DraggingFramerateSlider);
        _framerateInputField.onEndEdit.RemoveListener(FramerateInputField);
        _vSyncToggle.onValueChanged.RemoveListener(ToggleVSync);
        _displayModeDropdown.onValueChanged.RemoveAllListeners();
        _applyButton.onClick.RemoveListener(ApplyVideoSettings);
        _resolutionDropdown.onValueChanged.RemoveListener(OnResolutionChange);
    }

    private void LoadVideoSettings()
    {
        _framerate = _videoManager.Framerate;
        _framerateSlider.value = _framerate;
        _framerateInputField.text = _framerate.ToString();
        _isOnVSync = _videoManager.IsOnVSync;
        _vSyncToggle.isOn = _isOnVSync;
        if (_isOnVSync == true)
        {
            _framerateSlider.interactable = false;
            _framerateInputField.interactable = false;
        }
        _displayMode = _videoManager.DisplayMode;
        _displayModeDropdown.value = _displayMode;
        LoadResolutionsOptions();
        _resolutionDropdown.value = _videoManager.ResolutionIndex;
    }
    
    private void LoadResolutionsOptions()
    {
        _resolutions = Screen.resolutions;
        double currentHz = Screen.currentResolution.refreshRateRatio.value;
        foreach (Resolution res in _resolutions)
        {
            if (res.refreshRateRatio.value != currentHz) continue;
            string option = res.width + " x " + res.height;
            if (!_resolutionOptions.Contains(option))
            {
                _resolutionOptions.Add(option);
                _currentHzResolutions.Add(res);
            }
        }
        _videoManager._resolutions = _currentHzResolutions.ToArray();
        _resolutionDropdown.ClearOptions();
        _resolutionDropdown.AddOptions(_resolutionOptions);
    }

    private void ApplyVideoSettings()
    {
        _videoManager.ChangeFrameRate((int)_framerateSlider.value);
        _videoManager.SetVSync(_isOnVSync);
        _videoManager.ChangeDisplayMode(_displayMode);
        _videoManager.SetResolution(_resolutionIndex);
        _videoManager.ChangeResolution();
    }

    private void OnResolutionChange(int resolutionIndex)
    {
        _resolutionIndex = resolutionIndex;
    }

    private void DraggingFramerateSlider(float framerate)
    {
        _framerate = (int)framerate;
        _framerateInputField.text = _framerate.ToString();
    }

    private void FramerateInputField(string framerate)
    {
        if (int.TryParse(framerate, out int result))
        {
            _framerate = Mathf.Clamp(result, 30, 256);
            _framerateSlider.value = _framerate;
            _framerateInputField.text = _framerate.ToString();
        }
        else
        {
            _framerateInputField.text = _framerate.ToString();
        }
    }

    private void ToggleVSync(bool isOn)
    {
        _isOnVSync = isOn;
        _framerateSlider.interactable = !isOn;
        _framerateInputField.interactable = !isOn;
    }

    private void ChangeDisplayModeDropdown(int displaymode)
    {
        _displayMode = displaymode;
    }
}
