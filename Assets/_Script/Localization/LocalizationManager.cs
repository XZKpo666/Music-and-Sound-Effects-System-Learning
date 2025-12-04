using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizationManager : MonoBehaviour, IGameService
{
    [Header("狀態")]
    public int CurrentLanguageIndex;

    private void OnEnable()
    {
        ServiceLocator.Instance.RegisterService(this, false);
    }

    private void OnDisable()
    {
        ServiceLocator.Instance.RemoveService<LocalizationManager>(false);
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
            SystemLanguage systemLanguage = Application.systemLanguage;
            CurrentLanguageIndex = ConvertSystemLanguage(systemLanguage);
            TranslateLanguage(CurrentLanguageIndex);
        }
        else
        {
            LoadLanguage();
        }
    }

    private int ConvertSystemLanguage(SystemLanguage systemLanguage)
    {
        switch (systemLanguage)
        {
            case SystemLanguage.Chinese:
            case SystemLanguage.ChineseTraditional:
            case SystemLanguage.ChineseSimplified:
                return 1;
                    
            default:
                return 0;
        }
    }
}
