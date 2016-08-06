using System.Collections;
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
        private List<IStateHandler> listeners;

        public State State { get; private set; }

        public StateManager()
        {
            listeners = new List<IStateHandler>();
        }

        public void RegisterListener(IStateHandler listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(IStateHandler listener)
        {
            listeners.Remove(listener);
        }

        public void SetState(State state)
        {
            State = state;
            listeners.ForEach(x => x.OnStateChanged(state));
        }

        public void ToggleState()
        {
            SetState(State == State.Paused ? State.Running : State.Paused);
        }
    }
}
