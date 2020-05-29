using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine;
using System.Collections;

public class PlayerLight : MonoBehaviour
{
    [Header("Light State")]
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Light2D darkLight;
    [SerializeField] private Light2D exitLight;
    [SerializeField] private Light2D worldBackLight;

    [SerializeField] private float lightLowIntensity = 0.15f;
    private float initialIntensity;
    private bool currentLight = true;

    [Header("Feet Bubble")]
    [SerializeField] private SpriteMask feetBubbleMask;
    [SerializeField] private GameObject feetPos;
    [SerializeField] private Light2D lightInsideBubble;
    [SerializeField] private float feetBubbleLifetime = 1f;
    [SerializeField] private SoundManager soundManager;
    private Vector2 feetVector;
    private IEnumerator feetCoroutine;
    private float initialScale;
    private float currentScale;
    private Vector2 newScale;

    private float radiusChangeSizeTime = 3.5f;
    private float currentInnerRadius;
    private float currentOuterRadius;
    private float maxRadius;
    bool isShrinking;
    WaitForSeconds waitForSeconds;

    [Header("Layer Masks")]
    [SerializeField] TilemapRenderer tilemapRenderer;
    [SerializeField] TilemapRenderer darkmapRenderer;
    [SerializeField] TilemapRenderer doorsRenderer;

    private void Start()
    {
        maxRadius = lightInsideBubble.pointLightOuterRadius;
        waitForSeconds = new WaitForSeconds(feetBubbleLifetime);

        initialScale = feetBubbleMask.transform.localScale.x;
        initialIntensity = worldBackLight.intensity;
        ResetBubbleLight();

        worldBackLight.intensity = lightLowIntensity;
    }

    private void Awake()
    {
        PlayerInput.onLightSwitch += ChangeLightState;
        JumpSimple.OnGroundLanding += LightOnLanding;

        feetCoroutine = LightBubbleTimer();
    }

    private void OnDestroy()
    {
        PlayerInput.onLightSwitch -= ChangeLightState;
        JumpSimple.OnGroundLanding -= LightOnLanding;

        if (feetCoroutine != null) { StopCoroutine(feetCoroutine); }
    }

    private void ChangeLightState(bool isLightOn)
    {
        switch (isLightOn)
        {
            case false:
                tilemapRenderer.maskInteraction = SpriteMaskInteraction.None;
                darkmapRenderer.gameObject.SetActive(false);
                doorsRenderer.gameObject.SetActive(false);

                worldBackLight.intensity = initialIntensity;
                soundManager.BubbleLightSuddenRemoveSFX();
                TurnOffBubbleLight();
                break;
            case true:
                tilemapRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                darkmapRenderer.gameObject.SetActive(true);
                doorsRenderer.gameObject.SetActive(true);

                worldBackLight.intensity = lightLowIntensity;
                break;
        }
        globalLight.gameObject.SetActive(!isLightOn);
        darkLight.gameObject.SetActive(isLightOn);
        exitLight.gameObject.SetActive(isLightOn);

        currentLight = isLightOn;
    }

    private void LightOnLanding(bool hasLanded)
    {
        if (feetCoroutine != null)
        {
            StopCurrentCoroutine(feetCoroutine);
        }

        if (hasLanded && currentLight)
        {
            feetCoroutine = LightBubbleTimer();
            StartCoroutine(feetCoroutine);
            soundManager.BubbleLightStartSFX(false);

        }
        else
        {
            TurnOffBubbleLight();
        }
    }

    #region LightBubbleTimer
    private IEnumerator LightBubbleTimer()
    {
        feetVector.x = feetPos.transform.position.x;
        feetVector.y = feetPos.transform.position.y - 0.1f;
        feetBubbleMask.transform.localPosition = feetVector;
        feetBubbleMask.gameObject.SetActive(true);

        while (currentOuterRadius < maxRadius && !isShrinking)
        {
            IncreaseOuterRadius();
            yield return null;
        }

        while (currentInnerRadius < maxRadius && !isShrinking && currentInnerRadius < currentOuterRadius)
        {
            lightInsideBubble.pointLightInnerRadius = currentInnerRadius;
            currentInnerRadius += Time.deltaTime;
            yield return null;
        }

        if (!isShrinking)
        {
            yield return waitForSeconds;
        }

        isShrinking = true;
        soundManager.BubbleLightStopSFX(false);

        while (currentOuterRadius > 0 && isShrinking)
        {
            ShrinkOuterRadius();
            yield return null;
        }

        ResetBubbleLight();
        feetBubbleMask.gameObject.SetActive(false);
    }

    private void IncreaseOuterRadius()
    {
        currentOuterRadius += radiusChangeSizeTime * Time.deltaTime;
        ChangeOuterRadius();
    }

    private void ShrinkOuterRadius()
    {
        currentOuterRadius -= radiusChangeSizeTime * Time.deltaTime;
        lightInsideBubble.pointLightInnerRadius = currentOuterRadius;
        ChangeOuterRadius();
    }

    private void ChangeOuterRadius()
    {
        lightInsideBubble.pointLightOuterRadius = currentOuterRadius;
        currentScale = GetNormalizedScale();
        newScale.x = currentScale;
        newScale.y = currentScale;
        feetBubbleMask.transform.localScale = newScale * initialScale;
    }
    #endregion LightBubbleTimer

    private float GetNormalizedScale()
    {
        return currentOuterRadius / maxRadius;
    }

    private void TurnOffBubbleLight()
    {
        ResetBubbleLight();
        feetBubbleMask.gameObject.SetActive(false);
        soundManager.BubbleLightStartSFX(true);
        soundManager.BubbleLightStopSFX(true);
    }

    private void ResetBubbleLight()
    {
        isShrinking = false;
        currentInnerRadius = 0;
        currentOuterRadius = 0;
        lightInsideBubble.pointLightInnerRadius = 0;
        lightInsideBubble.pointLightOuterRadius = 0;
    }

    private void StopCurrentCoroutine(IEnumerator currentCoroutine)
    {
        StopCoroutine(currentCoroutine);
        currentCoroutine = null;
    }
}
