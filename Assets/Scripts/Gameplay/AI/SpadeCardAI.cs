using System.Collections;
using UnityEngine;

public class SpadeCardAI : BaseAI
{
    [Header("Leap")]
    public float leapDistance = 4f;
    public float leapForce = 10f;
    public float leapCooldown = 4f;
    public float slamRadius = 1.5f;

    private float _nextLeapTime;
    private bool _isLeaping;

    private void Update()
    {
        if (_isLeaping) return;

        float distance = GetDistanceToPlayer();

        if (distance <= leapDistance && Time.time >= _nextLeapTime)
        {
            StartCoroutine(LeapRoutine());
        }
        else if (distance <= DetectionRange)
        {
            MoveToPlayer();
        }
        else
        {
            StopMoving();
        }
    }

    private IEnumerator LeapRoutine()
    {
        _isLeaping = true;
        _nextLeapTime = Time.time + leapCooldown;

        StopMoving();
        Vector2 targetPos = PlayerTarget.position;
        Vector2 dir = (targetPos - (Vector2)transform.position).normalized;

        yield return new WaitForSeconds(0.2f);

        Rb2D.linearVelocity = dir * leapForce;

        yield return new WaitForSeconds(0.4f);

        StopMoving();
        // alan hasarý
        Collider2D hit = Physics2D.OverlapCircle(transform.position, slamRadius, LayerMask.GetMask("Player"));

        yield return new WaitForSeconds(0.3f);
        _isLeaping = false;
    }
}