using UnityEngine;
using UnityEngine.UI;

public class DeveloperOptions : MonoBehaviour
{
    [SerializeField]
    private Button _backToMainMenuButton;

    private LevelLoader _levelLoader;

    private void Start()
    {
        _levelLoader = ServiceLocator.Instance.GetService<LevelLoader>();
    }

    private void OnEnable()
    {
        _backToMainMenuButton.onClick.AddListener(BackToMainMenu);
    }

    private void OnDisable()
    {
        _backToMainMenuButton.onClick.RemoveListener(BackToMainMenu);
    }

    private void BackToMainMenu()
    {
        _levelLoader.LoadLevel(1);
    }
}
