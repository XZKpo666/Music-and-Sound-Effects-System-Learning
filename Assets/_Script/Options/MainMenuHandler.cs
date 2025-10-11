using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _optionsCanvasPrefab;

    [SerializeField]
    private GameObject _mainMenuCanvasPrefab;

    [SerializeField]
    private Button _openOptions;

    [SerializeField]
    private Button _startGameButton;

    [SerializeField]
    private Button _exitGameButton;

    [SerializeField]
    private Button _cleanPlayerPrefsButton;

    private LevelLoader _levelLoader;

    private void Start()
    {
        _levelLoader = ServiceLocator.Instance.GetService<LevelLoader>();
    }

    private void OnEnable()
    {
        _startGameButton.onClick.AddListener(StartGame);
        _openOptions.onClick.AddListener(OpenOptions);
            _exitGameButton.onClick.AddListener(ExitGame);
        _cleanPlayerPrefsButton.onClick.AddListener(CleanPlayerPrefs);
    }

    private void OnDisable()
    {
        _startGameButton.onClick.RemoveListener(StartGame);
        _openOptions.onClick.RemoveListener(OpenOptions);
        _exitGameButton.onClick.RemoveListener(ExitGame);
        _cleanPlayerPrefsButton.onClick.RemoveListener(CleanPlayerPrefs);
    }

    private void StartGame()
    {
        _levelLoader.LoadLevel(2);
    }

    private void OpenOptions()
    {
        Instantiate(_optionsCanvasPrefab);
        Destroy(_mainMenuCanvasPrefab);
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void CleanPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
