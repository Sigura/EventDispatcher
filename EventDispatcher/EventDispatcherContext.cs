namespace Dispatcher
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Contracts;
    using Extensions;

    public class EventDispatcherContext : IEventDispatcherContext
    {
        private readonly List<EventStackItem> _events = new List<EventStackItem>();

        public bool IsDisposed { get; private set; }

        public void Add<T, TArg>(IEventHandler eventHandler, TArg argument) where T : class, IEvent<TArg>, new() where TArg : class, IEventArgument<T>
        {
            this._events.Add(new EventStackItem
            {
                Handler = eventHandler,
                Argument = argument
            });
        }

        ~EventDispatcherContext()
        {
            this.Dispose(false);
        }

        public void Add<T, TArg>(IEventHandler<T, TArg> eventHandler, TArg arg)
            where TArg : class, IEventArgument
            where T : class, IEvent<TArg>, new()
        {
            this._events.Add(new EventStackItem
            {
                Handler = eventHandler,
                Argument = arg
            });
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    Contract.Assume(this._events != null);


                    var priorityHandlers = this._events
                        .Where(e => e.Handler is IPriorityEventHandler)
                        .ToArray();

                    priorityHandlers
                        .Where(e => ((IPriorityEventHandler)e.Handler).Priority == Priority.High)
                        .ForEach(e => e.Invoke());

                    priorityHandlers.Where(e => ((IPriorityEventHandler)e.Handler).Priority == Priority.Normal)
                        .ForEach(e => e.Invoke());

                    this._events
                        .Where(e => !(e.Handler is IPriorityEventHandler))
                        .ForEach(e => e.Invoke());

                    priorityHandlers.Where(e => ((IPriorityEventHandler)e.Handler).Priority == Priority.Low)
                        .ForEach(e => e.Invoke());

                    this._events.Clear();
                }
            }

            this.IsDisposed = true;
        }
    }


}