using UnityEngine;

public interface IState<TContext>
{
    public void EnterState(TContext context);

    public void ExitState(TContext context);

    public void UpdateState(TContext context);

    public void FixedUpdateState(TContext context);
}
