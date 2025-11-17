using UnityEngine;
using UnityEngine.InputSystem;

public class KeyBoardRebindManager : MonoBehaviour, IGameService
{
    public PlayerInput PlayerInput;
    private InputActionRebindingExtensions.RebindingOperation _currentRebind;

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

    public void RemapButtonClicked(string actionName, int bindingIndex,  System.Action<string> onComplete)
    {
        PlayerInput.actions.FindActionMap("Player").Disable(); // 停用 Player 動作以避免干擾

        InputAction action = PlayerInput.actions[actionName];
        _currentRebind = action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .WithTargetBinding(bindingIndex)
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation =>
            {
                operation.Dispose();
                SaveRebinds();
                Debug.Log($"{actionName} rebind complete.");
                
                // 回傳目前的按鍵路徑（給 UI 更新顯示）
                onComplete?.Invoke(GetBindingPathDisplayName(action.bindings[bindingIndex]));

                PlayerInput.actions.FindActionMap("Player").Enable(); // 重新啟用 Player 動作
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
        string rebinds = PlayerInput.actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }

    private void LoadRebinds()
    {
        if (!PlayerPrefs.HasKey("rebinds"))
            return;

        string rebinds = PlayerPrefs.GetString("rebinds");
        PlayerInput.actions.LoadBindingOverridesFromJson(rebinds);
    }

    public void RestoreDefaults()
    {
        PlayerInput.actions.RemoveAllBindingOverrides();
        SaveRebinds();
    }
}
