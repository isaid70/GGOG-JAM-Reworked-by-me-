using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdle : BaseMovementState
{
    private System.Action<InputAction.CallbackContext> onMove;
    private System.Action<InputAction.CallbackContext> onDash;
    private System.Action<InputAction.CallbackContext> onAttack;

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        //This is Reset The Movement Trigger For Instant Transitions 
        PlayerStateMachine.instance.playerAnimator.ResetTrigger("Movement");
        PlayerStateMachine.instance.playerAnimator.ResetTrigger("Dash");

        PlayerStateMachine.instance.playerAnimator.SetTrigger("Idle");

        onMove = i => PlayerStateMachine.instance.ChangeCurrentState(new PlayerMove());
        PlayerInputManager.instance.playerInput.Player.Movement.performed += onMove;

        onDash = i => { PlayerStateMachine.instance.ChangeCurrentState(new PlayerDash(PlayerStateMachine.instance.dashDirection)); };
        PlayerInputManager.instance.playerInput.Player.Dash.performed += onDash;

        onAttack = i => PlayerStateMachine.instance.ChangeCurrentState(new PlayerSwordAttack1());
        PlayerInputManager.instance.playerInput.Player.Attack.performed += onAttack;

        

    }


    public override void OnStateExit()
    {
        PlayerInputManager.instance.playerInput.Player.Movement.performed -= onMove;

        PlayerInputManager.instance.playerInput.Player.Dash.performed -= onDash;

        PlayerInputManager.instance.playerInput.Player.Attack.performed -= onAttack;
        
        base.OnStateExit();
    }

}
