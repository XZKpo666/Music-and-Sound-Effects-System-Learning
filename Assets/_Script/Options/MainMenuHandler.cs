using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _optionsCanvasPrefab;

    [SerializeField]
    private Button _openOptions;

    private void OnEnable()
    {
        _openOptions.onClick.AddListener(OpenOptions);

    }

    private void OnDisable()
    {
        _openOptions.onClick.RemoveListener(OpenOptions);
    }

    public void OpenOptions()
    {
        Instantiate(_optionsCanvasPrefab);
    }
}
