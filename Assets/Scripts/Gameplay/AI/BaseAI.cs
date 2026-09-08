using UnityEngine;

public abstract class BaseAI : MonoBehaviour
{
    [Header("Base AI Settings")]
    public Transform PlayerTarget;
    public float MoveSpeed = 3.5f;
    public float DetectionRange = 8f;

    protected Rigidbody2D Rb2D;
    protected SpriteRenderer SpriteRenderer;

    protected virtual void Awake()
    {
        Rb2D = GetComponent<Rigidbody2D>();
        Rb2D.gravityScale = 0;
        Rb2D.freezeRotation = true;
        SpriteRenderer = GetComponent<SpriteRenderer>();

        if (PlayerTarget == null)
            PlayerTarget = GameObject.FindWithTag("Player")?.transform;
    }

    public void MoveToPlayer(float speedMultiplier = 1f)
    {
        if (PlayerTarget == null) return;

        Vector2 direction = (PlayerTarget.position - transform.position).normalized;
        Rb2D.linearVelocity = direction * (MoveSpeed * speedMultiplier);
        FlipSprite(direction.x);
    }

    public void StopMoving()
    {
        Rb2D.linearVelocity = Vector2.zero;
    }

    public void FlipSprite(float inputX)
    {
        if (inputX > 0.1f) SpriteRenderer.flipX = false;
        else if (inputX < -0.1f) SpriteRenderer.flipX = true;
    }

    public float GetDistanceToPlayer()
    {
        if (PlayerTarget == null) return float.MaxValue;
        return Vector2.Distance(transform.position, PlayerTarget.position);
    }
}