using UnityEngine;
using System.Collections;

public class PlatformCollision : MonoBehaviour
{

    public delegate void OnBubbleCollision(bool isTouching, Transform transform);
    public static event OnBubbleCollision onBubbleCollision = delegate { };

    public delegate void OnPlatformDeleted(Transform transform, int platformIndex);
    public static event OnPlatformDeleted onPlatformDeleted = delegate { };

    public delegate void OnTriggerCoroutine(float waitForSeconds, bool stopNow);
    public static event OnTriggerCoroutine onTriggerCoroutine = delegate { };

    public delegate void OnWallCollided(Transform transform, int platformIndex);
    public static event OnWallCollided onWallCollided = delegate { };

    ObjectPool<PlatformCollision> objectPool;

    private bool isTriggeredByBubble;
    [SerializeField] private float waitForSeconds = 0.1f;
    [SerializeField] private int platformIndex;

    private void Awake()
    {
        UpdateManager.onPlatformCollEnd += ChangeTriggerConditions;
    }

    private void Start()
    {
        objectPool = ObjectPool<PlatformCollision>.Instance;
    }

    private void OnDestroy()
    {
        UpdateManager.onPlatformCollEnd -= ChangeTriggerConditions;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BubbleLight") && !isTriggeredByBubble)
        {
            ChangeTriggerConditions(true);
        }

        if (collision.gameObject.CompareTag("PlatformShredder"))
        {
            onTriggerCoroutine?.Invoke(0, true);
            if (isTriggeredByBubble)
            {
                ChangeTriggerConditions(false);
            }
            objectPool.ReturnToPool(this);
            onPlatformDeleted?.Invoke(gameObject.transform, platformIndex);
            return;
        }

        if (collision.gameObject.CompareTag("PlatformWall"))
        {
            onWallCollided?.Invoke(gameObject.transform, platformIndex);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BubbleLight"))
        {
            onTriggerCoroutine?.Invoke(waitForSeconds, false);
        }
    }

    private void ChangeTriggerConditions(bool currentState)
    {
        onBubbleCollision?.Invoke(currentState, transform);
        isTriggeredByBubble = currentState;
    }
}
