using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwordAttack2 : BaseMovementState
{
    private System.Action<InputAction.CallbackContext> onAttack;

    private float timer = 0f;
    private bool attack3Flag = false;

    public override void OnStateEnter()
    {
        PlayerStateMachine.instance.playerAnimator.SetTrigger("Sword Attack 2");

        PlayerStateMachine.instance.SpawnSlashSmoke();

        onAttack = i => { attack3Flag = true; };

        PlayerInputManager.instance.playerInput.Player.Attack.performed += onAttack;


    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (timer < PlayerStateMachine.instance.swordattack2Time)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (attack3Flag)
            {
                PlayerStateMachine.instance.ChangeCurrentState(new PlayerSwordAttack3());
            }
            else
            {
                if (PlayerInputManager.instance.playerInput.Player.Dash.IsPressed())
                {
                    PlayerStateMachine.instance.ChangeCurrentState(new PlayerDash(PlayerStateMachine.instance.dashDirection));
                }
                else if (PlayerInputManager.instance.playerInput.Player.Movement.IsPressed())
                {
                    PlayerStateMachine.instance.ChangeCurrentState(new PlayerMove());
                }
                else
                {
                    PlayerStateMachine.instance.ChangeCurrentState(new PlayerIdle());
                }
            }
        }
    }

    public override void OnStateFixedUpdate()
    {
        PlayerStateMachine.instance.rb.AddForce(PlayerInputManager.instance.playerInput.Player.Movement.ReadValue<Vector2>().normalized * PlayerStateMachine.instance.forcePower);
    }
    public override void OnStateExit()
    {
        PlayerInputManager.instance.playerInput.Player.Attack.performed -= onAttack;

    }
}
