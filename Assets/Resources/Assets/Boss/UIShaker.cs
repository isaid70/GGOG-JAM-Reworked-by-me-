using UnityEngine;

public class UIShaker : MonoBehaviour
{
    public static UIShaker Instance;

    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 10f; // pixel cinsinden
    private float dampingSpeed = 1.0f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        rectTransform = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        originalPosition = rectTransform.anchoredPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            rectTransform.anchoredPosition = originalPosition + (Vector3)Random.insideUnitCircle * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            rectTransform.anchoredPosition = originalPosition;
        }
    }

    // UIShaker.Instance.Shake(0.5f, 20f);

    public void Shake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
