using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _volumeMixer;

    [SerializeField]
    private Slider _masterSlider;

    [SerializeField]
    private Slider _musicSlider;

    [SerializeField]
    private Slider _sfxSlider;

    void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
            LoadVolume();
        else
        {
            _masterSlider.onValueChanged.AddListener(SetMasterVolume);
            _musicSlider.onValueChanged.AddListener(SetMusicVolume);
            _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }   
    }

    public void SetMasterVolume(float volume)
    {
        volume = _masterSlider.value;
        _volumeMixer.SetFloat("master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        volume = _musicSlider.value;
        _volumeMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        volume = _sfxSlider.value;
        _volumeMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolume()
    {
        _masterSlider.onValueChanged.AddListener(SetMasterVolume);
        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        _masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        _musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }
}
