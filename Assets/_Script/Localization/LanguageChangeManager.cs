using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageChangeManager : MonoBehaviour, IGameService
{
    public int CurrentLanguageIndex;

    private void OnEnable()
    {
        ServiceLocator.Instance.RegisterService(this, false);
    }

    private void OnDisable()
    {
        ServiceLocator.Instance.RemoveService<LanguageChangeManager>(false);
    }

    private void Start()
    {
        InitializeLocalization();
    }

    public void TranslateLanguage(int languageIndex)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageIndex];
        SaveLanguage(languageIndex);
    }

    private void LoadLanguage()
    {
        CurrentLanguageIndex = PlayerPrefs.GetInt("language");
        TranslateLanguage(CurrentLanguageIndex);
    }

    private void SaveLanguage(int language)
    {
        CurrentLanguageIndex = language;
        PlayerPrefs.SetInt("language", language);
        PlayerPrefs.Save();
    }

    private void InitializeLocalization()
    {
        if(!PlayerPrefs.HasKey("language"))
        {
            List<Locale> locales = LocalizationSettings.AvailableLocales.Locales;
            Locale current = LocalizationSettings.SelectedLocale;             
            int CurrentLanguageIndex = locales.IndexOf(current);
            TranslateLanguage(CurrentLanguageIndex);
        }
        else
        {
            LoadLanguage();
        }
    }
}
