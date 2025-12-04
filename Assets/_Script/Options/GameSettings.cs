using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [SerializeField]
    private Dropdown _languageDropdown;

    private LanguageChangeManager _localizationManager;

    private void Start()
    {
        _localizationManager = ServiceLocator.Instance.GetService<LanguageChangeManager>();
        UpdateSettings();
    }

    private void OnEnable()
    {
        _languageDropdown.onValueChanged.AddListener(ChoeseLanguage);
    }

    private void OnDisable()
    {
        _languageDropdown.onValueChanged.RemoveAllListeners();
    }

    private void UpdateSettings()
    {
        _languageDropdown.value = _localizationManager.CurrentLanguageIndex;
    }

    private void ChoeseLanguage(int languageIndex)
    {
        _localizationManager.TranslateLanguage(languageIndex);
    }
}
