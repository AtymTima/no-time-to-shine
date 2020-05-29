using UnityEngine;

[CreateAssetMenu(menuName ="My Managers/Audio Manager")]
public class SoundManager : ScriptableObject
{
    public delegate void OnFadeOut(AudioSource audioSource, float initialVolume, float timeFadeOut);
    public static event OnFadeOut onFadeOut = delegate { };

    [SerializeField] Sounds[] sounds;
    private AudioSource[] audioSources = new AudioSource[13];
    private float initialPlayerVolume;
    private float initialEnemyVolume;

    public void CandelTimerStartSFX(bool stop)
    {
        PlayAudio(0, false);
        PlayAudio(1, stop);
    }

    public void CandelTimerStopSFX(bool stop)
    {
        PlayAudio(1, stop);
        PlayAudio(2, false);
    }

    public void BubbleLightStartSFX(bool stop)
    {
        PlayAudio(3, stop);
    }

    public void BubbleLightStopSFX(bool stop)
    {
        PlayAudio(4, stop);
    }

    public void BubbleLightSuddenRemoveSFX()
    {
        PlayAudio(5, false);
    }

    public void JumpSFX()
    {
        PlayAudio(6, false);
    }

    public void LandingSFX()
    {
        PlayAudio(7, false);
    }

    public void RunningSFX(bool stop)
    {
        PlayAudio(8, stop);
    }

    public void ChainsSFX()
    {
        PlayAudio(9, false);
    }

    public void LevelComplete()
    {
        MyAudioManager.AudioInstance.GetMusicSource().Stop();
        PlayAudio(10, false);
    }

    public void PlayerHitSFX(bool stop)
    {
        FadeInOrFadeOutSFX(stop, 11, initialPlayerVolume);
    }

    public void EnemyHitSFX(bool stop)
    {
        FadeInOrFadeOutSFX(stop, 12, initialEnemyVolume);
    }

    private void FadeInOrFadeOutSFX(bool stop, int sfxIndex, float initialVolume)
    {
        if (!stop)
        {
            PlayAudio(sfxIndex, stop);
            if (initialVolume <= Mathf.Epsilon) { return; }
            sounds[sfxIndex].audioSource.volume = sounds[sfxIndex].audioVolume;
        }
        else
        {
            if (sfxIndex == 11)
            {
                initialPlayerVolume = sounds[sfxIndex].audioVolume;
            }
            else if (sfxIndex == 12)
            {
                initialEnemyVolume = sounds[sfxIndex].audioVolume;
            }
            onFadeOut?.Invoke(sounds[sfxIndex].audioSource, initialVolume, 1.5f);
        }
    }

    private void PlayAudio(int audioIndex, bool stop)
    {
        if (audioSources[audioIndex] == null)
        {
            audioSources[audioIndex] = MyAudioManager.AudioInstance.GetAudioSource();
            sounds[audioIndex].audioSource = audioSources[audioIndex];
            sounds[audioIndex].audioSource.clip = sounds[audioIndex].audioClip;
            sounds[audioIndex].audioSource.volume = sounds[audioIndex].audioVolume;
            sounds[audioIndex].audioSource.loop = sounds[audioIndex].isLoop;
        }

        if (stop)
        {
            sounds[audioIndex].audioSource.Stop();
            return;
        }

        sounds[audioIndex].audioSource.Play();
    }

    public void StopAllSounds()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].audioSource != null)
            {
                sounds[i].audioSource.Stop();
            }
        }
    }
}
