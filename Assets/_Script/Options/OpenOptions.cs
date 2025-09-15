using UnityEngine;

public class OpenOptions : MonoBehaviour
{
    [SerializeField] private GameObject _optionsCanvasPrefab;
    public void Open()
    {
        Instantiate(_optionsCanvasPrefab);
    }
    public void Close()
    {
        Destroy(gameObject);
    }
}
