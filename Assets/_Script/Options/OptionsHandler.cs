using UnityEngine;
using UnityEngine.UI;

public class OptionsHandler : MonoBehaviour
{
    [SerializeField]
    private Button _closeButton;

    private void Start()
    {
        _closeButton.onClick.AddListener(CloseOptions);
    }

    private void CloseOptions()
    {
        Destroy(gameObject);
    }
}
