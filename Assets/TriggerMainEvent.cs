using System.Collections;
using UnityEngine;

public class TriggerMainEvent : MonoBehaviour
{
    public Camera cam;
    public GameObject boss;
    public float targetSize = 20f;
    public float zoomDuration = 1.5f;

    private bool hasZoomed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasZoomed || !collision.TryGetComponent(out PlayerHealth _))
        {
            return;
        }

        hasZoomed = true;

        if (boss != null)
        {
            boss.SetActive(true);
        }

        if (cam != null)
        {
            StartCoroutine(ZoomCamera());
        }
    }

    private IEnumerator ZoomCamera()
    {
        float startSize = cam.orthographicSize;
        float timeElapsed = 0f;

        while (timeElapsed < zoomDuration)
        {
            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, timeElapsed / zoomDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        cam.orthographicSize = targetSize;
    }
}
