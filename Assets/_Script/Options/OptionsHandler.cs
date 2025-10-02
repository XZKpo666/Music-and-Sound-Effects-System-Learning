using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsHandler : MonoBehaviour
{
    [SerializeField]
    private Button _closeButton;

    [SerializeField]
    private Button _gameSettingsButton;

    [SerializeField]
    private Button _audioSettingsButton;

    [SerializeField]
    private Button _videoSettingsButton;

    [SerializeField]
    private Button _controllerSettingsButton;

    [SerializeField]
    private Button _keyBoardSettingsButton;

    [SerializeField]
    private Material _defaultSettingMaterial;

    [SerializeField]
    private Material _selectedSettingMaterial;

    [SerializeField]
    private Image[] _settingsMaterialGroup;

    [SerializeField]
    private GameObject[] _settingsScrollViewGroup;

    private Image _openingSettingsMaterial;
    private GameObject _openingSettings;

    private void Awake()
    {
        //預設開啟第一個設定
        _openingSettings = _settingsScrollViewGroup[0];
        _openingSettings.SetActive(true);
        _openingSettingsMaterial = _settingsMaterialGroup[0];
        _openingSettingsMaterial.material = _selectedSettingMaterial;
    }    

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(CloseOptions);
        _gameSettingsButton.onClick.AddListener(OpenGameSettings);
        _audioSettingsButton.onClick.AddListener(OpenAudioSettings);
        _videoSettingsButton.onClick.AddListener(OpenVideoSettings);
        _controllerSettingsButton.onClick.AddListener(OpenControllerSettings);
        _keyBoardSettingsButton.onClick.AddListener(OpenKeyBoardSettings);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(CloseOptions);
        _gameSettingsButton.onClick.RemoveListener(OpenGameSettings);
        _audioSettingsButton.onClick.RemoveListener(OpenAudioSettings);
        _videoSettingsButton.onClick.RemoveListener(OpenVideoSettings);
        _controllerSettingsButton.onClick.RemoveListener(OpenControllerSettings);
        _keyBoardSettingsButton.onClick.RemoveListener(OpenKeyBoardSettings);
    }

    private void OpenGameSettings()
    {
        TurnOnSettings(0);
    }

    private void OpenAudioSettings()
    {
        TurnOnSettings(1);
    }

    private void OpenVideoSettings()
    {
        TurnOnSettings(2);
    }

    private void OpenControllerSettings()
    {
        TurnOnSettings(3);
    }

    private void OpenKeyBoardSettings()
    {
        TurnOnSettings(4);
    }

    private void CloseOptions()
    {
        Destroy(gameObject);
    }

    private void TurnOnSettings(int settingid)
    {
        //關閉 當前開啟的設定
        //將 當前開啟設定的名稱圖片材質 轉換為 預設材質
        _openingSettings.SetActive(false);
        _openingSettingsMaterial.material = _defaultSettingMaterial;

        //開啟 新設定
        //將 新設定的名稱圖片材質 轉換為 選擇材質
        _settingsScrollViewGroup[settingid].SetActive(true);
        _settingsMaterialGroup[settingid].material = _selectedSettingMaterial;

        //將 新設定 儲存為 當前開啟設定
        //將 新設定的名稱圖片材質 儲存為 當前開啟設定的名稱圖片材質
        _openingSettings = _settingsScrollViewGroup[settingid];
        _openingSettingsMaterial = _settingsMaterialGroup[settingid];
    }
}
