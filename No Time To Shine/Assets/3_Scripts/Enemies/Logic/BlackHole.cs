using UnityEngine;
using TMPro;

public class BlackHole : MonoBehaviour
{
    public delegate void OnSecondPhase(bool isLightOn);
    public static event OnSecondPhase onSecondPhase = delegate { };

    [Header("Movement")]
    [SerializeField] MovementAI[] movementsAI;
    [SerializeField] Transform targetPlayer;

    [Header("Components")]
    [SerializeField] Rigidbody2D myRigidBody2D;
    [SerializeField] SpriteRenderer[] collectiveSprite;
    [SerializeField] CircleCollider2D circleCollider2D;

    [Header("Reference")]
    [SerializeField] BlackHoleCollision blackHoleCollision;
    [SerializeField] EnemyHealthBar enemyHealthBar;
    [SerializeField] SoundManager soundManager;
    [SerializeField] TorchSpawner torchSpawner;
    [SerializeField] TextMeshProUGUI torchesCollectedText;

    [Header("Stats")]
    [SerializeField] float maxHealthPoints = 1000;
    [SerializeField] float damagePerCollision = 0.1f;
    [SerializeField] float timeBeforeChangingState = 1.5f;

    ObjectPool<ExplosionLifeTime> explosionPool;
    ExplosionLifeTime explosionParticles;
    MovementAI currentMovement;

    Color32 hitColor = new Color32(177, 179, 67, 255);
    Color32 originalColor = new Color32(255, 255, 255, 255);
    Color32 currentColor;

    Vector3 originalScale;
    Transform currentTarget;
    float currentHealth;
    float currentTimeBeforeNewState;

    bool currentLight;
    bool isDefendingState;
    bool isSecondPhase;
    int numberOfTorchCollected;

    string initialTorchesText = "0 / 5";

    private void Awake()
    {
        myRigidBody2D.bodyType = RigidbodyType2D.Static;
        circleCollider2D.isTrigger = true;
        ChangeCurrentState(true);
        currentHealth = maxHealthPoints;
        currentMovement = movementsAI[0];
        currentMovement.SetInitialParams(gameObject);
        currentTarget = targetPlayer;
        originalScale = gameObject.transform.localScale;

        PlayerInput.onLightSwitch += ChangeCurrentState;
        EnemyHealthBar.onBossDeath += DestroyBlackHole;
        CollectableTorch.onTorchCollected += TorchIsCollected;
    }

    private void OnDestroy()
    {
        PlayerInput.onLightSwitch -= ChangeCurrentState;
        EnemyHealthBar.onBossDeath -= DestroyBlackHole;
        CollectableTorch.onTorchCollected -= TorchIsCollected;
    }

    private void Start()
    {
        explosionPool = ObjectPool<ExplosionLifeTime>.Instance;
        explosionPool.AddObjects(1);
    }

    private void Update()
    {
        currentMovement.MoveAI(gameObject, currentTarget, currentLight);
        if (currentLight)
        {
            blackHoleCollision.IsBubbleCollided();
        }
        if (isSecondPhase)
        {
            TimerBeforeNewState();
        }
    }

    private void ChangeCurrentState(bool isLightOn)
    {
        currentLight = isLightOn;
        if (!isLightOn && currentColor.b == hitColor.b)
        {
            for (int i = 0; i < collectiveSprite.Length; i++)
            {
                collectiveSprite[i].color = originalColor;
            }
        }
    }

    public void ReduceHealth()
    {
        currentHealth -= damagePerCollision;
        enemyHealthBar.UpdateCandleBar(currentHealth);
    }

    public float GetMaxHealth()
    {
        return maxHealthPoints;
    }

    public void ChangeSprite(bool isHit)
    {
        currentColor = isHit ? hitColor : originalColor;
        for (int i=0; i < collectiveSprite.Length; i++)
        {
            collectiveSprite[i].color = currentColor;
        }
    }

    private void DestroyBlackHole()
    {
        SummonExplosion();
    }

    public void PrepareForSecondPhase()
    {
        isSecondPhase = true;
        currentTimeBeforeNewState = timeBeforeChangingState;
        SummonParticles();
    }

    private void SummonExplosion()
    {
        soundManager.PlayerHitSFX(true);
        soundManager.EnemyHitSFX(true);
        SummonParticles();
        gameObject.SetActive(false);
    }

    private void SummonParticles()
    {
        explosionParticles = explosionPool.Get();
        explosionParticles.transform.localPosition = gameObject.transform.localPosition;
        explosionParticles.gameObject.SetActive(true);
    }

    private void TimerBeforeNewState()
    {
        if (currentTimeBeforeNewState >= Mathf.Epsilon)
        {
            currentTimeBeforeNewState -= Time.deltaTime;
        }
        else
        {
            currentTarget = torchSpawner.GetCurrentTorch().transform;
            SummonParticles();
            isSecondPhase = false;
            onSecondPhase?.Invoke(false);
            ChangeState();

            torchesCollectedText.text = initialTorchesText;
            torchesCollectedText.gameObject.SetActive(true);
        }
    }

    private void ChangeState()
    {
        currentMovement = isDefendingState ? movementsAI[0] : movementsAI[1];
        isDefendingState = !isDefendingState;
    }

    private void TorchIsCollected(int previousIndex)
    {
        if (isDefendingState)
        {
            numberOfTorchCollected++;
            if (numberOfTorchCollected == 5)
            {
                currentTarget = targetPlayer;
                gameObject.transform.localScale = originalScale;
                SummonParticles();
                onSecondPhase?.Invoke(true);
                numberOfTorchCollected = 0;
                ChangeState();
                currentMovement.SetInitialParams(gameObject);
                soundManager.CandelTimerStartSFX(true);
                soundManager.CandelTimerStopSFX(false);
                torchesCollectedText.gameObject.SetActive(false);

                return;
            }
            torchesCollectedText.text = string.Format("{0} / 5", numberOfTorchCollected);
            currentMovement.StartExpand(true);
            currentTarget = torchSpawner.GetCurrentTorch().transform;
        }
    }
}
