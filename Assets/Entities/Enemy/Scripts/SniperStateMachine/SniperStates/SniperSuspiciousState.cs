using System.Collections;
using UnityEngine;

public class SniperSuspiciousState : SniperState
{
    public SniperSuspiciousState(SniperContext _context, SniperStateMachine.SniperState key) : base(_context, key)
    {
        context = _context;
    }

    public override SniperStateMachine.SniperState GetNextState()
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator OnStateEnter()
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator OnStateExit()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStateUpdate()
    {
        throw new System.NotImplementedException();
    }
}
