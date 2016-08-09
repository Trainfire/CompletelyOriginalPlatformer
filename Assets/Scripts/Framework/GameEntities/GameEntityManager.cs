using UnityEngine;
using System;
using System.Collections.Generic;

namespace Framework
{
    class GameEntityManager
    {
        public static event Action<GameEntity> EntitySpawned;
        public static event Action<GameEntity> EntityRemoved;

        private static List<GameEntity> _gameEntities;
        private static Game _game;

        public GameEntityManager(Game game)
        {
            _game = game;

            _gameEntities = new List<GameEntity>();

            game.ZoneListener.ZoneChanging += ZoneListener_ZoneChanging;
            game.ZoneListener.ZoneChanged += ZoneListener_ZoneChanged;

            InitializeEntities();
        }

        private void ZoneListener_ZoneChanged(GameZone obj)
        {
            InitializeEntities();
        }

        private void ZoneListener_ZoneChanging()
        {
            // GameEntities will be destroyed on level load, so just unhook the event here.
            _gameEntities.ForEach(x => x.Destroyed -= GameEntity_Destroyed);
            _gameEntities.Clear();
        }

        private void InitializeEntities()
        {
            // TODO: Get objects by IGameEntity instead of concrete class. Need to investigate how to do this.
            foreach (var gameEntity in GameObject.FindObjectsOfType<GameEntity>())
            {
                Register(gameEntity);
            }
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

                _gameEntities.Add(gameEntity);
            }
        }

        private static void Unregister(GameEntity gameEntity)
        {
            _gameEntities.Remove(gameEntity);
            gameEntity.Destroyed -= GameEntity_Destroyed;

            if (EntityRemoved != null)
                EntityRemoved(gameEntity);
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
    }
}