using JetBrains.Annotations;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : BaseMovementState 
{
    private System.Action<InputAction.CallbackContext> onAttack;
    private System.Action<InputAction.CallbackContext> onDashAgain;

    private Vector2 currentDirection;
    private bool isDashAgain = false;
    private float timer = 0f;

    //Dash içinde tekrar dashi etkinleþtirirsem dash anýmasyonuna takýlý kalýyor

    public override void OnStateEnter()
    {
            PlayerStateMachine.instance.playerAnimator.SetTrigger("Dash");

            onDashAgain =  i => isDashAgain = true;
            PlayerInputManager.instance.playerInput.Player.Dash.performed += onDashAgain;

            onAttack = i => PlayerStateMachine.instance.ChangeCurrentState(new PlayerSwordAttack1());
            PlayerInputManager.instance.playerInput.Player.Attack.performed += onAttack;

            PlayerStateMachine.instance.isDashing = true;
            Dash(currentDirection);


        
    }
    private void Dash(Vector2 direction)
    {
        PlayerStateMachine.instance.rb.AddForce(direction.normalized * PlayerStateMachine.instance.dashForce, ForceMode2D.Impulse);
    }

    public PlayerDash(Vector2 currentDirection)
    {
        this.currentDirection = currentDirection;
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        LookForRotation();
        if (timer < PlayerStateMachine.instance.dashTime)
        {
            timer += Time.deltaTime;
        }
        else if (PlayerInputManager.instance.playerInput.Player.Movement.IsPressed())
        {
            PlayerStateMachine.instance.ChangeCurrentState(new PlayerMove());
        }
        else if (isDashAgain)   
        {
            PlayerStateMachine.instance.ChangeCurrentState(new PlayerDash(PlayerStateMachine.instance.dashDirection));
        }
        else
        {
            PlayerStateMachine.instance.ChangeCurrentState(new PlayerIdle());
        }
    }

    public override void OnStateExit()
    {

        PlayerInputManager.instance.playerInput.Player.Attack.performed -= onAttack;

        PlayerInputManager.instance.playerInput.Player.Dash.performed -= onDashAgain;


        PlayerStateMachine.instance.canDash = false;

        PlayerStateMachine.instance.isDashing = false;
    }
}
