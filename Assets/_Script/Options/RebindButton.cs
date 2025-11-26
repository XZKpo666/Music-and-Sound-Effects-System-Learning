using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindButton : MonoBehaviour
{      
    [SerializeField]
    private Button _rebindButton;

    public Text DisplayText;
    
    private InputActionReference _action;
    private int _bindingIndex;
    private InputRebindManager _inputRebindManager;
    private bool _isGamepadSettings;

    private void OnEnable()
    {
        _rebindButton.onClick.AddListener(RemapClicked);
    }

    private void OnDisable()
    {
        _rebindButton.onClick.RemoveListener(RemapClicked);
    }

    public void Init(InputActionReference action, int bindingIndex, bool isGamepadSetings)
    {
        _action = action;
        _bindingIndex = bindingIndex;
        _isGamepadSettings = isGamepadSetings;
    }

    private void Start()
    {
        _inputRebindManager = ServiceLocator.Instance.GetService<InputRebindManager>();
        UpdateKeyDisplay();
    }

    private void RemapClicked()
    {
        DisplayText.text = "...";

        if (_isGamepadSettings == true)
        {
            _inputRebindManager.RemapGamepadButtonClicked(_action, _bindingIndex, newKeyPath =>
            {
                DisplayText.text = newKeyPath;
            });
        }
        else
        {
            _inputRebindManager.RemapKeyboardButtonClicked(_action, _bindingIndex, newKeyPath =>
            {
                DisplayText.text = newKeyPath;
            });
        }
    }

    private void UpdateKeyDisplay()
    {
        DisplayText.text = _inputRebindManager.GetBindingPathDisplayName(_action.action.bindings[_bindingIndex]);
    }
}
