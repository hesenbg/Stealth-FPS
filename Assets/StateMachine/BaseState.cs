using System;
public abstract class BaseState<Estate> where Estate : Enum
{
    public Estate StateKey { get; private set;}

    public BaseState(Estate state)
    {
        StateKey = state;
    }
    public abstract void Init();
    public abstract void OnStateUpdate();
    public abstract void OnStateExit();
    public abstract void OnStateEnter();
    public abstract Estate GetNextState();
}