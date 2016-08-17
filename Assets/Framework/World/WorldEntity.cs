using System;

namespace Framework
{
    public interface IWorldEntity
    {
        event Action<IWorldEntity> Destroyed;
        void Initialize(World world, StateListener stateListener);
    }

    public abstract class WorldEntity : MonoBehaviourEx, IWorldEntity
    {
        public event Action<IWorldEntity> Destroyed;

        private StateListener _stateListener;

        protected World World { get; private set; }

        void IWorldEntity.Initialize(World world, StateListener stateListener)
        {
            World = world;

            _stateListener = stateListener;
            _stateListener.StateChanged += OnStateChanged;

            OnInitialize();
        }

        protected virtual void OnInitialize() { }
        protected virtual void OnStateChanged(State state)
        {
            enabled = state == State.Running;
        }

        /// <summary>
        /// Called when the GameObject is destroyed. If override, you must call the base method!
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (_stateListener != null)
                _stateListener.StateChanged -= OnStateChanged;

            if (Destroyed != null)
                Destroyed(this);
        }
    }
}
