using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class SoundEffects 
{
    public string Id;

    public AudioClip AudioCilp;

    [Range(0f, 1f)] public float Volume;
    public float MinVolume;
    public float MaxVolume;
    [Range(0.1f, 3f)] public float Pitch;
    public float MinPitch;
    public float MaxPitch;
    public bool Loop;
}
