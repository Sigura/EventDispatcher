namespace Dispatcher.Contracts
{
    using System;

    public interface IEventDispatcherContext : IDisposable
    {
        void Add<T, TArg>(IEventHandler<T, TArg> eventHandler, TArg arg)
            where TArg : class, IEventArgument
            where T : class, IEvent<TArg>, new();

        bool IsDisposed { get; }

        void Add<T, TArg>(IEventHandler eventHandler, TArg argument)
            where T : class, IEvent<TArg>, new()
            where TArg : class, IEventArgument<T>;
    }
}