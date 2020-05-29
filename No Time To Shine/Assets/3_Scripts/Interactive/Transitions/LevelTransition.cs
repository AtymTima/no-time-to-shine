using Unity.Collections;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    private static LevelTransition levelTransitionInstance;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Animator animator;
    private static bool isNextLevel;

    private void Awake()
    {
        if (isNextLevel) { RestartSingleton(); }
        SetUpSingleton();
        ShowLevelScreen();
        SceneLoader.onLoadNextLevel += NextTransitionText;
    }

    private void OnDestroy()
    {
        SceneLoader.onLoadNextLevel -= NextTransitionText;
    }

    private void SetUpSingleton()
    {
        if (levelTransitionInstance == null)
        {
            levelTransitionInstance = this;
        }
        else
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void ShowLevelScreen()
    {
        animator.SetBool("isNextLevel", isNextLevel);
        isNextLevel = false;
    }

    private void RestartSingleton()
    {
        levelTransitionInstance.gameObject.SetActive(false);
        Destroy(levelTransitionInstance.gameObject);
        levelTransitionInstance = null;
        isNextLevel = false;
    }

    private void NextTransitionText()
    {
        isNextLevel = true;
    }
}
