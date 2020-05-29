using UnityEngine;

public class Chains : MonoBehaviour
{
    [SerializeField] bool nonVisible;
    [SerializeField] bool canBeFrozen;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Rigidbody2D myRigidBody2D;

    private void Awake()
    {
        if (nonVisible && spriteRenderer != null)
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            PlayerInput.onLightSwitch += ChangeChainMask;
        }
    }

    private void OnDestroy()
    {
        if (nonVisible && spriteRenderer != null)
        {
            PlayerInput.onLightSwitch -= ChangeChainMask;
        }
    }

    private void ChangeChainMask(bool isLightOn)
    {
        if (!nonVisible) { return; }
        switch (isLightOn)
        {
            case false:
                spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
                break;
            case true:
                spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                break;
        }
    }

}
