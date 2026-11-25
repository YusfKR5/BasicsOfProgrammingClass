using UnityEngine;

public class EnemyDeathState : IState<EnemyController>
{
    public void EnterState(EnemyController context)
    {
        context.PlayAnim("Death", 0.2f);
        context.StartCoroutine(context.DisplayYouWonScreen());
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
