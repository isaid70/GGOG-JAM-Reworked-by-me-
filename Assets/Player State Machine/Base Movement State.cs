using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseMovementState
{
    public virtual void OnStateEnter()
    {
    }

    public virtual void OnStateUpdate()
    {
        ChangeAttackColliderDirection(ref PlayerStateMachine.instance.attackColliderPivotTransform, PlayerStateMachine.instance.dashDirection.normalized);
        SetDashDirection();
    }

    public virtual void OnStateFixedUpdate()
    {

    }
    public virtual void OnStateExit()
    {
    }

    public void ChangeAttackColliderDirection(ref Transform transform,Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.normalized.y, direction.normalized.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void LookForRotation()
    {
        if (PlayerInputManager.instance.playerInput.Player.Movement.ReadValue<Vector2>().x < 0)
        {
            PlayerStateMachine.instance.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else if (PlayerInputManager.instance.playerInput.Player.Movement.ReadValue<Vector2>().x > 0)
        {
            PlayerStateMachine.instance.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void SetDashDirection()
    {
        if (PlayerInputManager.instance.playerInput.Player.Movement.ReadValue<Vector2>().normalized != Vector2.zero)
        {
            PlayerStateMachine.instance.dashDirection = PlayerInputManager.instance.playerInput.Player.Movement.ReadValue<Vector2>().normalized;
        }
    }




    //Get To Any State From A State When Button Pressed.If Any Button Pressed Then Switch To The Idle State
    public void GetToAnyState()
    {
        if(PlayerInputManager.instance.playerInput.Player.Movement.IsPressed())
        {
            PlayerStateMachine.instance.ChangeCurrentState(new PlayerMove());
        }

        else if (PlayerInputManager.instance.playerInput.Player.Dash.IsPressed())
        {
            PlayerStateMachine.instance.ChangeCurrentState(new PlayerDash(PlayerStateMachine.instance.dashDirection));
        }

        else if(PlayerInputManager.instance.playerInput.Player.Attack.IsPressed())
        {
            PlayerStateMachine.instance.ChangeCurrentState(new PlayerSwordAttack1());
        }

        else
        {
            PlayerStateMachine.instance.ChangeCurrentState(new PlayerIdle());
        }
    }

}
