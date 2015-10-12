namespace Dispatcher.Contracts
{
    using System;
    //using System.Reactive.Subjects;

    public interface IEventDispatcher//: IObserver<IEventArgument>
    {
        void Subscribe<T, TArg>(Action<TArg> action)
            where TArg : class, IEventArgument<T>
            where T : class;

        void Subscribe<TArg>(Action<TArg> action)
            where TArg : class, IEventArgument;

        void Subscribe<T, TArg>(IEventHandler<T, TArg> eventHendler)
            where TArg : class, IEventArgument<T>
            where T : class, IEvent<TArg>, new();

        void Subscribe<TArg>(IEventHandler<TArg> eventHendler)
            where TArg : class, IEventArgument;

        void Unsubscribe<T, TArg>(Action<TArg> action)
            where TArg : class, IEventArgument<T>
            where T : class;

        void Unsubscribe<TArg>(Action<TArg> action)
            where TArg : class, IEventArgument;

        void Unsubscribe<T, TArg>(IEventHandler<T, TArg> eventHendler)
            where TArg : class, IEventArgument<T>
            where T : class, IEvent<TArg>, new();

        void Unsubscribe<TArg>(IEventHandler<TArg> eventHendler)
            where TArg : class, IEventArgument;

        //ISubject<IEventArgument> GetSubject<T>()
        //    where T : class;

        void OnNext<T, TArg>(TArg args)
            where TArg : class, IEventArgument<T>
            where T : class, IEvent<TArg>, new();

        //void OnCompleted<T, TArg>(TArg args)
        //    where TArg : class, IEventArgument<T>
        //    where T : class;

        //void OnError<T>(Exception e)
        //    where T : class;

        IDisposable CreateContext();
    }
}
