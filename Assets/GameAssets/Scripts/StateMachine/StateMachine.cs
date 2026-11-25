using UnityEngine;

public class StateMachine<TContext>
{
    public IState<TContext> currentState { get; private set; }
    
    public void Initialize(TContext context, IState<TContext> startingState)
    {
        currentState = startingState;
        currentState.EnterState(context);
    }

    public void ChangeState(TContext context, IState<TContext> newState)
    {
        currentState.ExitState(context);
        currentState = newState;
        currentState.EnterState(context);
    }

    public void Update(TContext context)
    {
        currentState.UpdateState(context);
    }

    public void FixedUpdate(TContext context)
    {
        currentState.FixedUpdateState(context);
    }
}
