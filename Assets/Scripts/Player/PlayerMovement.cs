using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Dash")]
    public float dashSpeed = 12f;
    public float dashDuration = 0.25f;
    public float dashCooldown = 1f;

    [Header("Attack")]
    [SerializeField] PlayerCombat combat;
    public float attackDuration = 0.35f;
    public int activeSkill = 0;

    Rigidbody2D rb;
    Animator anim;

    Vector2 input;
    Vector2 lastDirection = Vector2.right;

    bool isDashing;
    bool isAttacking;
    bool canDash = true;
    SpriteRenderer sr;
    PlayerCombat playerCombat;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    void Update()
    {
        if (!GameManager.Instance.CanMove)
        {
            rb.linearVelocity = Vector2.zero;
            anim.Play("Idle");
            return;
        }

        if (!isDashing && !isAttacking)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            input.Normalize();

            if (input != Vector2.zero)
                lastDirection = input;
        }
        
        FlipToMouse();

        HandleAnimation();

        if (Input.GetMouseButtonDown(0)
            && GameManager.Instance.CanAttack
            && !isAttacking
            && !isDashing)
        {
            StartCoroutine(Attack());
        }
        if (Input.GetMouseButtonDown(1)
        && GameManager.Instance.CanAttack
        && !isAttacking
        && !isDashing)
        {
            StartCoroutine(UseSkill());
        }

        if (Input.GetKeyDown(KeyCode.Space)
            && GameManager.Instance.CanDash
            && canDash
            && !isAttacking
            && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.CanMove)
            return;

        if (isDashing || isAttacking)
            return;

        rb.linearVelocity = input * moveSpeed;
    }

    void HandleAnimation()
    {
        if (isAttacking)
        {
            //anim.Play("Attack");
            return;
        }

        if (isDashing)
        {
            anim.Play("Dash");
            return;
        }

        if (input != Vector2.zero)
        {
            anim.Play("Movement");
        }
        else
        {
            anim.Play("Idle");
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        rb.linearVelocity = Vector2.zero;

        combat.Attack();

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        anim.Play("Dash");

        rb.linearVelocity = lastDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector2.zero;

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    void FlipToMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        sr.flipX = mousePos.x < transform.position.x;
    }

    IEnumerator UseSkill()
    {
        isAttacking = true;
        switch (activeSkill)
        {
            case 0:
                // Skill 0 logic
                break;
            case 1:
                playerCombat.StartOrbit();
                break;
            case 2:
                // Skill 2 logic
                break;
            default:
                // Default skill logic
                break;
        }
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
    }

    public void GiveSkill(int skill)
    {
        activeSkill = skill;
    }
}