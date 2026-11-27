using UnityEngine;

public class EnemyHitState : IState<EnemyController>
{
    public void EnterState(EnemyController context)
    {
        if (context.targetHP > 0)
            if (context.phase == 1)
                context.PlayAnim("Hit", 0.1f);
            else
                context.PlayAnim("HitPhase2", 0.1f);
        else
        {
            context.isDead = true;
            context.ChangeState(context.deathState);
        }
        context.StartCoroutine(context.SpawnBloodVFX());
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
