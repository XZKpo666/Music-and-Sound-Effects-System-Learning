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

    [SerializeField]
    private GameObject _confirmResolutionChange;

    [SerializeField]
    private Button _cancelResolutionButton;

    [SerializeField]
    private Button _confirmResolutionButton;

    [SerializeField]
    private Text _countdownText;

    private VideoManager _videoManager;
    private bool _isOnVSync;
    private int _framerate;
    private int _displayMode;
    private int _resolutionIndex;
    private int _oldResolutionIndex;
    private bool _isResolutionChange = false;
    private float _countdownTime = 15f;

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
        _cancelResolutionButton.onClick.AddListener(CancelResolution);
        _confirmResolutionButton.onClick.AddListener(ConfirmResolution);
    }

    private void OnDisable()
    {
        _framerateSlider.onValueChanged.RemoveListener(DraggingFramerateSlider);
        _framerateInputField.onEndEdit.RemoveListener(FramerateInputField);
        _vSyncToggle.onValueChanged.RemoveListener(ToggleVSync);
        _displayModeDropdown.onValueChanged.RemoveAllListeners();
        _applyButton.onClick.RemoveListener(ApplyVideoSettings);
        _resolutionDropdown.onValueChanged.RemoveListener(OnResolutionChange);
        _cancelResolutionButton.onClick.RemoveListener(CancelResolution);
        _confirmResolutionButton.onClick.RemoveListener(ConfirmResolution);
    }

    private void Update()
    {
        CountdownToRevertResolution();
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
        AddResolutionsOptions();
        _resolutionDropdown.value = _videoManager.ResolutionIndex;
        _oldResolutionIndex = _videoManager.ResolutionIndex;
        _isResolutionChange = false;
    }

    private void AddResolutionsOptions()
    {
        _videoManager.LoadResolutionsOptions();
        _resolutionDropdown.ClearOptions();
        _resolutionDropdown.AddOptions(_videoManager.ResolutionOptions);
    }

    private void ApplyVideoSettings()
    {
        _videoManager.ChangeFrameRate((int)_framerateSlider.value);
        _videoManager.SetVSync(_isOnVSync);
        _videoManager.ChangeDisplayMode(_displayMode);
        _videoManager.SetResolution(_resolutionIndex);
        _videoManager.ChangeResolution();
        if (_isResolutionChange)
            ConfirmResolutionChange();
    }

    private void OnResolutionChange(int resolutionIndex)
    {
        _resolutionIndex = resolutionIndex;
        _isResolutionChange = _resolutionIndex != _oldResolutionIndex;
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

    private void ConfirmResolutionChange()
    {
        _confirmResolutionChange.SetActive(true);
    }

    private void CancelResolution()
    {
        _videoManager.RevertResolution();
        OnResolutionChange(_oldResolutionIndex);
        _resolutionDropdown.value = _oldResolutionIndex;
        _isResolutionChange = false;
        _confirmResolutionChange.SetActive(false);
        _countdownTime = 15f;
    }

    private void ConfirmResolution()
    {
        _oldResolutionIndex = _resolutionIndex;
        _isResolutionChange = false;
        _confirmResolutionChange.SetActive(false);
        _countdownTime = 15f;
    }

    private void CountdownToRevertResolution()
    {
        if (_confirmResolutionChange.activeSelf)
        {
            _countdownTime -= Time.deltaTime;
            _countdownText.text = Mathf.Ceil(_countdownTime).ToString() + "秒";
            if (_countdownTime <= 0f)
            {
                CancelResolution();
                _countdownTime = 15f;
            }
        }
    }
}
