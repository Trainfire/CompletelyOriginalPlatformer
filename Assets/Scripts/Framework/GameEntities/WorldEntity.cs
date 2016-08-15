using System;

namespace Framework
{
    public interface IWorldEntity
    {
        event Action<IWorldEntity> Destroyed;
        uint ID { get; }
        void Initialize(World world, StateListener stateListener);
    }

    public abstract class WorldEntity : MonoBehaviourEx, IWorldEntity, IInputHandler
    {
        public event Action<IWorldEntity> Destroyed;

        private StateListener _stateListener;

        protected World World { get; private set; }

        uint IWorldEntity.ID
        {
            get { return (uint)GetInstanceID(); }
        }

        void IWorldEntity.Initialize(World world, StateListener stateListener)
        {
            World = world;

            _stateListener = stateListener;
            _stateListener.StateChanged += OnStateChanged;

            InputManager.RegisterHandler(this);

            OnInitialize();
        }

        protected virtual void OnInitialize() { }
        protected virtual void HandleInput(InputActionEvent action) { }
        protected virtual void OnStateChanged(State state)
        {
            enabled = state == State.Running;
        }

        /// <summary>
        /// Called when the GameObject is destroyed. If override, you must call the base method!
        /// </summary>
        protected virtual void OnDestroy()
        {
            InputManager.UnregisterHandler(this);

            if (_stateListener != null)
                _stateListener.StateChanged -= OnStateChanged;

            if (Destroyed != null)
                Destroyed(this);
        }

        void IInputHandler.HandleInput(InputActionEvent action)
        {
            HandleInput(action);
        }
    }
}
