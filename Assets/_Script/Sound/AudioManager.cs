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
    private float FadeInDuration = 1f;

    [SerializeField]
    private float FadeOutDuration = 1f;

    private AudioSource _oldAudioSource;

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

        _soundEffectDictionary = new Dictionary<string, SoundEffects>();
        _backgroundMusicDictionary = new Dictionary<string, BackgroundMusic>();
        
        foreach (SoundEffects soundEffect in _soundEffects)
            _soundEffectDictionary.Add(soundEffect.Id, soundEffect);
        
        foreach (BackgroundMusic backgroundMusic in _backgroundMusic)
            _backgroundMusicDictionary.Add(backgroundMusic.Id, backgroundMusic);   
    }

    private void Start()
    {
        IsHasPlayerPrefs();
    }

    private void IsHasPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
            LoadVolumes();
        if (PlayerPrefs.HasKey("MusicVolume"))
            LoadVolumes();
        if (PlayerPrefs.HasKey("SFXVolume"))
            LoadVolumes();
    }

    private void LoadVolumes()
    {
        Debug.Log("Set Default Volume");
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume");
        _volumeMixer.SetFloat("master", Mathf.Log10(masterVolume) * 20);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        _volumeMixer.SetFloat("music", Mathf.Log10(musicVolume) * 20);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
        _volumeMixer.SetFloat("sfx", Mathf.Log10(sfxVolume) * 20);
    }

    public void PlaySoundEffects(string id)
    {
        SoundEffects soundEffect = _soundEffectDictionary.ContainsKey(id) ? _soundEffectDictionary[id] : null;
        AudioSource audioSource = Instantiate(_audioSourcePrefab);
        audioSource.outputAudioMixerGroup = _volumeMixer.FindMatchingGroups("SFX")[0];

        if (soundEffect == null)
        {
            Debug.LogWarning("Sound Effect: " + name + " not found!");
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
        audioSource.volume = UnityEngine.Random.Range(soundEffect.MinVolume, soundEffect.MaxVolume);
        audioSource.pitch = UnityEngine.Random.Range(soundEffect.MinPitch, soundEffect.MaxPitch);     
    }
    
    private IEnumerator DestroyAfterSoundEffect(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
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
        float inverseDuration = 1 / FadeOutDuration;
        for (float t = 0; t < FadeOutDuration; t += Time.deltaTime)
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
        float inverseDuration = 1 / FadeInDuration;
        for (float t = 0; t < FadeInDuration; t += Time.deltaTime)
        {
            newMusic.volume = Mathf.Lerp(0, endVolume, t * inverseDuration);
            yield return null;
        } // 淡入
        newMusic.volume = endVolume; // 確保音量設置為最終值
    }

}
