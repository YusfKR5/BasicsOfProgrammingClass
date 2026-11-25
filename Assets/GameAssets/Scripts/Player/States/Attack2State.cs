using UnityEngine;

public class Attack2State : IState<PlayerController>
{
    public void EnterState(PlayerController context)
    {
        context.PlayAnim("Attack2", 0.05f);
    }

    public void ExitState(PlayerController context)
    {
    }

    public void FixedUpdateState(PlayerController context)
    {
    }

    public void UpdateState(PlayerController context)
    {
        if (!context.isAttacking)
        {
            context.ChangeState(context.idleState);
            return;
        }
    }
}
