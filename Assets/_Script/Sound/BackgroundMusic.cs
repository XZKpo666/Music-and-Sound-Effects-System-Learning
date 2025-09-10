using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class BackgroundMusic
{
    public string Id;
    public AudioClip AudioCilp;
    [Range(0f, 1f)] public float Volume;
    [Range(0.1f, 3f)] public float Pitch;
    public bool Loop;
}
