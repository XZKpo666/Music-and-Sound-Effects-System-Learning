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
    private KeyBoardRebindManager _keyBoardRebindManager;

    public System.Action OnRebindStarted;
    public System.Action OnRebindComplete;

    private void OnEnable()
    {
        RebindButton.onClick.AddListener(RemapClicked);
    }

    private void OnDisable()
    {
        RebindButton.onClick.RemoveListener(RemapClicked);
    }

    public void Init(InputActionReference action, int bindingIndex, Text displayText)
    {
        Action = action;
        BindingIndex = bindingIndex;
        DisplayText = displayText;
    }

    private void Start()
    {
        _keyBoardRebindManager = ServiceLocator.Instance.GetService<KeyBoardRebindManager>();
    }

    private void RemapClicked()
    {
        OnRebindStarted?.Invoke();

        DisplayText.text = "...";

        _keyBoardRebindManager.RemapButtonClicked(Action, BindingIndex, newKeyPath =>
        {
            DisplayText.text = newKeyPath;
            OnRebindComplete?.Invoke();
        });
    }
}
