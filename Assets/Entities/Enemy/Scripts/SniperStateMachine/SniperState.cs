
public abstract class SniperState : BaseState<SniperStateMachine.SniperState>
{
    protected SniperContext context;

    public SniperState(SniperContext _context, SniperStateMachine.SniperState key) : base(key)
    {
        context = _context;
    }
}
