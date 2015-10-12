namespace Dispatcher.Contracts
{
    public class EventStackItem
    {
        public IEventHandler Handler { get; set; }
        public IEventArgument Argument { get; set; }

        public void Invoke()
        {
            this.Handler.OnNext(this.Argument);
        }
    }
}