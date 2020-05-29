using UnityEngine;
using System;
using UnityEngine.UI;

public class CandleTimer : MonoBehaviour
{
    [SerializeField] private Image candleTimerFiller;
    [SerializeField] private float timeLimitOfLevel = 5f;
    [SerializeField] private float damagePerCollision = 0.05f;

    [SerializeField] private RectTransform fireFlameRectTransform;
    [SerializeField] private RectTransform candleRectTransform;
    [SerializeField] private SoundManager soundManager;

    private Vector2 fireFlamePosition;
    private float candleBarWidth;
    private float timeRemains;
    private float normalizedTime;
    private bool currentLight;
    private bool timerIsOver;
    private bool isCollisionWithEnemy;

    public static Action OnCandleTimerStop = delegate { };

    private void Awake()
    {
        timeRemains = timeLimitOfLevel;
        candleBarWidth = candleRectTransform.rect.width;
    }

    private void OnEnable()
    {
        PlayerInput.onLightSwitch += ChangeTimerState;
        CollectableTorch.onTorchCollected += RestoreTimeToMax;
    }

    private void OnDisable()
    {
        PlayerInput.onLightSwitch -= ChangeTimerState;
        CollectableTorch.onTorchCollected -= RestoreTimeToMax;
    }

    private void Update()
    {
        if (currentLight && !timerIsOver)
        {
            StartCandleTimer(false);
        }

        if (isCollisionWithEnemy && !timerIsOver)
        {
            StartCandleTimer(true);
        }
    }

    private void StartCandleTimer(bool isCollision)
    {
        timeRemains -= isCollision ? damagePerCollision : Time.deltaTime;
        UpdateCandleTimer();
    }

    private void RestoreTimeToMax(int spawnIndex)
    {
        timeRemains = timeLimitOfLevel;
        UpdateCandleTimer();
    }

    private void UpdateCandleTimer()
    {
        normalizedTime = NormalizeBar();
        candleTimerFiller.fillAmount = normalizedTime;

        fireFlamePosition.x = normalizedTime * candleBarWidth - 185f;
        fireFlameRectTransform.anchoredPosition = fireFlamePosition;

        if (timeRemains <= 0)
        {
            OnCandleTimerStop?.Invoke();
            timerIsOver = true;
            fireFlameRectTransform.gameObject.SetActive(false);
            candleRectTransform.gameObject.SetActive(false);
        }
    }

    private void ChangeTimerState(bool isLightOn)
    {
        currentLight = !isLightOn;
        if (currentLight)
        {
            soundManager.CandelTimerStartSFX(false);
        }
        else
        {
            soundManager.CandelTimerStopSFX(true);
        }
    }

    private float NormalizeBar()
    {
        return timeRemains / timeLimitOfLevel;
    }

    public void ReduceTime(bool isCollided)
    {
        if (timerIsOver) { return; }
        isCollisionWithEnemy = isCollided;
        timeRemains -= damagePerCollision;
        UpdateCandleTimer();
    }
}
