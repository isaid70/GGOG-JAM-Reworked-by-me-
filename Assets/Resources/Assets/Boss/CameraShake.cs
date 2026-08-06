using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private Vector3 originalPosition;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.7f;
    private float dampingSpeed = 1.0f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void OnEnable()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        originalPosition = transform.localPosition;
        if (shakeDuration > 0)
        {

            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            GetComponent<followPlayer>().enabled = true;
            shakeDuration = 0f;
        }
    }

    // CameraShake.Instance.Shake(0.3f, 0.2f);
    public void Shake(float duration, float magnitude)
    {

        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}