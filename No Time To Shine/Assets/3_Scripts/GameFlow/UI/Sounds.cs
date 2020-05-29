using UnityEngine;

[System.Serializable]
public class Sounds
{
    public AudioClip audioClip;
    [Range(0f, 1f)]
    public float audioVolume = 0.5f;
    public bool isLoop;
    [HideInInspector]
    public AudioSource audioSource;
}
