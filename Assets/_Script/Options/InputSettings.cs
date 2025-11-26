using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InputSettings : MonoBehaviour
{
    [SerializeField]
    private Button _restoreDefaultsButton;

    [SerializeField]
    private GameObject _disableClickBlocker; 

    [SerializeField]
    private RebindData[] _rebindDatas;

    [SerializeField]
    private bool _isGamepadSettings; 
    
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
        SendDataToRebindButton();
    }

    private void HandleRebindStarted()
    {
        _disableClickBlocker.SetActive(true);
    }

    private void HandleRebindComplete()
    {
        _disableClickBlocker.SetActive(false);
    }

    private void SendDataToRebindButton()
    {
        for (int i = 0; i < _rebindDatas.Length; i++)
        {
            _rebindDatas[i].RebindButton.Init(
                _rebindDatas[i].Action, 
                _rebindDatas[i].BindingIndex,
                _isGamepadSettings);
        }
    }

    private void RestoreDefaultsClicked()
    {
        if (_isGamepadSettings == true)
            _inputRebindManager.ResetControllerSettings();
        else
            _inputRebindManager.ResetKeyboardSettings();

        for (int i = 0; i < _rebindDatas.Length; i++)
        {
            _rebindDatas[i].RebindButton.DisplayText.text = _inputRebindManager.GetBindingPathDisplayName(_rebindDatas[i].Action.action.bindings[_rebindDatas[i].BindingIndex]);
        }
    }
}
