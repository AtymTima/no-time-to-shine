using UnityEngine;

public class MyAudioManager : MonoBehaviour
{
    [Header("Source")]
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private AudioSource musicSource;

    [Header("Music Clips")]
    [SerializeField] private AudioClip bossFinalTheme;
    [SerializeField] private AudioClip mainMenuTheme;
    [SerializeField] private AudioClip levelsTheme;

    [Header("Scene Transitions")]
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private bool isStartMenu;
    [SerializeField] private bool isBossLevel;

    public static MyAudioManager AudioInstance;
    private static bool isRestarted;
    private AudioClip currentAudioClip;

    private void Awake()
    {
        if (isRestarted) { RestartSingleton(); }
        SetUpSingleton();
        StopAllSounds();
        soundManager.BubbleLightSuddenRemoveSFX();
        SceneLoader.onLoadNextLevel += RestartAudio;

        currentAudioClip = levelsTheme;
        StartMenuMusic();
        StartBossTheme();
        musicSource.clip = currentAudioClip;
        musicSource.Play();
    }

    private void OnDestroy()
    {
        SceneLoader.onLoadNextLevel -= RestartAudio;
    }

    private void SetUpSingleton()
    {
        if (AudioInstance == null)
        {
            AudioInstance = this;
        }
        else
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void RestartSingleton()
    {
        AudioInstance.gameObject.SetActive(false);
        Destroy(AudioInstance.gameObject);
        AudioInstance = null;
        isRestarted = false;
    }

    private void RestartAudio()
    {
        isRestarted = true;
    }

    private void StopAllSounds()
    {
        soundManager.StopAllSounds();
    }

    public AudioSource GetAudioSource()
    {
        return gameObject.AddComponent<AudioSource>();
    }

    public AudioSource GetMusicSource()
    {
        return musicSource;
    }

    private void StartMenuMusic()
    {
        if (isStartMenu)
        {
            currentAudioClip = mainMenuTheme;
        }
    }

    private void StartBossTheme()
    {
        if (isBossLevel)
        {
            currentAudioClip = bossFinalTheme;
        }
    }
}
