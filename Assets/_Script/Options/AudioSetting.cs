using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [SerializeField]
    private Slider _masterSlider;

    [SerializeField]
    private Slider _musicSlider;

    [SerializeField]
    private Slider _sfxSlider;

    private AudioManager _audioManager;

    void Start()
    {
        _audioManager = ServiceLocator.Instance.GetService<AudioManager>();
        SetVolumeListener();
        LoadVolume();  
    }

    private void LoadVolume()
    {
        _masterSlider.value = _audioManager.MasterVolume;
        _musicSlider.value = _audioManager.MusicVolume;
        _sfxSlider.value = _audioManager.SfxVolume;
    }

    private void SetVolumeListener()
    {
        _masterSlider.onValueChanged.AddListener(DraggingMasterVolumeSlider);
        _musicSlider.onValueChanged.AddListener(DraggingMusicVolumeSlider);
        _sfxSlider.onValueChanged.AddListener(DraggingSFXVolumeSlider);
    }

    private void DraggingMasterVolumeSlider(float volume)
    {
        _audioManager.ChangeMasterVolume(volume);
    }

    private void DraggingMusicVolumeSlider(float volume)
    {
        _audioManager.ChangeMusicVolume(volume);
    }

    private void DraggingSFXVolumeSlider(float volume)
    {
        _audioManager.ChangeSFXVolume(volume);
    }
}
