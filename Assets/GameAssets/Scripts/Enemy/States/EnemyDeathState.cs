using UnityEngine;

public class EnemyDeathState : IState<EnemyController>
{
    public void EnterState(EnemyController context)
    {
        if (context.phase == 1)
        {
            context.targetHP = 0;
            EnemyCombat.instance.SetIsTransitionStart();
            context.ChangeState(context.transitionState);
        }
        else
        {
            context.targetHP = 0;
            context.isDead = true;
            context.PlayAnim("Death", 0.2f);
            context.StartCoroutine(context.DisplayYouWonScreen());
            TurnManager.instance.isGameOver = true;
        }
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
