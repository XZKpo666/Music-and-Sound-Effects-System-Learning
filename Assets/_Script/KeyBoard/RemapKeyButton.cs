using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RemapKeyButton : MonoBehaviour
{      
    [SerializeField]
    private Button RebindButton;

    private InputActionReference Action;
    private int BindingIndex;
    private Text DisplayText;
    private GameObject _disableClickBlocker;
    private KeyBoardRebindManager _keyBoardRebindManager;

    private void OnEnable()
    {
        RebindButton.onClick.AddListener(RemapClicked);
    }

    private void OnDisable()
    {
        RebindButton.onClick.RemoveListener(RemapClicked);
    }

    public void Init(InputActionReference action, int bindingIndex, Text displayText, GameObject disableClickBlocker)
    {
        Action = action;
        BindingIndex = bindingIndex;
        DisplayText = displayText;
        _disableClickBlocker = disableClickBlocker;
    }

    private void Start()
    {
        _keyBoardRebindManager = ServiceLocator.Instance.GetService<KeyBoardRebindManager>();
    }

    private void RemapClicked()
    {
        _disableClickBlocker.SetActive(true);
        DisplayText.text = "...";

        _keyBoardRebindManager.RemapButtonClicked(Action, BindingIndex, newKeyPath =>
        {
            DisplayText.text = newKeyPath;
            _disableClickBlocker.SetActive(false);
        });
    }
}
