using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyBoardRebindManager : MonoBehaviour, IGameService
{
    [SerializeField]
    private InputActionAsset _inputActionAsset;
    private InputActionRebindingExtensions.RebindingOperation _currentRebind;
    private string _rebinds;
    public event Action OnRebindStarted;
    public event Action OnRebindComplete;

    private void OnEnable()
    {
        ServiceLocator.Instance.RegisterService(this, false);
    }

    private void OnDisable()
    {
        ServiceLocator.Instance.RemoveService<KeyBoardRebindManager>(false);
    }

    private void Start()
    {
        LoadRebinds();
    }

    public void RemapButtonClicked(InputActionReference inputAction, int bindingIndex,  System.Action<string> onComplete)
    {
        _inputActionAsset.FindActionMap("Player").Disable(); // 停用 Player 動作以避免干擾
        OnRebindStarted?.Invoke();
        InputAction action = inputAction.action;
        _currentRebind = action.PerformInteractiveRebinding()
            .WithTargetBinding(bindingIndex)
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation =>
            {
                operation.Dispose();
                SaveRebinds();
                Debug.Log($"{inputAction.name} rebind complete.");
                
                // 回傳目前的按鍵路徑（給 UI 更新顯示）
                onComplete?.Invoke(GetBindingPathDisplayName(action.bindings[bindingIndex]));
                OnRebindComplete?.Invoke();
                _inputActionAsset.FindActionMap("Player").Enable(); // 重新啟用 Player 動作
            });

        _currentRebind.Start();
    }

    private string GetBindingPathDisplayName(InputBinding binding)
    {
        return InputControlPath.ToHumanReadableString(
        binding.effectivePath,
        InputControlPath.HumanReadableStringOptions.OmitDevice
        );
    }

    private void SaveRebinds()
    {
        _rebinds = _inputActionAsset.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", _rebinds);
    }

    private void LoadRebinds()
    {
        if (!PlayerPrefs.HasKey("rebinds"))
            return;

        _rebinds = PlayerPrefs.GetString("rebinds");
        _inputActionAsset.LoadBindingOverridesFromJson(_rebinds);
    }

    public void RestoreDefaults()
    {
        _inputActionAsset.RemoveAllBindingOverrides();
        SaveRebinds();
    }
}
