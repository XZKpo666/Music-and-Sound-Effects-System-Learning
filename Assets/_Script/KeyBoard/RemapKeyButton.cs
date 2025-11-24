using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RemapKeyButton : MonoBehaviour
{      
    [SerializeField]
    private Button _rebindButton;

    [SerializeField]
    private Text _displayText;

    private InputActionReference _action;
    private int _bindingIndex;
    
    private KeyBoardRebindManager _keyBoardRebindManager;
    private void OnEnable()
    {
        _rebindButton.onClick.AddListener(RemapClicked);
    }

    private void OnDisable()
    {
        _rebindButton.onClick.RemoveListener(RemapClicked);
    }

    public void Init(InputActionReference action, int bindingIndex)
    {
        _action = action;
        _bindingIndex = bindingIndex;
    }

    private void Start()
    {
        _keyBoardRebindManager = ServiceLocator.Instance.GetService<KeyBoardRebindManager>();
    }

    private void RemapClicked()
    {
        _displayText.text = "...";

        _keyBoardRebindManager.RemapButtonClicked(_action, _bindingIndex, newKeyPath =>
        {
            _displayText.text = newKeyPath;
        });
    }
}
