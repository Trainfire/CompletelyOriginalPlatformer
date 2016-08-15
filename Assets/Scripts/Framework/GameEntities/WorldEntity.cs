using System;

namespace Framework
{
    public interface IWorldEntity
    {
        void Initialize(World world, StateListener stateListener);
    }

    public class WorldEntity : MonoBehaviourEx, IWorldEntity, IInputHandler
    {
        public event Action<WorldEntity> Destroyed;

        private StateListener _stateListener;

        protected World World { get; private set; }

        public int InstanceID
        {
            get { return GetInstanceID(); }
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
        protected virtual void OnStateChanged(State state) { }

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
