using System;
using UnityEngine;

[CreateAssetMenu(menuName ="My Player/Jumps/Standard")]
public class JumpSimple : JumpsObject
{
    [Header("Player Related")]
    [SerializeField] float jumpPower = 80f;
    [Range(0.1f, 0.5f)] [SerializeField] float timerDelay = 0.2f;
    [SerializeField] float slowGravity = 5f;
    [SerializeField] float mainGravity = 10f;
    [SerializeField] float jumpDelay = 0.15f;

    float timerElapsed; //how long the player can jump higher
    bool canJumpHigher; //jump higher when holding the space

    bool nextJump; // to prevent the double jump
    bool hasGravityChanged; //decrease gravity when jumping,
    bool feetInTheAir; // for making the bubble appear

    float lastTimeOnGround;
    bool hasLeftTheGround;

    [Header("Feet Related")]
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float feetRadius = 0.2f;

    GameObject feet;
    Transform feetTransform;
    bool isGrounded;

    Rigidbody2D rb;
    Vector2 jumpVector;

    public static event Action<bool> OnGroundLanding;

    private void OnEnable()
    {
        jumpVector = Vector2.up * jumpPower * Time.fixedDeltaTime;
    }

    public override void GetJumpComponents(Rigidbody2D rb, GameObject feet)
    {
        this.rb = rb;
        this.feet = feet;

        feetTransform = feet.transform;
        timerElapsed = timerDelay;
        canJumpHigher = true;
        nextJump = true;
    }

    public override void Jump(MonoBehaviour monoBehaviour, bool isJumping)
    {
        isGrounded = Physics2D.OverlapCircle(feetTransform.position, feetRadius, whatIsGround);

        GravityConditions(isJumping);
        CheckIfCanJump(isJumping);

        if (!isGrounded && !hasLeftTheGround)
        {
            lastTimeOnGround = Time.time;
            hasLeftTheGround = true;
        }

        if (isGrounded)
        {
            lastTimeOnGround = 0;
            hasLeftTheGround = false;
        }
    }

    private void CheckIfCanJump(bool isJumping)
    {
        if (isJumping && nextJump)
        {
            TimerConditions();
            JumpOnPress();

            if (!feetInTheAir) { OnGroundLanding?.Invoke(false); }
            feetInTheAir = true;

        }
        else
        {
            if (!isGrounded && feetInTheAir)
            {
                nextJump = false;

            }
            else if (isGrounded && feetInTheAir)
            {
                if (feetInTheAir) { OnGroundLanding?.Invoke(true); }
                feetInTheAir = false;
                nextJump = true;
                canJumpHigher = true;
            }

            timerElapsed = timerDelay;
        }
    }

    private void GravityConditions(bool isJumping)
    {
        if (isJumping && !hasGravityChanged)
        {
            rb.gravityScale = slowGravity;
            hasGravityChanged = true;
        }
        else if (!isJumping && hasGravityChanged)
        {
            rb.gravityScale = mainGravity;
            hasGravityChanged = false;
        }
    }

    private void TimerConditions()
    {
        if (canJumpHigher)
        {
            if (timerElapsed > 0)
            {
                timerElapsed -= Time.deltaTime;
            }
            else
            {
                canJumpHigher = false;
                timerElapsed = timerDelay;
                CheckIfCanJump(false);

            }
        }
    }

    private void JumpOnPress()
    {
        if (rb == null) { return; }

        if (canJumpHigher && JumpDelayAfterGround())
        {
            //rb.velocity = Vector2.up * jumpPower * Time.fixedDeltaTime;
            rb.velocity = jumpVector;
        }
    }

    private bool JumpDelayAfterGround()
    {
        if (isGrounded) { return true; }

        if (Time.time - lastTimeOnGround <= jumpDelay || !nextJump)
        {
            return true;
        }
        return false;
    }


}
