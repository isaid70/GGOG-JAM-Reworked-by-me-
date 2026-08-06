using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : BaseMovementState
{
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private float forcePower;

    private System.Action<InputAction.CallbackContext> onIdle;
    private System.Action<InputAction.CallbackContext> onDash;
    private System.Action<InputAction.CallbackContext> onAttack;

    public override void OnStateEnter()
    {

        PlayerStateMachine.instance.playerAnimator.SetTrigger("Movement");

        rb = PlayerStateMachine.instance.rb;
        forcePower = PlayerStateMachine.instance.forcePower;

        onIdle = i => PlayerStateMachine.instance.ChangeCurrentState(new PlayerIdle());
        PlayerInputManager.instance.playerInput.Player.Movement.canceled += onIdle;

        onDash = i => {  PlayerStateMachine.instance.ChangeCurrentState(new PlayerDash(PlayerStateMachine.instance.dashDirection)); };
        PlayerInputManager.instance.playerInput.Player.Dash.performed += onDash;

        onAttack = i => PlayerStateMachine.instance.ChangeCurrentState(new PlayerSwordAttack1());
        PlayerInputManager.instance.playerInput.Player.Attack.performed += onAttack;

    }
    private void MoveToDirection(Vector2 direction)
    {
        if(rb != null)
        { 
            rb.AddForce(moveDirection.normalized * forcePower);
        }
    }


    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        moveDirection = PlayerInputManager.instance.playerInput.Player.Movement.ReadValue<Vector2>().normalized;
        PlayerStateMachine.instance.dashDirection = PlayerInputManager.instance.playerInput.Player.Movement.ReadValue<Vector2>().normalized;

        PlayerStateMachine.instance.playerAnimator.SetFloat("Movement X",PlayerInputManager.instance.playerInput.Player.Movement.ReadValue<Vector2>().normalized.x);
        PlayerStateMachine.instance.playerAnimator.SetFloat("Movement Y", PlayerInputManager.instance.playerInput.Player.Movement.ReadValue<Vector2>().normalized.y);

        

        LookForRotation();
    }

    public override void OnStateFixedUpdate()
    {
        MoveToDirection(moveDirection);
    }

    public override void OnStateExit()
    {

        PlayerInputManager.instance.playerInput.Player.Movement.canceled -= onIdle;

        PlayerInputManager.instance.playerInput.Player.Dash.performed -= onDash;

        PlayerInputManager.instance.playerInput.Player.Attack.performed -= onAttack;
    }




}
