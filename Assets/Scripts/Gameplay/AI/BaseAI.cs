using UnityEngine;

public abstract class BaseAI : MonoBehaviour
{
    [Header("Base Settings")]
    public Transform playerTarget;
    public float moveSpeed = 4f;
    public float detectionRange = 10f;

    protected Rigidbody2D Rb2D;

    protected virtual void Awake()
    {
        Rb2D = GetComponent<Rigidbody2D>();

        Rb2D.freezeRotation = true;

        if (playerTarget == null)
            playerTarget = GameObject.FindWithTag("Player")?.transform;
    }

    public void MoveToPlayer()
    {
        if (playerTarget == null) return;

        Vector2 direction = (playerTarget.position - transform.position).normalized;

        Rb2D.linearVelocity = direction * moveSpeed;

        FlipSprite(direction.x);
    }

    public void StopMoving()
    {
        Rb2D.linearVelocity = Vector2.zero;
    }

    public void FlipSprite(float inputX)
    {
        if (inputX > 0.1f)
            transform.localScale = new Vector3(1, 1, 1); // Sağ
        else if (inputX < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1); // sol
    }


    public float GetDistanceToPlayer()
    {
        if (playerTarget == null) return float.MaxValue;
        return Vector2.Distance(transform.position, playerTarget.position);
    }
}