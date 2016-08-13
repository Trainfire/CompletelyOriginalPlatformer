using UnityEngine;
using System;

namespace Framework
{
    public interface IGameEntity
    {
        void Initialize(Game game);
    }

    public class GameEntity : MonoBehaviourEx, IGameEntity, IInputHandler
    {
        public event Action<GameEntity> Destroyed;

        protected Game Game { get; private set; }

        public int InstanceID
        {
            get { return GetInstanceID(); }
        }

        void IGameEntity.Initialize(Game game)
        {
            Game = game;
            Game.StateListener.StateChanged += OnStateChanged;

            InputManager.RegisterHandler(this);

            OnInitialize();
        }

        protected virtual void OnInitialize() { }
        protected virtual void OnStateChanged(State state) { }
        public virtual void HandleInput(InputActionEvent action) { }

        /// <summary>
        /// Called when the GameObject is destroyed. If override, you must call the base method!
        /// </summary>
        protected virtual void OnDestroy()
        {
            InputManager.UnregisterHandler(this);

            if (Destroyed != null)
                Destroyed(this);

            if (Game != null)
                Game.StateListener.StateChanged -= OnStateChanged;
        }
    }
}
