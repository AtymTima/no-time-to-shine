using UnityEngine;
using System.Collections;

public class Animations : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float fixedLandingTime = 0.2f;
    [SerializeField] SoundManager soundManager;
    private bool currentModeIsDark = true;
    private bool currentlyJumping;
    private bool currentlyRunning;

    public delegate void OnChangeMode();
    public static event OnChangeMode onChangeMode = delegate { };
    public delegate void OnLandingAnim(float waitForSeconds, bool animStateIsDark, bool stopNow);
    public static event OnLandingAnim onLandingAnim = delegate { };

    private void Awake()
    {
        PlayerInput.onLightSwitch += ChangeModeAnimation;
        JumpSimple.OnGroundLanding += JumpAnimation;
        Player.OnRunning += RunAnimation;
        UpdateManager.onAnimationEnd += ChangeLandingAnimation;
        Button.onButtonHover += ChangeIdleAnimation;
    }

    private void OnDestroy()
    {
        PlayerInput.onLightSwitch -= ChangeModeAnimation;
        JumpSimple.OnGroundLanding -= JumpAnimation;
        Player.OnRunning -= RunAnimation;
        UpdateManager.onAnimationEnd -= ChangeLandingAnimation;
        Button.onButtonHover -= ChangeIdleAnimation;
    }

    private void ChangeModeAnimation(bool isDarkMode)
    {
        onChangeMode?.Invoke();
        SubstitueAnimation();

        switch (isDarkMode)
        {
            case true:
                animator.SetBool("isLightMode", false);
                currentModeIsDark = true;
                break;
            case false:
                animator.SetBool("isLightMode", true);
                currentModeIsDark = false;
                break;
        }
    }

    private void SubstitueAnimation()
    {
        if (animator.GetBool("isDarkJump"))
        {
            animator.SetBool("isDarkJump", false);
            animator.SetBool("isLightJump", true);
            return;
        }

        if (animator.GetBool("isLightJump"))
        {
            animator.SetBool("isLightJump", false);
            animator.SetBool("isDarkJump", true);
            return;
        }

        if (animator.GetBool("isDarkRun"))
        {
            animator.SetBool("isDarkRun", false);
            animator.SetBool("isLightRun", true);
            return;
        }

        if (animator.GetBool("isLightRun"))
        {
            animator.SetBool("isLightRun", false);
            animator.SetBool("isDarkRun", true);
            return;
        }

        if (animator.GetBool("isDarkLanding"))
        {
            animator.SetBool("isDarkLanding", false);
            animator.SetBool("isLightLanding", true);
            return;
        }

        if (animator.GetBool("isLightLanding"))
        {
            animator.SetBool("isLightLanding", false);
            animator.SetBool("isDarkLanding", true);
            return;
        }
    }

    private void RunAnimation(bool isRunning)
    {
        if (!currentlyJumping)
        {
            if (currentModeIsDark)
            {
                animator.SetBool("isDarkRun", isRunning);
            }
            else
            {
                animator.SetBool("isLightRun", isRunning);
            }
        }
        else
        {
            animator.SetBool("isDarkRun", false);
            animator.SetBool("isLightRun", false);
        }

    }

    private void JumpAnimation(bool isLanded)
    {
        onLandingAnim?.Invoke(0, true, true);

        if (currentModeIsDark)
        {
            if (!isLanded)
            {
                animator.SetBool("isDarkLanding", false);
                animator.SetBool("isDarkJump", true);
            }
            else
            {
                animator.SetBool("isDarkJump", false);
                animator.SetBool("isDarkLanding", true);
                onLandingAnim?.Invoke(fixedLandingTime, true, false);
            }
        }
        else
        {
            if (!isLanded)
            {
                animator.SetBool("isLightLanding", false);
                animator.SetBool("isLightJump", true);
            }
            else
            {
                animator.SetBool("isLightJump", false);
                animator.SetBool("isLightLanding", true);
                onLandingAnim?.Invoke(fixedLandingTime, false, false);
            }
        }
        if (!isLanded)
        {
            soundManager.JumpSFX();
        }
        else
        {
            soundManager.LandingSFX();
        }

        currentlyJumping = !isLanded;
    }

    private void ChangeLandingAnimation(bool animStateIsDark)
    {
        switch (animStateIsDark)
        {
            case true:
                animator.SetBool("isDarkLanding", false);
                break;
            case false:
                animator.SetBool("isLightLanding", false);
                break;
        }
    }

    private void ChangeIdleAnimation(bool isDarkMode)
    {
        animator.SetBool("isLightMode", !isDarkMode);
    }
}
