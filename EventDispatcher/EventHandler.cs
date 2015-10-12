namespace Dispatcher
{
    using Contracts;

    public abstract class EventHandler<TEvent, TArg> : IEventHandler<TEvent, TArg>
        where TEvent : IEvent<TArg>, new() where TArg : class, IEventArgument
    {
        protected abstract void Handler(TArg arg);


        public void OnNext(TArg eventArgument)
        {
            this.Handler(eventArgument);
        }

        public void OnNext(IEventArgument<TArg> eventArgument)
        {
            this.Handler(eventArgument.Subject);
        }

        public void OnNext(IEventArgument eventArgument)
        {
            this.Handler(null);
        }
    }
}