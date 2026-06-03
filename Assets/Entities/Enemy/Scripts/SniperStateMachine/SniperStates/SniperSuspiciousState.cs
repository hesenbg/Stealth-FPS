using System.Collections;
using UnityEngine;

public class SniperSuspiciousState : SniperState
{
    public SniperSuspiciousState(SniperContext _context, SniperStateMachine.SniperState key) : base(_context, key)
    {
        context = _context;
    }

    SniperStateMachine.SniperState NextState = SniperStateMachine.SniperState.Suspicious;

    public override SniperStateMachine.SniperState GetNextState()
    {
        return NextState;
    }

    public override IEnumerator OnStateEnter()
    {
        context.GetEvents.FightEvent += OnPlayerSeen;
        context.GetEvents.SearchEvent += OnClueFound;
        context.GetEvents.AlarmEvent += OnPlayerSeen;

        NextState = SniperStateMachine.SniperState.Suspicious;
        CurrentTimer = 0f;
        HasReached = false;
        yield return null;
    }

    private void OnClueFound(object sender, EventData e)
    {
        context.GetData.CluePosition = e.GetPos();
        NextState = SniperStateMachine.SniperState.Search;
    }

    public override IEnumerator OnStateExit()
    {
        context.GetEvents.FightEvent -= OnPlayerSeen;
        context.GetEvents.SearchEvent -= OnClueFound;
        context.GetEvents.AlarmEvent -= OnPlayerSeen;

        CurrentTimer = 0f;
        yield return null;  
    }

    float CurrentTimer = 0f;

    bool HasReached = false;

    public override void OnStateUpdate()
    {
        if (CurrentTimer < context.GetData.WonderTimer)
        {
            CurrentTimer += Time.deltaTime;
            if(!HasReached)
                HasReached = context.UpdateRotation((context.GetData.last.Position - context.GetParent.transform.position).normalized, 3f);
        }
        else
        {
            CurrentTimer = 0f;
            NextState = SniperStateMachine.SniperState.idle;
        }
    }
    private void OnPlayerSeen(object sender, EventData e)
    {
        EnemyManager.instance.LKP = e.GetPos() ;
        NextState = SniperStateMachine.SniperState.Fight;
        EnemyManager.instance.AlertAllies();
    }
}