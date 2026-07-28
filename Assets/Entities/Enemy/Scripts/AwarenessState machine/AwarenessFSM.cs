using System;
using UnityEngine;
[RequireComponent(typeof(EnemyStateMachine))]
public class AwarenessFSM : MonoBehaviour
{
    EnemyStateMachine fsm;
    EnemyAIData data;
    [Serializable]
    public struct EnemyAwarnesParams
    {
        public float SightRange;
        public float AwarenessSpeed;
        public float Angle;
        public float AllyCallSpeed;
        public float AllyCallNumber;
        public float AudioDetetctionRange;
        public float MovementSpeed;

        public static EnemyAwarnesParams Zero()
        {
            return new EnemyAwarnesParams
            {
                SightRange = 0f,
                AwarenessSpeed = 0f,
                Angle = 0f,
                AllyCallSpeed = 0f,
                AllyCallNumber = 0f,
                AudioDetetctionRange = 0f,
                MovementSpeed = 0f,
            };
        }

        public static EnemyAwarnesParams Lerp(EnemyAwarnesParams a, EnemyAwarnesParams b, float t)
        {
            return new EnemyAwarnesParams
            {
                SightRange = Mathf.Lerp(a.SightRange, b.SightRange, t),
                AwarenessSpeed = Mathf.Lerp(a.AwarenessSpeed, b.AwarenessSpeed, t),
                Angle = Mathf.Lerp(a.Angle, b.Angle, t),
                AllyCallSpeed = Mathf.Lerp(a.AllyCallSpeed, b.AllyCallSpeed, t),
                AllyCallNumber = Mathf.Lerp(a.AllyCallNumber, b.AllyCallNumber, t),
                AudioDetetctionRange = Mathf.Lerp(a.AudioDetetctionRange, b.AudioDetetctionRange, t),
                MovementSpeed = Mathf.Lerp(a.MovementSpeed, b.MovementSpeed, t),
            };
        }

        public static float Difference(EnemyAwarnesParams a, EnemyAwarnesParams b)
        {
            return Mathf.Max(
                Mathf.Abs(a.SightRange - b.SightRange),
                Mathf.Abs(a.AwarenessSpeed - b.AwarenessSpeed),
                Mathf.Abs(a.Angle - b.Angle),
                Mathf.Abs(a.AllyCallSpeed - b.AllyCallSpeed),
                Mathf.Abs(a.AllyCallNumber - b.AllyCallNumber),
                Mathf.Abs(a.AudioDetetctionRange - b.AudioDetetctionRange),
                Mathf.Abs(a.MovementSpeed - b.MovementSpeed)
            );
        }
    }

    [SerializeField] EnemyAwarnesParams Idle;
    [SerializeField] EnemyAwarnesParams Suspicious;
    [SerializeField] EnemyAwarnesParams Alarmed;
    [SerializeField] float TransitionSpeed = 2f;
    [SerializeField] float SnapAccuracy = 0.01f;

    [SerializeField] EnemyUI UI;

    public EnemyAwarnesParams CurrentParams;
    EnemyStateMachine.EnemyState currentEnemyState;

    public enum AwarnessState {Idle, Suspicious, Alarmed}

    public AwarnessState CurrentAwarnessState= AwarnessState.Idle;

    public bool IsIdle()
    {
        return currentEnemyState == EnemyStateMachine.EnemyState.Idle;
    }

    private void Start()
    {
        UI = GetComponentInChildren<EnemyUI>(); 
        fsm = GetComponent<EnemyStateMachine>();
        data = fsm.context.enemyAIData;
        data.CurrentAwarenessState = Idle;
        currentEnemyState = fsm.Current;
    }

    private void Update()
    {
        currentEnemyState = fsm.Current;
        UpdateStates();
        ApplyStates();
    }

    private void ApplyStates()
    {
        if (UI.IsEffected)
            return;

        if (EnemyAwarnesParams.Difference(data.CurrentAwarenessState, CurrentParams) <= SnapAccuracy)
            data.CurrentAwarenessState = CurrentParams;
        else
            data.CurrentAwarenessState = EnemyAwarnesParams.Lerp(
                data.CurrentAwarenessState, CurrentParams, Time.deltaTime * TransitionSpeed);
    }

    private void UpdateStates()
    {
        switch (currentEnemyState)
        {
            case EnemyStateMachine.EnemyState.Idle:
                CurrentAwarnessState = AwarnessState.Idle;
                break;

            case EnemyStateMachine.EnemyState.Suspicious:
                CurrentAwarnessState = AwarnessState.Suspicious;
                break;

            case EnemyStateMachine.EnemyState.Alarmed:
            case EnemyStateMachine.EnemyState.Fight:
            case EnemyStateMachine.EnemyState.Search:
                CurrentAwarnessState = AwarnessState.Alarmed;
                break;
        }


        switch (CurrentAwarnessState)
        {
            case AwarnessState.Idle:
                CurrentParams = Idle;
                UI.IdleUI();
                break;

            case AwarnessState.Suspicious:
                CurrentParams = Suspicious;
                UI.SuspiciousUI();
                break;

            case AwarnessState.Alarmed:
                UI.AlarmedUI();
                CurrentParams = Alarmed;
                break;
        }
    }
}