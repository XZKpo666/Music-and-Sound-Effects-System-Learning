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
    private Button _remapForwardButton;

    [SerializeField]
    private Text _remapForwardButtonText;   

    [SerializeField]
    private Button _remapBackwardButton;

    [SerializeField]
    private Text _remapBackwardButtonText;

    [SerializeField]
    private Button _remapLeftButton;

    [SerializeField]
    private Text _remapLeftButtonText;

    [SerializeField]
    private Button _remapRightButton;

    [SerializeField]
    private Text _remapRightButtonText;

    [SerializeField]
    private InputActionReference _moveAction;

    private KeyBoardRebindManager _keyBoardRebindManager; 

    private void Start()
    {
        _keyBoardRebindManager = ServiceLocator.Instance.GetService<KeyBoardRebindManager>();
        UpdateKeyDisplay();
    }

    private void OnEnable()
    {
        _remapForwardButton.onClick.AddListener(ForwardRemapClicked);
        _remapBackwardButton.onClick.AddListener(BackwardRemapClicked);
        _remapLeftButton.onClick.AddListener(LeftRemapClicked);
        _remapRightButton.onClick.AddListener(RightRemapClicked);
        _restoreDefaultsButton.onClick.AddListener(RestoreDefaultsClicked);       
    }

    private void OnDisable()
    {
        _remapForwardButton.onClick.RemoveListener(ForwardRemapClicked);
        _remapBackwardButton.onClick.RemoveListener(BackwardRemapClicked);
        _remapLeftButton.onClick.RemoveListener(LeftRemapClicked);
        _remapRightButton.onClick.RemoveListener(RightRemapClicked);
        _restoreDefaultsButton.onClick.RemoveListener(RestoreDefaultsClicked);
    }

    private void UpdateKeyDisplay()
    {
        KeyDisplay(_moveAction, 4, _remapForwardButtonText);
        KeyDisplay(_moveAction, 5, _remapBackwardButtonText);
        KeyDisplay(_moveAction, 2, _remapLeftButtonText);
        KeyDisplay(_moveAction, 3, _remapRightButtonText);
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

    private void ForwardRemapClicked()
    {
        RemapClicked(_moveAction, 4, _remapForwardButtonText); 
    }

    private void BackwardRemapClicked()
    {
        RemapClicked(_moveAction, 5, _remapBackwardButtonText);
    }

    private void LeftRemapClicked()
    {
        RemapClicked(_moveAction, 2, _remapLeftButtonText);
    }

    private void RightRemapClicked()
    {
        RemapClicked(_moveAction, 3, _remapRightButtonText);
    }

    private void RemapClicked(InputActionReference inputAction, int bindingIndex, Text buttonText)
    {
        _disableClickBlocker.SetActive(true);
        buttonText.text = "...";

        _keyBoardRebindManager.RemapButtonClicked(inputAction, bindingIndex, newKeyPath =>
        {
            buttonText.text = newKeyPath;
            _disableClickBlocker.SetActive(false);
        });
    }
}
