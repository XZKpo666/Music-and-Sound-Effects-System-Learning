using UnityEngine.Audio;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour, IGameService
{   
    [SerializeField] private SoundEffects[] _soundEffects;
    [SerializeField] private BackgroundMusic[] _backgroundMusic;
    [SerializeField] private AudioSource _audioSourcePrefab;
    [SerializeField] private AudioMixer _volumeMixer;
    public float FadeDuration = 1f; // 音量淡出持續時間
    private AudioSource _oldAudioSource;

    private void Awake()
    {  
        DontDestroyOnLoad(gameObject);                   
        Debug.Log("AudioManager: " + name + " RegisterService!");
        ServiceLocator.Instance.RegisterService<AudioManager>(this, false);
    }

    public void PlaySoundEffects(string id)
    {
        SoundEffects soundEffect = Array.Find(_soundEffects, sound => sound.Id == id);
        AudioSource audioSource = Instantiate(_audioSourcePrefab);
        audioSource.outputAudioMixerGroup = _volumeMixer.FindMatchingGroups("SFX")[0];

        IsSoundEffectExist(soundEffect);
        SetSoundEffect(audioSource, soundEffect);
        RandomSoundEffectsValue(audioSource, soundEffect);
        audioSource.Play();   
        StartCoroutine(DestroyAfterSoundEffect(audioSource));
    }

    private void IsSoundEffectExist(SoundEffects soundEffect)
    {
        if (soundEffect == null)
        {
            Debug.LogWarning("Sound Effect: " + name + " not found!");
            return;
        }
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
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        Destroy(audioSource);
    }

    public void PlayBackgroundMusic(string id)
    {
        BackgroundMusic backgroundMusic = Array.Find(_backgroundMusic, sound => sound.Id == id);
        AudioSource audioSource = Instantiate(_audioSourcePrefab);
        audioSource.outputAudioMixerGroup = _volumeMixer.FindMatchingGroups("Music")[0];

        IsBackgroundMusicExist(backgroundMusic);
        IsOldBackgroundMusic(_oldAudioSource);
        SetBackgroundMusic(audioSource, backgroundMusic);

        StartCoroutine(FadeInMusic(audioSource));
        _oldAudioSource = audioSource;
    }

    private void IsBackgroundMusicExist(BackgroundMusic backgroundMusic)
    {
        if (backgroundMusic == null)
        {
            Debug.LogWarning("Background Music: " + name + " not found!");
            return;
        }
    }

    private void IsOldBackgroundMusic(AudioSource oldAudioSource)
    {
        if (_oldAudioSource != null)
        {
            StartCoroutine(FadeOutMusicAndDestroy(_oldAudioSource));
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
        {
            yield break;
        }     
        float startVolume = oldMusicSource.volume;
        for (float t = 0; t < FadeDuration; t += Time.deltaTime)
        {
            oldMusicSource.volume = Mathf.Lerp(startVolume, 0, t / FadeDuration);
            yield return null;
        } // 淡出
        oldMusicSource.Stop();
        Destroy(oldMusicSource); // 銷毀舊音樂物件
    }

    private IEnumerator FadeInMusic(AudioSource newMusic)
    {
        newMusic.Play();
        float endVolume = newMusic.volume;
        newMusic.volume = 0; // 將音量設置為0以便淡入
        for (float t = 0; t < FadeDuration; t += Time.deltaTime)
        {
            newMusic.volume = Mathf.Lerp(0, endVolume, t / FadeDuration);
            yield return null;
        } // 淡入
        newMusic.volume = endVolume; // 確保音量設置為最終值
    }

}
