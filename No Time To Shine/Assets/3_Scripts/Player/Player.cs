using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [Header("Movement Params")]
    [SerializeField] MovementObject movementObject;
    private float movementDirection;

    [Header("Sprite Rendered")]
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] SoundManager soundManager;
    public static Action<bool> OnRunning = delegate { };

    bool isRunning;

    private void Update()
    {
        movementDirection = Input.GetAxis("Horizontal");
        FlipCharacter();
        CheckIfRunning();
    }

    private void FixedUpdate()
    {
        movementObject.Move(this, movementDirection);
    }

    private void CheckIfRunning()
    {
        if (!isRunning && Mathf.Abs(movementDirection) > Mathf.Epsilon)
        {
            OnRunning?.Invoke(true);
            isRunning = true;
            soundManager.RunningSFX(false);
        }
        else if (isRunning && Mathf.Abs(movementDirection) < Mathf.Epsilon)
        {
            OnRunning?.Invoke(false);
            isRunning = false;
            soundManager.RunningSFX(true);
        }
    }

    private void FlipCharacter()
    {
        if (movementDirection > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (movementDirection < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}
