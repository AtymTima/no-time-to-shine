using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    float currentTime;

    float platformCollWaitTime;
    float platformCollStartTime;
    bool platformCollStop = true;

    float jumpParticlesWaitTime;
    float jumpParticlesStartTime;
    bool jumpParticlesStop = true;

    float animationWaitTime;
    float animationStartTime;
    bool animationStop = true;
    bool currentAnimState;

    bool fadeOutStarted;
    float maxVolume;
    float fadeOutTime;

    public delegate void OnPlatformCollEnd(bool alwaysFalse);
    public static event OnPlatformCollEnd onPlatformCollEnd = delegate { };

    public delegate void OnJumpParticlesEnd();
    public static event OnJumpParticlesEnd onJumpParticlesEnd = delegate { };

    public delegate void OnAnimationEnd(bool currentAnimState);
    public static event OnAnimationEnd onAnimationEnd = delegate { };

    AudioSource currentAudioSource;

    private void Awake()
    {
        PlatformCollision.onTriggerCoroutine += PlatformCollisionState;
        JumpParticles.onParticlesBegin += JumpParticlesState;
        Animations.onLandingAnim += AnimationState;
        SoundManager.onFadeOut += FadeOutAudio;
    }

    private void Start()
    {
        Application.targetFrameRate = 30;
    }

    private void OnDestroy()
    {
        PlatformCollision.onTriggerCoroutine -= PlatformCollisionState;
        JumpParticles.onParticlesBegin -= JumpParticlesState;
        Animations.onLandingAnim -= AnimationState;
        SoundManager.onFadeOut -= FadeOutAudio;

    }

    private void Update()
    {
        currentTime = Time.time;

        if (!platformCollStop && currentTime - platformCollStartTime > platformCollWaitTime)
        {
            platformCollStop = true;
            onPlatformCollEnd?.Invoke(false);
        }

        if (!jumpParticlesStop && (currentTime - jumpParticlesStartTime > jumpParticlesWaitTime))
        {
            jumpParticlesStop = true;
            onJumpParticlesEnd?.Invoke();
        }

        if (!animationStop && currentTime - animationStartTime > animationWaitTime)
        {
            animationStop = true;
            onAnimationEnd?.Invoke(currentAnimState);
        }

        if (fadeOutStarted)
        {
            while (currentAudioSource.volume > 0)
            {
                currentAudioSource.volume -= maxVolume * Time.deltaTime / fadeOutTime;
                return;
            }
            fadeOutStarted = false;
            currentAudioSource.Stop();
        }
    }

    private void PlatformCollisionState(float waitForSeconds, bool stopNow)
    {
        platformCollStop = stopNow;
        if (stopNow) { return; }
        platformCollWaitTime = waitForSeconds;
        platformCollStartTime = currentTime;
    }

    private void JumpParticlesState(float waitForSeconds, bool stopNow)
    {
        jumpParticlesStop = stopNow;
        if (stopNow) { return; }
        jumpParticlesWaitTime = waitForSeconds;
        jumpParticlesStartTime = currentTime;
    }

    private void AnimationState(float waitForSeconds, bool animStateIsDark, bool stopNow)
    {
        animationStop = stopNow;
        if (stopNow) { return; }
        animationWaitTime = waitForSeconds;
        animationStartTime = currentTime;
        currentAnimState = animStateIsDark;
    }

    private void FadeOutAudio(AudioSource audioSource, float initialVolume, float timeFadeOut)
    {
        fadeOutStarted = true;
        fadeOutTime = timeFadeOut;
        maxVolume = initialVolume;
        currentAudioSource = audioSource;
    }
}
