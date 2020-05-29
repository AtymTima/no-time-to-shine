using UnityEngine;

public class Interactive : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool isInvisible;

    private void Awake()
    {
        if (isInvisible)
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
    }
}
