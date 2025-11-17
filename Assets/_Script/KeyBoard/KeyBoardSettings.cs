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
        InputAction action = _keyBoardRebindManager.PlayerInput.actions["Move"];
        _remapForwardButtonText.text = GetBindingPathDisplayName(action.bindings[4]);
        _remapBackwardButtonText.text = GetBindingPathDisplayName(action.bindings[5]);
        _remapLeftButtonText.text = GetBindingPathDisplayName(action.bindings[2]);
        _remapRightButtonText.text = GetBindingPathDisplayName(action.bindings[3]);
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
        _disableClickBlocker.SetActive(true);
        _remapForwardButtonText.text = "...";

        _keyBoardRebindManager.RemapButtonClicked("Move", 4, newKeyPath =>
        {
            _remapForwardButtonText.text = newKeyPath;
            _disableClickBlocker.SetActive(false);
        });  
    }

    private void BackwardRemapClicked()
    {
        _disableClickBlocker.SetActive(true);
        _remapBackwardButtonText.text = "...";

        _keyBoardRebindManager.RemapButtonClicked("Move", 5, newKeyPath =>
        {
            _remapBackwardButtonText.text = newKeyPath;
            _disableClickBlocker.SetActive(false);
        });
    }

    private void LeftRemapClicked()
    {
        _disableClickBlocker.SetActive(true);
        _remapLeftButtonText.text = "...";

        _keyBoardRebindManager.RemapButtonClicked("Move", 2, newKeyPath =>
        {
            _remapLeftButtonText.text = newKeyPath;
            _disableClickBlocker.SetActive(false);
        });
    }

    private void RightRemapClicked()
    {
        _disableClickBlocker.SetActive(true);
        _remapRightButtonText.text = "...";

        _keyBoardRebindManager.RemapButtonClicked("Move", 3, newKeyPath =>
        {
            _remapRightButtonText.text = newKeyPath;
            _disableClickBlocker.SetActive(false);
        });
    }
}
