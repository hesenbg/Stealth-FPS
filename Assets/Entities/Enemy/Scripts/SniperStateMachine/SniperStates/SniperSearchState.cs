using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperSearchState : SniperState
{
    public SniperSearchState(SniperContext _context, SniperStateMachine.SniperState key) : base(_context, key)
    {
        context = _context;
    }

    List<Vector3> CoverPos;
    int currentCoverIndex;
    float Timer;
    SniperStateMachine.SniperState NextState = SniperStateMachine.SniperState.Search;

    public override SniperStateMachine.SniperState GetNextState() => NextState;

    public override IEnumerator OnStateEnter()
    {
        context.GetEvents.FightEvent += OnPlayerSeen;
        context.GetEvents.AlarmEvent += OnPlayerSeen;

        currentCoverIndex = 0;
        Timer = 0f;
        CoverPos = EnemyManager.instance.GenerateCoverPos(4, 10, context.GetParent.transform.position);
        yield return null;
    }

    private void OnPlayerSeen(object sender, EventData e)
    {
        EnemyManager.instance.LKP = e.GetPos();
        NextState = SniperStateMachine.SniperState.Fight;
        EnemyManager.instance.AlertAllies();
    }

    public override IEnumerator OnStateExit()
    {
        context.GetEvents.FightEvent -= OnPlayerSeen;
        context.GetEvents.AlarmEvent -= OnPlayerSeen;

        yield return null;
    }

    public override void OnStateUpdate()
    {
        if (currentCoverIndex >= CoverPos.Count)
        {
            NextState = SniperStateMachine.SniperState.idle;
            return;
        }

        Vector3 toTarget = CoverPos[currentCoverIndex] - context.GetParent.transform.position;
        context.UpdateRotation(toTarget, 2f);

        if (Vector3.Angle(context.GetSight.transform.forward, toTarget) < 5f)
        {
            Timer += Time.deltaTime;
            if (Timer >= context.GetData.WonderTimer)
            {
                Timer = 0f;
                currentCoverIndex++;
            }
        }
    }
}