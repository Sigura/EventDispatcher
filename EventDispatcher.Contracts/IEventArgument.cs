namespace Dispatcher.Contracts
{
    public interface IEventArgument<T> : IEventArgument
    {
        T Subject { get; set; }
    }

    public interface IEventArgument
    { }
}