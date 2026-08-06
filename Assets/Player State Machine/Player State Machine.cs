using System;
using System.Collections;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public static PlayerStateMachine instance { get; private set; }

    [Header("Move State")]
    [HideInInspector] public Rigidbody2D rb;
    public float forcePower;
    public Transform attackColliderPivotTransform;

    [Header("Dash State")]
    public float dashTime;
    public float dashForce;
    [HideInInspector] public Vector2 dashDirection;
    [HideInInspector] public bool canDash;
    public float timeBeforeNextDash;
    [HideInInspector] public bool isDashing;


    [Header("Sword Combo States")]
    public float swordattack1Time;
    public float swordattack2Time;
    public float swordattack3Time;

    [HideInInspector] public BaseMovementState currentState;

    [HideInInspector] public Animator playerAnimator;

    [Header("Smoke Object References")]
    public GameObject smokeObject;
    public float smokeObjectVelocity;

    [Header("Damaged State")]
    public float timeBeforeMoveAfterDamaged;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        rb = GetComponent<Rigidbody2D>();

        playerAnimator = GetComponentInChildren<Animator>();

        canDash = true;
        isDashing = false;

        
    }

    private void Start()
    {
        ChangeCurrentState(new PlayerIdle());
        playerAnimator.ResetTrigger("Idle");
    }

    public void ChangeCurrentState(BaseMovementState state)
    {
        currentState?.OnStateExit();
        currentState = state;
        currentState.OnStateEnter();
    }

    private void Update()
    {
        currentState.OnStateUpdate();
        playerAnimator.SetFloat("Movement X", PlayerInputManager.instance.playerInput.Player.Movement.ReadValue<Vector2>().normalized.x);
        playerAnimator.SetFloat("Movement Y", PlayerInputManager.instance.playerInput.Player.Movement.ReadValue<Vector2>().normalized.y);


    }

    public void CallDashCourotine(float time)
    {
        StartCoroutine(WaitForNextDash(time));
    }

    public void SpawnSlashSmoke()
    {
        GameObject smokeObjectInstantiate = Instantiate(smokeObject,GetComponentInChildren<BoxCollider2D>().transform.position, attackColliderPivotTransform.transform.rotation);
        Rigidbody2D smokeRB = smokeObjectInstantiate.GetComponent<Rigidbody2D>();
        smokeRB.linearVelocity = smokeRB.transform.up * smokeObjectVelocity * Time.fixedDeltaTime;
        Destroy(smokeObjectInstantiate, 0.7f);
    }

    private IEnumerator WaitForNextDash(float time)
    {
        yield return new WaitForSeconds(time);
        canDash = true;
    }

    private void FixedUpdate()
    {
        currentState.OnStateFixedUpdate();
    }

}
