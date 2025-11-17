using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, IGameService
{
    public InputActionReference _playerMovement;
    public InputActionReference _escAction;
    private GameUIManager _gameUIManager;

    private void OnEnable()
    {
        ServiceLocator.Instance.RegisterService(this, false);
        _escAction.action.performed += ESC;
    }

    private void OnDisable()
    {
        ServiceLocator.Instance.RemoveService<InputManager>(false);
        _escAction.action.performed -= ESC;
    }

    private void Start()
    {
        _gameUIManager = ServiceLocator.Instance.GetService<GameUIManager>();
    }

    private void ESC(InputAction.CallbackContext context)
    {
        if (_gameUIManager._isMenuOpen == false)
        {
            _gameUIManager.OpenMenu();
        }
        else if (_gameUIManager._isMenuOpen == true)
        {
            if (_gameUIManager._isInSettings == false)
            {
                _gameUIManager.CloseMenu();
                return;
            }
            else if (_gameUIManager._isInSettings == true)
            {
                _gameUIManager.CloseSettings();
            }
        }
    }
}
