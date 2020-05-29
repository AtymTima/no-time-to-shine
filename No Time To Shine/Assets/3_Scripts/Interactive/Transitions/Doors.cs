using UnityEngine;
using System;

public class Doors : MonoBehaviour
{
    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] SoundManager soundManager;
    [SerializeField] float invokeDelay = 3f;
    [SerializeField] float slowTimeScale = 0.1f;

    public static Action<GameObject> OnEnterExit = delegate { };
    public static Action OnExitInput = delegate { };
    private string InvokeString = "InvokeOnExit";

    private void Awake()
    {
        EnemyHealthBar.onLoadWinScreen += SlowDownTime;
    }

    private void OnDestroy()
    {
        EnemyHealthBar.onLoadWinScreen -= SlowDownTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            SlowDownTime();
        }
    }

    private void SlowDownTime()
    {
        Time.timeScale = slowTimeScale;
        soundManager.StopAllSounds();
        soundManager.LevelComplete();
        OnExitInput?.Invoke();
        Invoke(InvokeString, slowTimeScale * invokeDelay);
    }

    private void InvokeOnExit()
    {
        Time.timeScale = 1;
        OnEnterExit?.Invoke(gameObject);
    }
}
