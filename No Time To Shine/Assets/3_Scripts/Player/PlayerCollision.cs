using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] SoundManager soundManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Chains"))
        {
            soundManager.ChainsSFX();
        }
    }
}
