using UnityEngine;

public class InteractiveObjects : MonoBehaviour
{
    [SerializeField] GameObject[] interactiveObjects;
    [SerializeField] Rigidbody2D[] rigidbodies2D;
    [SerializeField] SpriteRenderer[] spriteRenderers;
    [SerializeField] Collider2D[] colliders2D;

    SpriteMaskInteraction spriteMaskInteraction;
    RigidbodyType2D currentRbType;

    bool currentLight;

    private void Awake()
    {
        for (int i = 0; i < rigidbodies2D.Length; i++)
        {
            rigidbodies2D[i].bodyType = RigidbodyType2D.Static;
        }

        ChangeSpriteVisibility(true);
    }

    private void OnEnable()
    {
        PlayerInput.onLightSwitch += ChangeSpriteVisibility;
    }

    private void OnDisable()
    {
        PlayerInput.onLightSwitch -= ChangeSpriteVisibility;
    }

    private void ChangeSpriteVisibility(bool isLightOn)
    {
        switch (isLightOn)
        {
            case false:
                spriteMaskInteraction = SpriteMaskInteraction.None;
                break;
            case true:
                spriteMaskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                break;
        }

        if (interactiveObjects == null) { return; }

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].maskInteraction = spriteMaskInteraction;
        }

        currentLight = isLightOn;
    }
}
