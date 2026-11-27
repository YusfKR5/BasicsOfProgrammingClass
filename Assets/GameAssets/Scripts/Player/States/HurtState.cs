using UnityEngine;

public class HurtState : IState<PlayerController>
{
    public void EnterState(PlayerController context)
    {
        context.StartCoroutine(context.TakeDamage(EnemyCombat.instance.currentAttackData.damage));
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
