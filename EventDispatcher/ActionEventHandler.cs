namespace Dispatcher
{
    using System;
    using Contracts;

    public class ActionEventHandler<TArg> : IEventHandler<TArg>
        where TArg : class, IEventArgument
    {
        private readonly Action<TArg> _action;

        public static implicit operator ActionEventHandler<TArg>(Action<TArg> action)
        {
            return new ActionEventHandler<TArg>(action);
        }

        public void OnNext(TArg argument)
        {
            this._action(argument);
        }

        public void OnNext(IEventArgument argument)
        {
            this.OnNext((TArg)argument);
        }

        public ActionEventHandler(Action<TArg> action)
        {
            this._action = action;
        }

        public bool IsAction(Action<TArg> action)
        {
            return action == this._action;
        }

        #region Observable patter
        //protected readonly Action<Exception> Error;
        //private readonly Action<TArg> _onCompleted;
        //public ActionEventHandler(Action<TArg> action, Action<Exception> error)
        //    : this(action)
        //{
        //    this.Error = error;
        //}

        //public ActionEventHandler(Action<TArg> action, Action<Exception> onError, Action<TArg> onCompleted)
        //    : this(action, onError)
        //{
        //    this._onCompleted = onCompleted;
        //}

        //public void OnError(Exception error)
        //{
        //    this.Error?.Invoke(error);
        //}

        //public void OnCompleted()
        //{
        //    this._onCompleted?.Invoke(null);
        //}

        //public void OnCompleted(TArg argument)
        //{
        //    this._onCompleted?.Invoke(argument);
        //}

        //public void OnCompleted(IEventArgument argument)
        //{
        //    this.OnCompleted((TArg)argument);
        //}
        #endregion
    }

    public class ActionEventHandler<T, TArg> : ActionEventHandler<TArg>
        where TArg : class, IEventArgument<T>
    {
        public static implicit operator ActionEventHandler<T, TArg>(Action<TArg> action)
        {
            return new ActionEventHandler<T, TArg>(action);
        }

        public ActionEventHandler(Action<TArg> action)
            : base(action)
        {
        }

        //public ActionEventHandler(Action<TArg> action, Action<Exception> error)
        //    : base(action, error)
        //{
        //}

        //public ActionEventHandler(Action<TArg> action, Action<Exception> onError, Action<TArg> onCompleted)
        //    : base(action, onError, onCompleted)
        //{
        //}
    }
}
