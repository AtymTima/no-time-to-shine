using UnityEngine;

public class BlackHoleCollision : MonoBehaviour
{
    [Header("Overlap Circle")]
    [SerializeField] private Transform blackHoleCenter;
    [SerializeField] private float centerRadius = 1.5f;
    [SerializeField] private LayerMask whatIsBubble;

    [Header("Reference")]
    [SerializeField] private BlackHole blackHole;
    [SerializeField] private CandleTimer candleTimer;
    [SerializeField] private SoundManager soundManager;
    private bool isCollided;
    private bool isSoundChanged;

    public void IsBubbleCollided()
    {
        isCollided = Physics2D.OverlapCircle(blackHoleCenter.position, centerRadius, whatIsBubble);
        if (isCollided)
        {
            blackHole.ReduceHealth();
            if (!isSoundChanged)
            {
                blackHole.ChangeSprite(true);
                soundManager.EnemyHitSFX(false);
                isSoundChanged = true;
            }
        }
        else
        {
            if (isSoundChanged)
            {
                blackHole.ChangeSprite(false);
                soundManager.EnemyHitSFX(true);
                isSoundChanged = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            candleTimer?.ReduceTime(true);
            soundManager.PlayerHitSFX(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            candleTimer?.ReduceTime(false);
            soundManager.PlayerHitSFX(true);
        }
    }
}
