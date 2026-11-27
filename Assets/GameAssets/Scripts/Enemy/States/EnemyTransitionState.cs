using UnityEngine;

public class EnemyTransitionState : IState<EnemyController>
{
    public void EnterState(EnemyController context)
    {
        context.PlayAnim("Transition", 0.1f);
    }

    public void ExitState(EnemyController context)
    {
    }

    public void FixedUpdateState(EnemyController context)
    {
    }

    public void UpdateState(EnemyController context)
    {
        if (!EnemyCombat.instance.isTransitionGoingOn)
        {
            if (TurnManager.instance.currentTurn % 2 == 0)
            {
                context.ChangeState(new EnemyAttackState());
            }
            else
            {
                context.ChangeState(context.idleState);
            }
        }
    }
}
