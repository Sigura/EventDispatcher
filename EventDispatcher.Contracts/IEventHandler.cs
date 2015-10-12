namespace Dispatcher.Contracts
{
    public interface IEventHandler<TEvent, in TArg> : IEventHandler<TArg>
        where TArg : class, IEventArgument
        where TEvent : IEvent<TArg>, new()
    {
    }

    public interface IEventHandler<in TArg>: IEventHandler
        where TArg : class, IEventArgument
    {
        void OnNext(TArg eventArgument);
    }

    public interface IEventHandler
    {
        void OnNext(IEventArgument eventArgument);
    }
}