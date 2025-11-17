using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour, IGameService
{
    [SerializeField]
    private Button _backToMainMenuButton;

    [SerializeField]
    private Button _settingsButton;

    [SerializeField]
    private Button _backToGameButton;

    [SerializeField]
    private GameObject _settingsUIPrefab;

    public GameObject _settingsUI;

    private GameUIManager _gameUIManager;
    private LevelLoader _levelLoader;

    private void Start()
    {
        _levelLoader = ServiceLocator.Instance.GetService<LevelLoader>();
        _gameUIManager = ServiceLocator.Instance.GetService<GameUIManager>();
    }

    private void OnEnable()
    {
        ServiceLocator.Instance.RegisterService(this, false);
        _backToMainMenuButton.onClick.AddListener(BackToMainMenu);
        _settingsButton.onClick.AddListener(OpenSettings);
        _backToGameButton.onClick.AddListener(BackToGame);
    }

    private void OnDisable()
    {
        ServiceLocator.Instance.RemoveService<MenuHandler>(false);
        _backToMainMenuButton.onClick.RemoveListener(BackToMainMenu);
        _settingsButton.onClick.RemoveListener(OpenSettings);
        _backToGameButton.onClick.RemoveListener(BackToGame);
    }

    private void BackToMainMenu()
    {
        _levelLoader.LoadLevel(1);
        Destroy(gameObject);
    }

    private void OpenSettings()
    {
        _settingsUI = Instantiate(_settingsUIPrefab);
        _gameUIManager._isInSettings = true;
        gameObject.SetActive(false);
    }

    private void BackToGame()
    {
        _gameUIManager._isMenuOpen = false;
        Destroy(gameObject);
    }
}
