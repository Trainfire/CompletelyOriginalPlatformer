using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework
{
    class GameEntityManager
    {
        public static event Action<GameEntity> EntitySpawned;
        public static event Action<GameEntity> EntityRemoved;

        private static List<GameEntity> _gameEntities;
        private static List<IGameEntityListener> _listeners;
        private static Game _game;

        public GameEntityManager(Game game)
        {
            _game = game;
            _gameEntities = new List<GameEntity>();
            _listeners = new List<IGameEntityListener>();
        }

        public void LoadEntities()
        {
            // TODO: Get objects by IGameEntity instead of concrete class. Need to investigate how to do this.
            foreach (var gameEntity in GameObject.FindObjectsOfType<GameEntity>())
            {
                Register(gameEntity);
            }
        }

        public void Cleanup()
        {
            // Cleanup listeners.   
            _listeners.ForEach(x => x.Destroy());
            _listeners.Clear();

            // GameEntities will be destroyed on level load, so just unhook the event here.
            _gameEntities.ToList().ForEach(x => Unregister(x));
            _gameEntities.Clear();
        }

        private static void Register(GameEntity gameEntity)
        {
            if (_gameEntities.Contains(gameEntity))
            {
                Debug.LogErrorFormat("The GameEntity '{0}' has already been registered.", gameEntity.name);
            }
            else
            {
                // Temp until I can just pass in the interface.
                var gameEntityInterface = gameEntity as IGameEntity;
                gameEntityInterface.Initialize(_game);

                gameEntity.Destroyed += GameEntity_Destroyed;

                if (EntitySpawned != null)
                    EntitySpawned(gameEntity);

                _listeners.ForEach(x => x.OnSpawn(gameEntity));

                _gameEntities.Add(gameEntity);
            }
        }

        private static void Unregister(GameEntity gameEntity)
        {
            _gameEntities.Remove(gameEntity);
            gameEntity.Destroyed -= GameEntity_Destroyed;

            if (EntityRemoved != null)
                EntityRemoved(gameEntity);

            _listeners.ForEach(x => x.OnRemove(gameEntity));
        }

        public static T Spawn<T>(T original) where T : GameEntity
        {
            var instance = GameObject.Instantiate<GameEntity>(original);
            Register(instance);
            return instance as T;
        }

        private static void GameEntity_Destroyed(GameEntity gameEntity)
        {
            if (!_gameEntities.Contains(gameEntity))
            {
                Debug.LogError("A GameEntity was destroyed but it was never registered.");
            }
            else
            {
                Unregister(gameEntity);
            }
        }

        /// <summary>
        /// Adds a listener which will callback when a GameEntity of the specified type is Spawned or Removed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static GameEntityListener<T> AddListener<T>() where T : GameEntity
        {
            var gameEntityEvent = new GameEntityListener<T>();
            _listeners.Add(gameEntityEvent);
            return gameEntityEvent;
        }
    }
}
