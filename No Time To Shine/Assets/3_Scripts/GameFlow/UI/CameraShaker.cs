using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private float shakePower = 0.15f;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private bool isShaking;
    [SerializeField] private float shakeSlowDownRate = 1f;

    private Vector3 startPosition;
    private float hugeShakeSlowDownRate = 0.25f;
    private float currentSlowDownRate;
    private float initialDuration;
    private bool isBossDefeated;

    private void Awake()
    {
        Animations.onChangeMode += StartShaking;
        EnemyHealthBar.onBossDeath += BossDestroyedShake;
    }

    private void Start()
    {
        startPosition = mainCamera.localPosition;
        initialDuration = shakeDuration;
    }

    private void OnDestroy()
    {
        Animations.onChangeMode -= StartShaking;
        EnemyHealthBar.onBossDeath -= BossDestroyedShake;
    }

    private void Update()
    {
        if (isBossDefeated)
        {
            currentSlowDownRate = hugeShakeSlowDownRate;
            ShakeCamera(shakePower * 5, shakeDuration * 10);
            return;
        }

        if (isShaking)
        {
            currentSlowDownRate = shakeSlowDownRate;
            ShakeCamera(shakePower, shakeDuration);
        }
    }

    private void ShakeCamera(float shakeMagnitude, float currentDuration)
    {
        if (shakeDuration > 0)
        {
            mainCamera.localPosition = startPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * currentSlowDownRate;
        }
        else
        {
            isBossDefeated = false;
            isShaking = false;
            shakeDuration = initialDuration;
            mainCamera.localPosition = startPosition;
        }
    }

    private void StartShaking()
    {
        isShaking = true;
    }

    public void BossDestroyedShake()
    {
        isBossDefeated = true;
    }
}
