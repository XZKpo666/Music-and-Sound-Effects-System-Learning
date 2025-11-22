using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class KeyBoardSettings : MonoBehaviour
{
    [SerializeField]
    private Button _restoreDefaultsButton;

    [SerializeField]
    private GameObject _disableClickBlocker; 

    [SerializeField]
    private KeyBoardRebindData[] _keyBoardRebindDatas;
    
    private KeyBoardRebindManager _keyBoardRebindManager; 

    private void Start()
    {
        _keyBoardRebindManager = ServiceLocator.Instance.GetService<KeyBoardRebindManager>();

        for (int i = 0; i < _keyBoardRebindDatas.Length; i++)
        {
            _keyBoardRebindDatas[i].RebindButton.Init(
                _keyBoardRebindDatas[i].Action, 
                _keyBoardRebindDatas[i].BindingIndex, 
                _keyBoardRebindDatas[i].KeyDisplayText);
        }

        UpdateKeyDisplay();

        DisableClickBlockerIsActive();
    }

    private void OnEnable()
    {
        _restoreDefaultsButton.onClick.AddListener(RestoreDefaultsClicked);       
    }

    private void OnDisable()
    {
        _restoreDefaultsButton.onClick.RemoveListener(RestoreDefaultsClicked);
    }

    private void DisableClickBlockerIsActive()
    {
        foreach (var btn in _keyBoardRebindDatas)
        {
            btn.RebindButton.OnRebindStarted += () =>
            {
                _disableClickBlocker.SetActive(true);
            };

            btn.RebindButton.OnRebindComplete += () =>
            {
                _disableClickBlocker.SetActive(false);
            };
        }
    }

    private void UpdateKeyDisplay()
    {
        for (int i = 0; i < _keyBoardRebindDatas.Length; i++)
        {
            KeyDisplay(_keyBoardRebindDatas[i].Action,
            _keyBoardRebindDatas[i].BindingIndex,
            _keyBoardRebindDatas[i].KeyDisplayText);
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
        _keyBoardRebindManager.RestoreDefaults();
        UpdateKeyDisplay();
    }
}
