using UnityEngine.Audio;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour, IGameService
{   
    [SerializeField] private SoundEffects[] _soundEffects;
    [SerializeField] private BackgroundMusic[] _backgroundMusic;
    [SerializeField] private GameObject _audioObjectPrefab;
    public float FadeDuration = 1f; // 音量淡出持續時間
    private GameObject _oldAudioObject;

    private void Awake()
    {  
        DontDestroyOnLoad(gameObject);                   
        Debug.Log("AudioManager: " + name + " RegisterService!");
        ServiceLocator.Instance.RegisterService<AudioManager>(this, false);
    }

    public void PlaySoundEffects(int id)
    {      
        GameObject audioObject = Instantiate(_audioObjectPrefab);
        SoundEffects soundEffect = Array.Find(_soundEffects, sound => sound.Id == id);   
        AudioSource audioSource =  audioObject.AddComponent<AudioSource>();
        audioSource.clip = soundEffect.AudioCilp;
        audioSource.volume = soundEffect.Volume;
        audioSource.pitch = soundEffect.Pitch;
        audioSource.loop = soundEffect.Loop;        

        if (soundEffect == null)
        {
            Debug.LogWarning("Sound Effect: " + name + " not found!");
            return;
        }

        RandomSoundEffects(audioSource, soundEffect.MinVolume, soundEffect.MaxVolume, soundEffect.MinPitch, soundEffect.MaxPitch);
        audioSource.Play();
        StartCoroutine(DestroyAfterSound(audioObject, audioSource));
    }

    public void PlayBackgroundMusic(int id)
    {
        GameObject audioObject = Instantiate(_audioObjectPrefab);
        BackgroundMusic backgroundMusic = Array.Find(_backgroundMusic, sound => sound.Id == id);
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic.AudioCilp;
        audioSource.volume = backgroundMusic.Volume;
        audioSource.pitch = backgroundMusic.Pitch;
        audioSource.loop = backgroundMusic.Loop;
        
        if (backgroundMusic == null)
        {
            Debug.LogWarning("Background Music: " + name + " not found!");
            return;
        }

        if (_oldAudioObject != null)
        {
            StartCoroutine(FadeOutMusicAndDestroy(_oldAudioObject));
        }
        
        StartCoroutine(FadeInMusic(audioSource));
        
        _oldAudioObject = audioObject; 
    }

    private void RandomSoundEffects(AudioSource audioSource, float minVolume, float maxVolume, float minPitch, float maxPitch)
    {             
        audioSource.volume = UnityEngine.Random.Range(minVolume, maxVolume);
        audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);     
    }

    private IEnumerator DestroyAfterSound(GameObject gameObject, AudioSource audioSource)
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
    
    private IEnumerator FadeOutMusicAndDestroy(GameObject oldMusicObject)
    {
        if (oldMusicObject == null)
        {
            yield break;
        }
        
        AudioSource oldMusic = oldMusicObject.GetComponent<AudioSource>();
        float startVolume = oldMusic.volume;
        for (float t = 0; t < FadeDuration; t += Time.deltaTime)
        {
            oldMusic.volume = Mathf.Lerp(startVolume, 0, t / FadeDuration);
            yield return null;
        }
        oldMusic.Stop();
        Destroy(oldMusicObject); // 銷毀舊音樂物件
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
        }
        newMusic.volume = endVolume; // 確保音量設置為最終值
    }

}
