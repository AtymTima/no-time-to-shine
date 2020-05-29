using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    bool isLightOn;
    bool shineIsOff;
    bool inputIsOff;

    KeyCode lightSwitcher;
    KeyCode jumpKey;
    KeyCode restartLevel;
    KeyCode quitKey;

    public delegate void OnLightSwitch(bool isLightOn);
    public static event OnLightSwitch onLightSwitch = delegate { };

    public delegate void OnJumpPressed(bool isJumping);
    public static event OnJumpPressed onJumpPressed = delegate { };

    public delegate void OnRestartPressed(GameObject player);
    public static event OnRestartPressed onRestartPressed = delegate { };

    private void Awake()
    {
        CandleTimer.OnCandleTimerStop += BlockShineAbility;
        Doors.OnExitInput += BlockInput;
        BlackHole.onSecondPhase += TemporaryBlockOfShine;

        lightSwitcher = KeyCode.W;
        jumpKey = KeyCode.Space;
        restartLevel = KeyCode.X;
        quitKey = KeyCode.U;
    }

    private void OnDestroy()
    {
        CandleTimer.OnCandleTimerStop -= BlockShineAbility;
        Doors.OnExitInput -= BlockInput;
        BlackHole.onSecondPhase -= TemporaryBlockOfShine;
    }

    private void Update()
    {
        if (inputIsOff) { return; }

        if (Input.GetKeyDown(lightSwitcher) && !shineIsOff)
        {
            onLightSwitch?.Invoke(isLightOn);
            isLightOn = !isLightOn;

        }

        if (Input.GetKeyDown(jumpKey))
        {
            onJumpPressed?.Invoke(true);
        }

        if (Input.GetKeyUp(jumpKey))
        {
            onJumpPressed?.Invoke(false);
        }

        if (Input.GetKeyDown(restartLevel))
        {
            onRestartPressed?.Invoke(gameObject);
        }

        if (Input.GetKeyDown(quitKey))
        {
            Application.Quit();
        }
    }

    private void BlockShineAbility()
    {
        shineIsOff = !shineIsOff;
        if (!isLightOn) { return; }
        onLightSwitch?.Invoke(isLightOn);
    }

    private void TemporaryBlockOfShine(bool isLight)
    {
        shineIsOff = !shineIsOff;
        if (!isLight)
        {
            if (isLightOn)
            {
                return;
            }
        }
        else
        {
            if (!isLightOn)
            {
                return;
            }
        }
        onLightSwitch?.Invoke(isLightOn);
        isLightOn = !isLightOn;
    }

    private void BlockInput()
    {
        inputIsOff = true;
    }
}
