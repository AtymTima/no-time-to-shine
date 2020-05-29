using UnityEngine;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [DllImport("__Internal")]
    private static extern void openWindow(string url);
    private static bool isInteractive = true;

    public delegate void OnButtonHover(bool isDarkMode);
    public static event OnButtonHover onButtonHover = delegate { };
    public delegate void OnGameStarted(GameObject currentLevel);
    public static event OnGameStarted onGameStarted = delegate { };

    [SerializeField] SoundManager soundManager;
    [SerializeField] Animator currentAnimator;
    [SerializeField] RectTransform currentBtn;
    [SerializeField] float speedOfScale = 2f;
    [SerializeField] float maximumScale = 1.15f;
    [SerializeField] bool isLastLevel;

    Vector2 currentScale;
    bool isScale;
    bool isHover;
    float timeRemains = 7.5f;

    private void Awake()
    {
        if (currentAnimator != null)
        {
            currentAnimator.SetBool("isGameOver", false);
        }
        isInteractive = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isInteractive) { return; }
        isHover = true;
        isScale = true;
        if (!isLastLevel)
        {
            onButtonHover?.Invoke(false);
            soundManager.CandelTimerStartSFX(false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isInteractive) { return; }
        OnExitButton();
    }

    public void OpenYoutubeLink()
    {
        if (!isInteractive) { return; }

        OnExitButton();
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {

            #if !UNITY_EDITOR
                             openWindow("https://bit.ly/atymtima_games");
            #endif
            return;
        }
        Application.OpenURL("https://bit.ly/atymtima_games");
    }

    private void OnExitButton()
    {
        if (!isLastLevel)
        {
            onButtonHover?.Invoke(true);
            soundManager.CandelTimerStopSFX(false);
            soundManager.CandelTimerStartSFX(true);
        }
        isScale = false;
    }

    public void StartTheGame()
    {
        if (!isInteractive) { return; }
        isInteractive = false;
        StartTransition();
    }

    private void StartTransition()
    {
        soundManager.CandelTimerStartSFX(true);
        currentAnimator.SetBool("isGameOver", true);
    }

    private void Update()
    {
        if (!isHover) { return; }
        if (isScale)
        {
            currentScale.x += speedOfScale * Time.deltaTime;
            currentScale.x = Mathf.Clamp(currentScale.x, 1, maximumScale);
            currentScale.y += speedOfScale * Time.deltaTime;
            currentScale.y = Mathf.Clamp(currentScale.y, 1, maximumScale);
            currentBtn.localScale = currentScale;
        }
        else
        {
            currentScale.x -= speedOfScale * Time.deltaTime;
            currentScale.x = Mathf.Clamp(currentScale.x, 1, maximumScale);
            currentScale.y -= speedOfScale * Time.deltaTime;
            currentScale.y = Mathf.Clamp(currentScale.y, 1, maximumScale);
            currentBtn.localScale = currentScale;
            if (currentScale.x <= 1)
            {
                isHover = false;
            }
        }

        if (!isInteractive)
        {
            if (timeRemains >= Mathf.Epsilon)
            {
                timeRemains -= Time.deltaTime;
            }
            else
            {
                LoadTheGame();
            }
        }
    }

    private void LoadTheGame()
    {
        onGameStarted?.Invoke(gameObject);
    }
}
