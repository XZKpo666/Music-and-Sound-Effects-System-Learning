using UnityEngine;

public class AutoLoadScene : MonoBehaviour
{
    private LevelLoader _levelLoader;

    private void Start()
    {
        _levelLoader = ServiceLocator.Instance.GetService<LevelLoader>();
    }

    private void Update()
    {
        LoadScene();
    }

    private void LoadScene()
    {
        _levelLoader.LoadLevel(1);
    }
}
