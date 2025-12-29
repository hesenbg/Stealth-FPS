using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<Estate> : MonoBehaviour where Estate : Enum
{
    protected Dictionary<Estate , BaseState<Estate>> States = new Dictionary<Estate, BaseState<Estate>>();

    protected BaseState<Estate> CurrentState;

    bool OnTransitioningToState = false;

    private void Start()
    {
        CurrentState.OnStateEnter();
    }

    private void Update()
    {
        Estate NextStateKey = CurrentState.GetNextState();
        if (!OnTransitioningToState && NextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.OnStateUpdate();
        }
        else if (!OnTransitioningToState)
        {
            TransitionToState(NextStateKey);
        }

    }

    public void TransitionToState(Estate state)
    {
        OnTransitioningToState = true;
        CurrentState.OnStateExit();

        CurrentState = States[state];

        CurrentState.OnStateEnter();
        OnTransitioningToState = false;

    }
}
