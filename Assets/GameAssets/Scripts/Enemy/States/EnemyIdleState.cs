using UnityEngine;

public class EnemyIdleState : IState<EnemyController>
{
    public void EnterState(EnemyController context)
    {
        context.PlayAnim("Idle", 0.1f);
    }

    public void ExitState(EnemyController context)
    {
    }

    public void FixedUpdateState(EnemyController context)
    {
    }

    public void UpdateState(EnemyController context)
    {
    }
}
