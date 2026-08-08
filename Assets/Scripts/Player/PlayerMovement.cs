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
    SpriteRenderer sr;
    Camera mainCamera;
    PlayerInputManager inputManager;

    Vector2 input;
    Vector2 lastDirection = Vector2.down;

    bool isDashing;
    bool isAttacking;
    bool canDash = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }

    void Start()
    {
        inputManager = PlayerInputManager.Instance;
    }

    void Update()
    {
        if (inputManager == null)
        {
            inputManager = PlayerInputManager.Instance;
        }

        if (!CanMove())
        {
            rb.linearVelocity = Vector2.zero;
            SetSpeed(0);
            return;
        }

        if (!isDashing && !isAttacking)
        {
            input = inputManager != null ? inputManager.ReadMovement().normalized : Vector2.zero;

            if (input != Vector2.zero)
            {
                lastDirection = input;
            }
        }

        FlipToMouse();
        HandleAnimation();

        if (inputManager != null
            && inputManager.AttackWasPressedThisFrame()
            && CanAttack()
            && !isAttacking
            && !isDashing)
        {
            StartCoroutine(Attack());
        }

        if (inputManager != null
            && inputManager.SkillWasPressedThisFrame()
            && CanAttack()
            && !isAttacking
            && !isDashing)
        {
            StartCoroutine(UseSkill());
        }

        if (inputManager != null
            && inputManager.DashWasPressedThisFrame()
            && CanDash()
            && canDash
            && !isAttacking
            && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (!CanMove())
            return;

        if (isDashing || isAttacking)
            return;

        rb.linearVelocity = input * moveSpeed;
    }

    void HandleAnimation()
    {
        if (anim == null)
        {
            return;
        }

        anim.SetFloat("MoveX", lastDirection.x);
        anim.SetFloat("MoveY", lastDirection.y);
        SetSpeed(input.sqrMagnitude);
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        rb.linearVelocity = Vector2.zero;


        combat?.Attack();

        yield return new WaitForSeconds(attackDuration);
        combat?.HideSwing();
        isAttacking = false;
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        anim?.Play("Dash");

        rb.linearVelocity = lastDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector2.zero;

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    void FlipToMouse()
    {
        if (sr == null || inputManager == null)
        {
            return;
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        Vector3 mousePos = inputManager.GetPointerWorldPosition(mainCamera);

        sr.flipX = mousePos.x < transform.position.x;
    }

    IEnumerator UseSkill()
    {
        isAttacking = true;

        switch (activeSkill)
        {
            case 0:
                break;

            case 1:
                combat.StartOrbit();
                break;

            case 2:
                break;
        }

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
    }

    public void GiveSkill(int skill)
    {
        activeSkill = skill;
    }

    bool CanMove()
    {
        return GameManager.Instance == null || GameManager.Instance.CanMove;
    }

    bool CanAttack()
    {
        return GameManager.Instance == null || GameManager.Instance.CanAttack;
    }

    bool CanDash()
    {
        return GameManager.Instance == null || GameManager.Instance.CanDash;
    }

    void SetSpeed(float speed)
    {
        anim?.SetFloat("Speed", speed);
    }
}
