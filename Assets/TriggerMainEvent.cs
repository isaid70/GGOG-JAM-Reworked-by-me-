using System.Collections;
using UnityEngine;

[System.Serializable]
public class IntroAnimationSettings
{
    public float enterDistance = 1200f;
    public float enterDuration = 0.45f;
    public float enterDelay = 0f;

    public float exitDistance = 1200f;
    public float exitDuration = 0.35f;
    public float exitDelay = 0f;
}

public class TriggerMainEvent : MonoBehaviour
{
    [Header("Scene")]
    public GameObject boss;
    public GameObject introCanvas;
    public CanvasGroup introGroup;

    [Header("Intro Objects")]
    public RectTransform introBoss;
    public RectTransform introTitle;
    public RectTransform introLeftDice;
    public RectTransform introRightDice;

    [Header("Animation")]
    public IntroAnimationSettings bossSettings;
    public IntroAnimationSettings leftDiceSettings;
    public IntroAnimationSettings rightDiceSettings;
    public IntroAnimationSettings titleSettings;

    [Header("Timing")]
    public float holdTime = 0.15f;

    private bool hasTriggered;

    private Vector2 bossTarget;
    private Vector2 titleTarget;
    private Vector2 leftDiceTarget;
    private Vector2 rightDiceTarget;

    private void Start()
    {
        bossTarget = introBoss.anchoredPosition;
        titleTarget = introTitle.anchoredPosition;
        leftDiceTarget = introLeftDice.anchoredPosition;
        rightDiceTarget = introRightDice.anchoredPosition;

        boss.SetActive(false);
        introCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered)
            return;

        if (!collision.TryGetComponent(out PlayerHealth _))
            return;

        hasTriggered = true;

        StartCoroutine(BossIntroRoutine());
    }

    private IEnumerator BossIntroRoutine()
    {
        GameManager.Instance?.SetIntroState();

        introCanvas.SetActive(true);
        introGroup.alpha = 1f;

        PrepareStartPositions();

        yield return AnimateEnter();

        yield return new WaitForSecondsRealtime(holdTime);

        boss.SetActive(true);

        GameManager.Instance?.SetCombatState();

        yield return AnimateExit();

        introCanvas.SetActive(false);

        Destroy(introCanvas);
    }

    private void PrepareStartPositions()
    {
        introBoss.anchoredPosition =
            bossTarget + Vector2.right * bossSettings.enterDistance;

        introLeftDice.anchoredPosition =
            leftDiceTarget + Vector2.right * leftDiceSettings.enterDistance;

        introRightDice.anchoredPosition =
            rightDiceTarget + Vector2.right * rightDiceSettings.enterDistance;

        introTitle.anchoredPosition =
            titleTarget + Vector2.left * titleSettings.enterDistance;
    }

    private IEnumerator AnimateEnter()
    {
        yield return AnimateObject(
            introBoss,
            bossTarget,
            bossSettings,
            true,
            false
        );

        yield return AnimateObject(
            introLeftDice,
            leftDiceTarget,
            leftDiceSettings,
            true,
            true
        );

        yield return AnimateObject(
            introRightDice,
            rightDiceTarget,
            rightDiceSettings,
            true,
            true
        );

        yield return AnimateObject(
            introTitle,
            titleTarget,
            titleSettings,
            true,
            false
        );
    }

    private IEnumerator AnimateExit()
    {
        Coroutine bossExit = StartCoroutine(
            AnimateObject(
                introBoss,
                bossTarget + Vector2.right * bossSettings.exitDistance,
                bossSettings,
                false,
                false
            )
        );

        Coroutine leftDiceExit = StartCoroutine(
            AnimateObject(
                introLeftDice,
                leftDiceTarget + Vector2.right * leftDiceSettings.exitDistance,
                leftDiceSettings,
                false,
                false
            )
        );

        Coroutine rightDiceExit = StartCoroutine(
            AnimateObject(
                introRightDice,
                rightDiceTarget + Vector2.right * rightDiceSettings.exitDistance,
                rightDiceSettings,
                false,
                false
            )
        );

        Coroutine titleExit = StartCoroutine(
            AnimateObject(
                introTitle,
                titleTarget + Vector2.left * titleSettings.exitDistance,
                titleSettings,
                false,
                false
            )
        );

        yield return bossExit;
        yield return leftDiceExit;
        yield return rightDiceExit;
        yield return titleExit;
    }

    private IEnumerator AnimateObject(
        RectTransform target,
        Vector2 destination,
        IntroAnimationSettings settings,
        bool entering,
        bool shake)
    {
        float delay = entering
            ? settings.enterDelay
            : settings.exitDelay;

        float duration = entering
            ? settings.enterDuration
            : settings.exitDuration;

        bool isShaking = entering && shake
            ? true : false;

        if (delay > 0f)
            yield return new WaitForSecondsRealtime(delay);

        Vector2 start = target.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / duration);

            float eased = entering
                ? EaseOutCubic(t)
                : EaseInCubic(t);

            target.anchoredPosition =
                Vector2.Lerp(start, destination, eased);
            
            if (isShaking)
            {
                CameraShake.Instance?.Shake(0.3f, 0.2f);
            }

            yield return null;
        }

        target.anchoredPosition = destination;
    }

    private static float EaseOutCubic(float t)
    {
        return 1f - Mathf.Pow(1f - t, 3f);
    }

    private static float EaseInCubic(float t)
    {
        return t * t * t;
    }
}