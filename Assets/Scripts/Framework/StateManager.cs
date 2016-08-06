using System;
using System.Collections.Generic;

namespace Framework
{
    public enum State
    {
        Running,
        Paused,
    }

    public interface IStateHandler
    {
        void OnStateChanged(State state);
    }

    public class StateManager 
    {
        private IStateHandler _handler;

        public State State { get; private set; }

        public StateManager(IStateHandler handler)
        {
            _handler = handler;
        }

        public void SetState(State state)
        {
            State = state;
            _handler.OnStateChanged(State);
        }

        public void ToggleState()
        {
            SetState(State == State.Paused ? State.Running : State.Paused);
        }
    }

    public class StateListener : IStateHandler
    {
        public event Action<State> StateChanged;

        void IStateHandler.OnStateChanged(State state)
        {
            if (StateChanged != null)
                StateChanged(state);
        }
    }
}
