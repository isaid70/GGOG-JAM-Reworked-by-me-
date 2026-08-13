using System.Collections;
using UnityEngine;

public class HeartCardAI : BaseAI
{
    [Header("Bomb")]
    public float explodeDistance = 1.2f;
    public float explosionRadius = 2.5f;
    public float fuseTime = 1f; // Patlama öncesi geri sayým
    public GameObject explosionVFX;

    private bool _isExploding;

    private void Update()
    {
        if (_isExploding) return;

        float distance = GetDistanceToPlayer();

        if (distance <= explodeDistance)
        {
            StartCoroutine(ExplodeRoutine());
        }
        else if (distance <= DetectionRange)
        {
            // Patlamak için biraz daha hýzlý koþar
            MoveToPlayer(1.3f);
        }
        else
        {
            StopMoving();
        }
    }

    private IEnumerator ExplodeRoutine()
    {
        _isExploding = true;
        StopMoving();

        // Buraya kýrmýzý yanýp sönme efekti / ses eklenebilir
        Debug.Log("KUPA PATLIYOR! Kaç!");

        yield return new WaitForSeconds(fuseTime);

        // Patlama Aný
        Collider2D hit = Physics2D.OverlapCircle(transform.position, explosionRadius, LayerMask.GetMask("Player"));
        if (hit != null) Debug.Log("Kupa patladý, oyuncu dev hasar aldý!");

        if (explosionVFX != null) Instantiate(explosionVFX, transform.position, Quaternion.identity);

        Destroy(gameObject); // Kendini yok et
    }
}