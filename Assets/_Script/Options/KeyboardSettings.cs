using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class KeyboardSettings : MonoBehaviour
{
    [SerializeField]
    private Button _restoreDefaultsButton;

    [SerializeField]
    private GameObject _disableClickBlocker; 

    [SerializeField]
    private RebindData[] _rebindDatas;
    
    private InputRebindManager _inputRebindManager; 

    private void OnEnable()
    {
        _inputRebindManager = ServiceLocator.Instance.GetService<InputRebindManager>();
        _restoreDefaultsButton.onClick.AddListener(RestoreDefaultsClicked);    
        _inputRebindManager.OnRebindStarted += HandleRebindStarted;
        _inputRebindManager.OnRebindComplete += HandleRebindComplete;

    }

    private void OnDisable()
    {
        _restoreDefaultsButton.onClick.RemoveListener(RestoreDefaultsClicked);
        _inputRebindManager.OnRebindStarted -= HandleRebindStarted;
        _inputRebindManager.OnRebindComplete -= HandleRebindComplete;
    }

    private void Start()
    {
        SendDataToRemapButton();
        UpdateKeyDisplay();
    }

    private void HandleRebindStarted()
    {
        _disableClickBlocker.SetActive(true);
    }

    private void HandleRebindComplete()
    {
        _disableClickBlocker.SetActive(false);
    }

    private void SendDataToRemapButton()
    {
        for (int i = 0; i < _rebindDatas.Length; i++)
        {
            _rebindDatas[i].RebindButton.Init(
                _rebindDatas[i].Action, 
                _rebindDatas[i].BindingIndex,
                false);
        }
    }

    private void UpdateKeyDisplay()
    {
        for (int i = 0; i < _rebindDatas.Length; i++)
        {
            KeyDisplay(_rebindDatas[i].Action,
            _rebindDatas[i].BindingIndex,
            _rebindDatas[i].KeyDisplayText);
        }
    } 

    private void KeyDisplay(InputActionReference actionReference, int bindingIndex, Text buttonText)
    {
        InputAction action = actionReference.action;
        buttonText.text = GetBindingPathDisplayName(action.bindings[bindingIndex]);
    }

    private string GetBindingPathDisplayName(InputBinding binding)
    {
        return InputControlPath.ToHumanReadableString(
        binding.effectivePath,
        InputControlPath.HumanReadableStringOptions.OmitDevice
        );
    }

    private void RestoreDefaultsClicked()
    {
        _inputRebindManager.ResetKeyboardSettings();
        UpdateKeyDisplay();
    }
}
