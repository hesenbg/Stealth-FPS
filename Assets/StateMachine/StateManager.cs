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
        foreach(var state in States)
        {
           state.Value.Init();
        }

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
        UpdateStateMachine();
    }

    private void FixedUpdate()
    {
        if (!OnTransitioningToState)
        {
            CurrentState.OnStateFixedUpdate();
        }
    }

    public abstract void UpdateStateMachine();

    public void TransitionToState(Estate state)
    {
        OnTransitioningToState = true;
        CurrentState.OnStateExit();

        CurrentState = States[state];

        CurrentState.OnStateEnter();
        OnTransitioningToState = false;
    }
}