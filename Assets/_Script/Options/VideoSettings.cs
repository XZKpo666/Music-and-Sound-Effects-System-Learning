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

    private VideoManager _videoManager;
    private bool _isOnVSync;
    private int _framerate;
    private bool _isFromInputField = false;


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
        _applyButton.onClick.AddListener(ApplyVideoSettings);
    }

    private void OnDisable()
    {
        _framerateSlider.onValueChanged.RemoveListener(DraggingFramerateSlider);
        _framerateInputField.onEndEdit.RemoveListener(FramerateInputField);
        _vSyncToggle.onValueChanged.RemoveListener(ToggleVSync);
        _applyButton.onClick.RemoveListener(ApplyVideoSettings);
    }

    private void LoadVideoSettings()
    {
        _framerate = _videoManager.Framerate;
        _framerateSlider.value = _framerate;
        _framerateInputField.text = _framerate.ToString();
        _isOnVSync = _videoManager.IsOnVSync;
        _vSyncToggle.isOn = _isOnVSync;
        UpdateFrameRateUIInteractable();
    }

    private void ApplyVideoSettings()
    {
        _videoManager.ChangeFrameRate((int)_framerateSlider.value);
        _videoManager.SetVSync(_isOnVSync);
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
}
