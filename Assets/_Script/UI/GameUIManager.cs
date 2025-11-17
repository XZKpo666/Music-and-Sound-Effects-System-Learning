using UnityEditor;
using UnityEngine;

public class GameUIManager : MonoBehaviour, IGameService
{
    public bool _isMenuOpen = false;
    public bool _isInSettings = false;
    private MenuHandler _menuHandler;
    private GameObject _menuUI;

    [SerializeField]
    private GameObject _menuUIPrefab;

    [SerializeField]
    private GameObject _settingsUI;

    private void OnEnable()
    {
        ServiceLocator.Instance.RegisterService(this, false);
    }

    private void OnDisable()
    {
        ServiceLocator.Instance.RemoveService<GameUIManager>(false);
    }

    public void OpenMenu()
    {
        _menuUI = Instantiate(_menuUIPrefab);
        _isMenuOpen = true;
    }

    public void CloseMenu()
    {
        Destroy(_menuUI);
        _isMenuOpen = false;
    }

    public void CloseSettings()
    {
        if (ServiceLocator.Instance.TryGetService(out _menuHandler) == false)
        { 
            _menuHandler = ServiceLocator.Instance.GetService<MenuHandler>();
            _settingsUI = _menuHandler._settingsUI; 
        }      
        _settingsUI = _menuHandler._settingsUI;
        Destroy(_settingsUI);
        _isInSettings = false;
        OpenMenu();
    }
}
