using UnityEngine;

public class EnemyAttackState : IState<EnemyController>
{
    public void EnterState(EnemyController context)
    {
        int dice = Random.Range(0, EnemyCombat.instance.attackDatas.Count);
        EnemyCombat.instance.StartAttack(((EnemyCombat.Attacks)((dice + 1) * context.phase)).ToString(), dice);
    }

    public void ExitState(EnemyController context)
    {
    }

    public void FixedUpdateState(EnemyController context)
    {
    }

    public void UpdateState(EnemyController context)
    {
        if (TurnManager.instance.currentTurn % 2 == 1)
        {
            context.ChangeState(context.idleState);
        }
    }
}
