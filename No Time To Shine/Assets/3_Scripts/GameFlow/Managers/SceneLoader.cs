using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName ="My Managers/Scene Manager")]
public class SceneLoader : ScriptableObject
{
    public delegate void OnLoadNextLevel();
    public static event OnLoadNextLevel onLoadNextLevel = delegate { };

    //public delegate void OnFinalBossLevel();
    //public static event OnFinalBossLevel onFinalBossLevel = delegate { };

    private void OnEnable()
    {
        Doors.OnEnterExit += LoadNextLevel;
        PlayerInput.onRestartPressed += RestartLevel;
        Button.onGameStarted += LoadTheFirstLevel;
    }

    private void OnDisable()
    {
        Doors.OnEnterExit -= LoadNextLevel;
        PlayerInput.onRestartPressed -= RestartLevel;
        Button.onGameStarted -= LoadTheFirstLevel;
    }

    private void LoadNextLevel(GameObject currentExit)
    {
        onLoadNextLevel?.Invoke();
        SceneManager.LoadSceneAsync(currentExit.scene.buildIndex + 1);
    }

    private void LoadTheFirstLevel(GameObject currentExit)
    {
        onLoadNextLevel?.Invoke();
        SceneManager.LoadSceneAsync(1);
    }

    public void RestartLevel(GameObject currentExit)
    {
        SceneManager.LoadSceneAsync(currentExit.scene.buildIndex);
    }

    //public bool StartWithBossTheme(GameObject currentExit)
    //{
    //    if (currentExit.scene.buildIndex == 14)
    //    {
    //        return true;
    //    }
    //    return false;
    //}
}
