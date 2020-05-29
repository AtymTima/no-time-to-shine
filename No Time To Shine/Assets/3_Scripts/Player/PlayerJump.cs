using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] JumpsObject currentJumpMod;
    [SerializeField] GameObject feet;
    Rigidbody2D myRigidbody2D;

    bool isJumping;

    private void Awake()
    {
        PlayerInput.onJumpPressed += Jump;

        myRigidbody2D = GetComponent<Rigidbody2D>();
        currentJumpMod.GetJumpComponents(myRigidbody2D, feet);
    }

    private void OnDestroy()
    {
        PlayerInput.onJumpPressed -= Jump;
    }

    private void Jump(bool jumpIsPressed)
    {
        isJumping = jumpIsPressed;
    }

    private void FixedUpdate()
    {
        currentJumpMod.Jump(this, isJumping);
    }
}
