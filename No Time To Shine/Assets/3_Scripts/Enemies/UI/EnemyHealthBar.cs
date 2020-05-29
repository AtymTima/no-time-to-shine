using UnityEngine.UI;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    public delegate void OnBossDeath();
    public static event OnBossDeath onBossDeath = delegate { };
    public delegate void OnLoadWinScreen();
    public static event OnLoadWinScreen onLoadWinScreen = delegate { };

    [SerializeField] private Image candleBarFiller;
    [SerializeField] private RectTransform fireFlameRectTransform;
    [SerializeField] private RectTransform candleRectTransform;
    [SerializeField] private SoundManager soundManager;

    [SerializeField] private BlackHole blackHole;
    private Vector2 fireFlamePosition;
    private float candleBarWidth;

    private float maxHealth;
    private float normalizedHealth;
    private bool gameIsOver;

    private bool isSecondStage;
    private bool isThirdStage;
    private bool isLastStage;


    private void Awake()
    {
        maxHealth = blackHole.GetMaxHealth();
        candleBarWidth = candleRectTransform.rect.width;
        CandleTimer.OnCandleTimerStop += GameIsOver;
    }

    private void OnDestroy()
    {
        CandleTimer.OnCandleTimerStop -= GameIsOver;
    }

    public void UpdateCandleBar(float currentHealth)
    {
        if (gameIsOver) { return; }
        normalizedHealth = GetNormalizedHealth(currentHealth);
        candleBarFiller.fillAmount = normalizedHealth;

        fireFlamePosition.x = normalizedHealth * candleBarWidth - 185f;
        fireFlameRectTransform.anchoredPosition = fireFlamePosition;


        if (!isSecondStage && normalizedHealth <= 0.75)
        {
            isSecondStage = true;
            blackHole.PrepareForSecondPhase();
        }
        if (!isThirdStage && normalizedHealth <= 0.5)
        {
            isThirdStage = true;
            blackHole.PrepareForSecondPhase();
        }
        if (!isLastStage && normalizedHealth <= 0.25)
        {
            isLastStage = true;
            blackHole.PrepareForSecondPhase();
        }

        if (currentHealth <= 0)
        {
            onBossDeath?.Invoke();
            gameIsOver = !gameIsOver;
            fireFlameRectTransform.gameObject.SetActive(false);
            candleRectTransform.gameObject.SetActive(false);
            onLoadWinScreen?.Invoke();
        }
    }

    private float GetNormalizedHealth(float currentHealth)
    {
        return currentHealth / maxHealth;
    }

    private void GameIsOver()
    {
        gameIsOver = true;
    }

}
