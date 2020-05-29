using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] Rigidbody2D myRigidBody2D;
    [SerializeField] float fallSpeed = 1f;
    [SerializeField] bool positiveDirection = true;
    [SerializeField] Platform currentPlatform;

    RigidbodyType2D currentRbType;
    SpriteRenderer[] spritesOfChildren;

    SpriteMaskInteraction spriteMaskInteraction;
    bool currentLight = true;
    bool isTouching;

    Vector2 currentPos;
    Vector2 targetPos;

    private void Awake()
    {
        PlayerInput.onLightSwitch += ChangeSpriteVisibility;
        PlatformCollision.onBubbleCollision += TouchesBubble;
        PlatformCollision.onWallCollided += ChangeDirection;

        currentRbType = RigidbodyType2D.Static;
        myRigidBody2D.bodyType = currentRbType;

        spritesOfChildren = GetComponentsInChildren<SpriteRenderer>();
        currentPlatform.SetPlatformSpeed(fallSpeed, positiveDirection, gameObject);
    }

    private void OnDestroy()
    {
        PlayerInput.onLightSwitch -= ChangeSpriteVisibility;
        PlatformCollision.onBubbleCollision -= TouchesBubble;
        PlatformCollision.onWallCollided -= ChangeDirection;

    }

    private void Update()
    {
        if (!currentLight || isTouching)
        {
            currentPlatform.MovePlatform(gameObject, positiveDirection);
        }
    }

    private void ChangeSpriteVisibility(bool isLightOn)
    {
        switch (isLightOn)
        {
            case false:
                currentRbType = RigidbodyType2D.Kinematic;
                spriteMaskInteraction = SpriteMaskInteraction.None;

                break;
            case true:
                currentRbType = RigidbodyType2D.Static;
                spriteMaskInteraction = SpriteMaskInteraction.VisibleInsideMask;

                break;
        }
        currentLight = isLightOn;
        ChangeCurrentState();
    }

    private void TouchesBubble(bool bubbleCollision, Transform platformTransform)
    {
        if (platformTransform.position != transform.position) { return; }
        isTouching = bubbleCollision;
        if (!currentLight) { bubbleCollision = true; }
        switch (bubbleCollision)
        {
            case true:
                currentRbType = RigidbodyType2D.Kinematic;
                spriteMaskInteraction = SpriteMaskInteraction.None;
                break;
            case false:
                currentRbType = RigidbodyType2D.Static;
                spriteMaskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                break;
        }
        ChangeCurrentState();
    }

    private void ChangeCurrentState()
    {
        for (int i = 0; i < spritesOfChildren.Length; i++)
        {
            spritesOfChildren[i].maskInteraction = spriteMaskInteraction;
        }
        myRigidBody2D.bodyType = currentRbType;
    }

    private void ChangeDirection(Transform myTransform, int platformIndex)
    {
        if (myTransform != gameObject.transform) { return; }
        positiveDirection = !positiveDirection;
    }

}
