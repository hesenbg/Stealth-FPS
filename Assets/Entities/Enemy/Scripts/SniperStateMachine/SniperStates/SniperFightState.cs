using System.Collections;
using UnityEngine;

public class SniperFightState : SniperState
{
    public SniperFightState(SniperContext _context, SniperStateMachine.SniperState key) : base(_context, key)
    {
        context = _context;
    }

    public override SniperStateMachine.SniperState GetNextState()
    {
        return StateKey;
    }

    public override IEnumerator OnStateEnter()
    {
        yield return null;
    }

    public override IEnumerator OnStateExit()
    {
        yield return null;
    }

    public override void OnStateUpdate()
    {
        
    }
}
