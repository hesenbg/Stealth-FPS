using System;
using System.Collections;

public abstract class BaseState<EState> where EState : Enum
{
    public EState StateKey { get; private set; }

    public BaseState(EState key)
    {
        StateKey = key;
    }

    public virtual void Init() { }
    public abstract IEnumerator OnStateEnter();
    public abstract IEnumerator OnStateExit();
    public abstract void OnStateUpdate();
    public abstract EState GetNextState();
}