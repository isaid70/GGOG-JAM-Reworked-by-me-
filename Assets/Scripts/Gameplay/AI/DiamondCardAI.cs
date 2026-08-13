using UnityEngine;

public class DiamondCardAI : BaseAI
{
    [Header("Ranged")]
    public GameObject cardProjectilePrefab;
    public Transform firePoint;
    public float keepDistance = 5f;
    public float shootCooldown = 2f;

    private float _nextShootTime;

    private void Update()
    {
        float distance = GetDistanceToPlayer();

        if (distance <= DetectionRange)
        {
            if (distance < keepDistance - 1f)
            {
                Vector2 awayDir = (transform.position - PlayerTarget.position).normalized;
                Rb2D.linearVelocity = awayDir * MoveSpeed;
            }
            else if (distance <= keepDistance + 1f)
            {
                StopMoving();
                if (Time.time >= _nextShootTime) ShootRanged();
            }
            else
            {
                MoveToPlayer();
            }
        }
        else
        {
            StopMoving();
        }
    }

    private void ShootRanged()
    {
        _nextShootTime = Time.time + shootCooldown;
        StopMoving();

        if (cardProjectilePrefab != null && firePoint != null)
        {
            Vector2 dir = (PlayerTarget.position - firePoint.position).normalized;
            GameObject bullet = Instantiate(cardProjectilePrefab, firePoint.position, Quaternion.identity);

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null) bulletRb.linearVelocity = dir * 8f;
        }
    }
}