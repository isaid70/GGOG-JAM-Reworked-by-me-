using System.Collections;
using UnityEngine;

public class SpiderAI : BaseAI
{
    public enum CreatureState { Idle, Chase, RangedAttack, LeapAttack }
    public CreatureState currentState;

    [Header("Ranged Attack")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float rangedCooldown = 3f;
    private float _nextRangedTime;

    [Header("2D Leap / Dash Skill")]
    public float leapDistance = 4f;
    public float leapForce = 12f;
    public float leapDuration = 0.4f;
    public float leapCooldown = 5f;
    private float _nextLeapTime;
    private bool _isLeaping;

    private void Update()
    {
        if (_isLeaping) return; // Z�plarken karar alma

        float distance = GetDistanceToPlayer();

        if (distance <= leapDistance && Time.time >= _nextLeapTime)
        {
            StartCoroutine(JumpToPlayer());
        }
        else if (distance <= detectionRange && Time.time >= _nextRangedTime)
        {
            ShootRanged();
        }
        else if (distance <= detectionRange && distance > 0.5f)
        {
            currentState = CreatureState.Chase;
            MoveToPlayer();
        }
        else
        {
            currentState = CreatureState.Idle;
            StopMoving();
        }
    }

    private IEnumerator JumpToPlayer()
    {
        _isLeaping = true;
        currentState = CreatureState.LeapAttack;
        _nextLeapTime = Time.time + leapCooldown;

        StopMoving();


        Vector2 targetPosition = playerTarget.position;
        Vector2 leapDirection = (targetPosition - (Vector2)transform.position).normalized;

        yield return new WaitForSeconds(0.2f);//gerilme

        Rb2D.linearVelocity = leapDirection * leapForce;

        yield return new WaitForSeconds(leapDuration);
        StopMoving();
        TriggerDamage();

        yield return new WaitForSeconds(0.3f);//düşme

        _isLeaping = false;
    }

    private void ShootRanged()
    {
        _nextRangedTime = Time.time + rangedCooldown;
        StopMoving();

        if (bulletPrefab != null && firePoint != null)
        {
            Vector2 dir = (playerTarget.position - firePoint.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null) bulletRb.linearVelocity = dir * 8f;
        }
    }

    private void TriggerDamage()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, 1.5f, LayerMask.GetMask("Player"));
        if (hitPlayer != null)
        {

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
}