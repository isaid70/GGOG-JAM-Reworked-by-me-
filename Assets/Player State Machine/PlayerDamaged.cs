using UnityEditor.Rendering;
using UnityEngine;

public class PlayerDamaged : BaseMovementState
{
    float timer = 0f;
    public override void OnStateEnter()
    {
        PlayerStateMachine.instance.playerAnimator.SetTrigger("Damage");
        Time.timeScale = 0f;
    }

    public override void OnStateUpdate()
    {
        if (timer <= PlayerStateMachine.instance.timeBeforeMoveAfterDamaged)
        {
            timer += Time.unscaledDeltaTime;
        }
        else
        {
            GetToAnyState();
        }
    }

    public override void OnStateExit()
    {
        Time.timeScale = 1f;
        PlayerStateMachine.instance.playerAnimator.ResetTrigger("Damage");
    }
}
