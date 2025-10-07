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

    private LevelLoader _levelLoader;

    private void Start()
    {
        _levelLoader = ServiceLocator.Instance.GetService<LevelLoader>();
    }

    private void OnEnable()
    {
        _startGameButton.onClick.AddListener(StartGame);
        _openOptions.onClick.AddListener(OpenOptions);
    }

    private void OnDisable()
    {
        _startGameButton.onClick.RemoveListener(StartGame);
        _openOptions.onClick.RemoveListener(OpenOptions);
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
}
