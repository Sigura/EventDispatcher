namespace Dispatcher
{
    using System;
    using System.Collections.Generic;
    using Contracts;

    public class EventDispatcher : IEventDispatcher, IDisposable
    {
        private readonly Dictionary<Type, List<IEventHandler>> _eventHandlers = new Dictionary<Type, List<IEventHandler>>();
        private bool _disposed;
        private readonly object _syncObject = new object();

        public void Subscribe<T, TArg>(Action<TArg> action) where T : class where TArg : class, IEventArgument<T>
        {
            lock (this._syncObject)
            {
                var type = typeof (TArg);

                this.UpdateHandlers(type);

                this._eventHandlers[type].Add(new ActionEventHandler<TArg>(action));
            }
        }

        private void UpdateHandlers(Type type)
        {
            if (!this._eventHandlers.ContainsKey(type))
                this._eventHandlers[type] = new List<IEventHandler>();
        }

        public void Subscribe<T, TArg>(IEventHandler<T, TArg> eventHendler) where T : class, IEvent<TArg>, new() where TArg : class, IEventArgument<T>
        {
            lock (this._syncObject)
            {
                var type = typeof (TArg);

                this.UpdateHandlers(type);

                this._eventHandlers[type].Add(eventHendler);
            }
        }

        public void Subscribe<TArg>(Action<TArg> action) where TArg : class, IEventArgument
        {
            lock (this._syncObject)
            {
                var type = typeof (TArg);

                this.UpdateHandlers(type);

                this._eventHandlers[type].Add(new ActionEventHandler<TArg>(action));
            }
        }

        public void Subscribe<TArg>(IEventHandler<TArg> eventHendler) where TArg : class, IEventArgument
        {
            lock (this._syncObject)
            {
                var type = typeof (TArg);

                this.UpdateHandlers(type);

                this._eventHandlers[type].Add(eventHendler);
            }
        }

        public void Unsubscribe<T, TArg>(Action<TArg> action) where T : class where TArg : class, IEventArgument<T>
        {
            lock (this._syncObject)
            {
                var type = typeof (TArg);

                this.UpdateHandlers(type);

                this._eventHandlers[type].RemoveAll(
                    x => x is ActionEventHandler<TArg> && ((ActionEventHandler<TArg>) x).IsAction(action));
            }
        }

        public void Unsubscribe<TArg>(Action<TArg> action) where TArg : class, IEventArgument
        {
            lock (this._syncObject)
            {
                var type = typeof (TArg);

                this.UpdateHandlers(type);

                this._eventHandlers[type].RemoveAll(
                    x => x is ActionEventHandler<TArg> && ((ActionEventHandler<TArg>) x).IsAction(action));
            }
        }

        public void Unsubscribe<T, TArg>(IEventHandler<T, TArg> eventHendler) where T : class, IEvent<TArg>, new() where TArg : class, IEventArgument<T>
        {
            lock (this._syncObject)
            {
                var type = typeof (TArg);

                this.UpdateHandlers(type);

                this._eventHandlers[type].RemoveAll(x => x == eventHendler);
            }
        }

        public void Unsubscribe<TArg>(IEventHandler<TArg> eventHendler) where TArg : class, IEventArgument
        {
            lock (this._syncObject)
            {
                var type = typeof (TArg);

                this.UpdateHandlers(type);

                this._eventHandlers[type].RemoveAll(x => x == eventHendler);
            }
        }

        public void OnNext<T, TArg>(TArg args) where T : class, IEvent<TArg>, new() where TArg : class, IEventArgument<T>
        {
            lock (this._syncObject)
            {
                var type = typeof (TArg);

                if (!this._eventHandlers.ContainsKey(type))
                    return;

                this._eventHandlers[type].ForEach(x => this.Execute<T, TArg>(args, x));
            }
        }

        private void Execute<T, TArg>(TArg argument, IEventHandler eventHandler) where TArg : class, IEventArgument<T> where T : class, IEvent<TArg>, new()
        {
            if (this._context == null || this._context.IsDisposed)
            {
                eventHandler.OnNext(argument);
            }
            else
            {
                this._context.Add<T, TArg>(eventHandler, argument);
            }
        }

        #region Observable pattern
        //public ISubject<IEventArgument> GetSubject<TArg>()
        //    where TArg : class
        //{
        //    var type = typeof(TArg);
        //    if (!this._eventHandlers.ContainsKey(type))
        //        this._eventHandlers[type] = new Subject<IEventArgument>();

        //    return this._eventHandlers[type];
        //}

        //public void OnCompleted<T, TArg>(TArg args)
        //    where T : class
        //    where TArg : class, IEventArgument<T>
        //{
        //    var type = typeof(TArg);

        //    if (!this._eventHandlers.ContainsKey(type))
        //        return;
        //    _eventHandlers[type].OnCompleted();
        //}

        //public void OnError<T>(Exception e)
        //    where T : class
        //{
        //    var type = typeof(T);

        //    if (!this._eventHandlers.ContainsKey(type))
        //        return;
        //    this._eventHandlers[type].OnError(e);
        //}

        //public void OnError(Exception error)
        //{
        //    var type = typeof(Unknown);

        //    if (!this._eventHandlers.ContainsKey(type))
        //        return;

        //    this._eventHandlers[type].OnError(error);
        //}

        //public void OnCompleted()
        //{
        //    var type = typeof(Unknown);

        //    if (!this._eventHandlers.ContainsKey(type))
        //        return;

        //    this._eventHandlers[type].OnCompleted();
        //}
        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (this._syncObject)
            {
                if (this._disposed) return;

                if (disposing)
                {
                    this._eventHandlers?.Clear();
                }

                this._disposed = true;
            }
        }

        ~EventDispatcher()
        {
            this.Dispose(false);
        }

        #endregion

        private IEventDispatcherContext _context;

        public IDisposable CreateContext()
        {
            if (this._context != null && !this._context.IsDisposed)
                throw new InvalidOperationException("context exists");

            return this._context = new EventDispatcherContext();
        }
    }
}
