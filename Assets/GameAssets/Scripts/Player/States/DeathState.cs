using UnityEngine;

public class DeathState : IState<PlayerController>
{
    public void EnterState(PlayerController context)
    {
        context.isDead = true;
        context.PlayAnim("Death", 0.1f);
        TurnManager.instance.isGameOver = true;
        context.StartCoroutine(context.DisplayYouLoseScreen());
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
