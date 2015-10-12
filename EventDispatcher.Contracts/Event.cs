namespace Dispatcher.Contracts
{
    public interface IEvent<TArg>
        where TArg : class, IEventArgument
    {
        //TArg Argument { get; set; }
        //ISubject<TArg> Subject { get; set; }
    }
}