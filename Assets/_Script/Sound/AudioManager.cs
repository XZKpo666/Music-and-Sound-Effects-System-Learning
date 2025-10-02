using UnityEngine.Audio;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour, IGameService
{
    [SerializeField]
    private SoundEffects[] _soundEffects;

    [SerializeField]
    private BackgroundMusic[] _backgroundMusic;

    private Dictionary<string, SoundEffects> _soundEffectDictionary;
    private Dictionary<string, BackgroundMusic> _backgroundMusicDictionary;

    [SerializeField]
    private AudioSource _audioSourcePrefab;

    [SerializeField]
    private AudioMixer _volumeMixer;

    [SerializeField]
    private float _fadeInDuration = 1f;

    [SerializeField]
    private float _fadeOutDuration = 1f;

    private AudioSource _oldAudioSource;

    public float MasterVolume;
    public float MusicVolume;
    public float SfxVolume;

    private void OnEnable()
    {
        ServiceLocator.Instance.RegisterService(this, false);
    }

    private void OnDisable()
    {
        ServiceLocator.Instance.RemoveService<AudioManager>(false);
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); //不用加到彈射少女

        //創建字典
        _soundEffectDictionary = new Dictionary<string, SoundEffects>();
        _backgroundMusicDictionary = new Dictionary<string, BackgroundMusic>();

        //用字典儲存
        foreach (SoundEffects soundEffect in _soundEffects)
            _soundEffectDictionary.Add(soundEffect.Id, soundEffect);

        foreach (BackgroundMusic backgroundMusic in _backgroundMusic)
            _backgroundMusicDictionary.Add(backgroundMusic.Id, backgroundMusic);
    }

    private void Start()
    {
        //載入音量
        LoadVolumes();
    }

    private void LoadVolumes()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            MasterVolume = PlayerPrefs.GetFloat("MasterVolume");
            _volumeMixer.SetFloat("master", Mathf.Log10(MasterVolume) * 20);
        }
        else
        {
            PlayerPrefs.SetFloat("MasterVolume", 1f);
            _volumeMixer.SetFloat("master", 0f);
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
            _volumeMixer.SetFloat("music", Mathf.Log10(MusicVolume) * 20);
        }
        else
        {
            PlayerPrefs.SetFloat("MusicVolume", 1f);
            _volumeMixer.SetFloat("music", 0f);
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            SfxVolume = PlayerPrefs.GetFloat("SFXVolume");
            _volumeMixer.SetFloat("sfx", Mathf.Log10(SfxVolume) * 20);
        }
        else
        {
            PlayerPrefs.SetFloat("SFXVolume", 1f);
            _volumeMixer.SetFloat("sfx", 0f);
        }
    }

    public void PlaySoundEffects(string id)
    {
        //尋找指定音效，ID查詢
        SoundEffects soundEffect = _soundEffectDictionary.ContainsKey(id) ? _soundEffectDictionary[id] : null;
        AudioSource audioSource = Instantiate(_audioSourcePrefab);
        //找出對應的Mixer
        audioSource.outputAudioMixerGroup = _volumeMixer.FindMatchingGroups("SFX")[0];
        
        //找不到音效就報錯並刪除這個AudioSource
        if (soundEffect == null)
        {
            Debug.LogWarning("Sound Effect: " + name + " not found!");
            Destroy(audioSource.gameObject);
            return;
        }

        SetSoundEffect(audioSource, soundEffect);
        RandomSoundEffectsValue(audioSource, soundEffect);
        audioSource.Play();
        StartCoroutine(DestroyAfterSoundEffect(audioSource));
    }

    private void SetSoundEffect(AudioSource audioSource, SoundEffects soundEffect)
    {
        audioSource.clip = soundEffect.AudioCilp;
        audioSource.volume = soundEffect.Volume;
        audioSource.pitch = soundEffect.Pitch;
        audioSource.loop = soundEffect.Loop;
    }

    private void RandomSoundEffectsValue(AudioSource audioSource, SoundEffects soundEffect)
    {
        audioSource.volume = Random.Range(soundEffect.MinVolume, soundEffect.MaxVolume);
        audioSource.pitch = Random.Range(soundEffect.MinPitch, soundEffect.MaxPitch);
    }

    private IEnumerator DestroyAfterSoundEffect(AudioSource audioSource)
    {
        float actualAudioLength = audioSource.clip.length / audioSource.pitch;
        yield return new WaitForSeconds(actualAudioLength);
        Destroy(audioSource.gameObject);
    }

    public void PlayBackgroundMusic(string id)
    {
        BackgroundMusic backgroundMusic = _backgroundMusicDictionary.ContainsKey(id) ? _backgroundMusicDictionary[id] : null;
        AudioSource audioSource = Instantiate(_audioSourcePrefab);
        audioSource.outputAudioMixerGroup = _volumeMixer.FindMatchingGroups("Music")[0];

        if (backgroundMusic == null)
        {
            Debug.LogWarning("Background Music: " + name + " not found!");
            Destroy(audioSource.gameObject);
            return;
        }
        IsOldBackgroundMusic(_oldAudioSource);
        SetBackgroundMusic(audioSource, backgroundMusic);

        StartCoroutine(FadeInMusic(audioSource));
        _oldAudioSource = audioSource;
    }

    private void IsOldBackgroundMusic(AudioSource oldAudioSource)
    {
        if (oldAudioSource != null)
        {
            StartCoroutine(FadeOutMusicAndDestroy(oldAudioSource));
        }
    }

    private void SetBackgroundMusic(AudioSource audioSource, BackgroundMusic backgroundMusic)
    {
        audioSource.clip = backgroundMusic.AudioCilp;
        audioSource.volume = backgroundMusic.Volume;
        audioSource.pitch = backgroundMusic.Pitch;
        audioSource.loop = backgroundMusic.Loop;
    }

    private IEnumerator FadeOutMusicAndDestroy(AudioSource oldMusicSource)
    {
        if (oldMusicSource == null)
            yield break;

        float startVolume = oldMusicSource.volume;
        float inverseDuration = 1 / _fadeOutDuration;
        for (float t = 0; t < _fadeOutDuration; t += Time.deltaTime)
        {
            oldMusicSource.volume = Mathf.Lerp(startVolume, 0, t * inverseDuration);
            yield return null;
        }
        oldMusicSource.Stop();
        Destroy(oldMusicSource.gameObject); // 銷毀舊音樂物件
    }

    private IEnumerator FadeInMusic(AudioSource newMusic)
    {
        newMusic.Play();
        float endVolume = newMusic.volume;
        newMusic.volume = 0; // 將音量設置為0以便淡入
        float inverseDuration = 1 / _fadeInDuration;
        for (float t = 0; t < _fadeInDuration; t += Time.deltaTime)
        {
            newMusic.volume = Mathf.Lerp(0, endVolume, t * inverseDuration);
            yield return null;
        } // 淡入
        newMusic.volume = endVolume; // 確保音量設置為最終值
    }

    public void ChangeMasterVolume(float volume)
    {
        MasterVolume = volume;
        _volumeMixer.SetFloat("master", Mathf.Log10(MasterVolume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
    }

    public void ChangeMusicVolume(float volume)
    {
        MusicVolume = volume;
        _volumeMixer.SetFloat("music", Mathf.Log10(MusicVolume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
    }

    public void ChangeSFXVolume(float volume)
    {
        SfxVolume = volume;
        _volumeMixer.SetFloat("sfx", Mathf.Log10(SfxVolume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", SfxVolume);
    }
}
