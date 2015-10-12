namespace Dispatcher.Contracts
{
    public interface IPriorityEventHandler: IEventHandler
    {
        Priority Priority { get; }
    }
}