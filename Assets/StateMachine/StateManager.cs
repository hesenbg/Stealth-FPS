using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> CurrentState;
    bool OnTransitioningToState = false;

    private void Start()
    {
        foreach (var state in States)
            state.Value.Init();

        StartCoroutine(CurrentState.OnStateEnter());
    }

    private void Update()
    {
        EState NextStateKey = CurrentState.GetNextState();
        if (!OnTransitioningToState && NextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.OnStateUpdate();
        }
        else if (!OnTransitioningToState)
        {
            StartCoroutine(TransitionToState(NextStateKey));
        }
        UpdateStateMachine();
    }

    private void FixedUpdate()
    {
        if (!OnTransitioningToState)
            CurrentState.OnStateFixedUpdate();
    }

    public abstract void UpdateStateMachine();

    public IEnumerator TransitionToState(EState state)
    {
        OnTransitioningToState = true;
        yield return StartCoroutine(CurrentState.OnStateExit());
        CurrentState = States[state];
        yield return StartCoroutine(CurrentState.OnStateEnter());
        OnTransitioningToState = false;
    }
}