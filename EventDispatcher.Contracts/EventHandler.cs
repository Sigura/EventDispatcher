namespace Dispatcher.Contracts
{
    public abstract class EventHandler<TArg> : IEventHandler<TArg> where TArg : class, IEventArgument
    {
        public void OnNext(IEventArgument eventArgument)
        {
            this.OnNext(eventArgument as TArg);
        }

        public abstract void OnNext(TArg eventArgument);
    }
}