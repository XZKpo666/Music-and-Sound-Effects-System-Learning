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

    private VideoManager _videoManager;
    private bool _isOnVSync;
    private int _framerate;
    private int _displayMode;

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
    }

    private void OnDisable()
    {
        _framerateSlider.onValueChanged.RemoveListener(DraggingFramerateSlider);
        _framerateInputField.onEndEdit.RemoveListener(FramerateInputField);
        _vSyncToggle.onValueChanged.RemoveListener(ToggleVSync);
        _displayModeDropdown.onValueChanged.RemoveAllListeners();
        _applyButton.onClick.RemoveListener(ApplyVideoSettings);
    }

    private void LoadVideoSettings()
    {
        _framerate = _videoManager.Framerate;
        _framerateSlider.value = _framerate;
        _framerateInputField.text = _framerate.ToString();
        _isOnVSync = _videoManager.IsOnVSync;
        _vSyncToggle.isOn = _isOnVSync;
        _displayMode = _videoManager.DisplayMode;
        _displayModeDropdown.value = _displayMode;
        UpdateFrameRateUIInteractable();        
    }

    private void ApplyVideoSettings()
    {
        _videoManager.ChangeFrameRate((int)_framerateSlider.value);
        _videoManager.SetVSync(_isOnVSync);
        _videoManager.ChangeDisplayMode(_displayMode);
        UpdateFrameRateUIInteractable();  
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
    }

    private void UpdateFrameRateUIInteractable()
    {
        if (_isOnVSync == true)
        {
            _framerateSlider.interactable = false;
            _framerateInputField.interactable = false;
        }
        else
        {
            _framerateSlider.interactable = true;
            _framerateInputField.interactable = true;
        }
    }

    private void ChangeDisplayModeDropdown(int displaymode)
    {
        _displayMode = displaymode;
    }
}
