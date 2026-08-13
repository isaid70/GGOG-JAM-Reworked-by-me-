using UnityEngine;


public class ClubCardAI : BaseAI
{
    [Header("Melee Attack")]
    public float attackRange = 1f;
    public float attackCooldown = 1.2f;

    private float _nextAttackTime;

    private void Update()
    {
        float distance = GetDistanceToPlayer();

        if (distance <= attackRange)
        {
            StopMoving();
            if (Time.time >= _nextAttackTime) Attack();
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

    private void Attack()
    {
        _nextAttackTime = Time.time + attackCooldown;
        // Býçak/Týrnak savurma animasyonu tetikle
        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Player"));
        if (hit != null) Debug.Log("Sinek kartý oyuncuyu diledi/vurdu!");
    }
}