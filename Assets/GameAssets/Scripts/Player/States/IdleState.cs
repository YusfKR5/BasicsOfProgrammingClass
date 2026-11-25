using UnityEngine;

public class IdleState : IState<PlayerController>
{
    public void EnterState(PlayerController context)
    {
        context.PlayAnim("Idle", 0.1f);
        context.canAttack = true;
    }

    public void ExitState(PlayerController context)
    {
    }

    public void FixedUpdateState(PlayerController context)
    {
    }

    public void UpdateState(PlayerController context)
    {
    }
}