using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework
{
    public class WorldEntityManager
    {
        public event Action<WorldEntity> EntitySpawned;
        public event Action<WorldEntity> EntityRemoved;

        private List<WorldEntity> _entities;
        private List<IWorldEntityListener> _listeners;
        private World _world;
        private StateListener _stateListener;

        public WorldEntityManager(World world, StateListener stateListener)
        {
            _world = world;
            _stateListener = stateListener;
            _entities = new List<WorldEntity>();
            _listeners = new List<IWorldEntityListener>();
        }

        public void RegisterAllEntities()
        {
            // TODO: Get objects by IGameEntity instead of concrete class. Need to investigate how to do this.
            foreach (var entity in GameObject.FindObjectsOfType<WorldEntity>())
            {
                Register(entity);
            }

            Debug.Log(_entities.Count);
        }

        public void Cleanup()
        {
            // Cleanup listeners.   
            _listeners.ForEach(x => x.Destroy());
            _listeners.Clear();

            // GameEntities will be destroyed on level load, so just unhook the event here.
            foreach (var entity in _entities.ToList())
            {
                Unregister(entity);
                _entities.Remove(entity);
            }
        }

        private void Register(WorldEntity worldEntity)
        {
            if (_entities.Contains(worldEntity))
            {
                Debug.LogErrorFormat("The GameEntity '{0}' has already been registered.", worldEntity.name);
            }
            else
            {
                // Temp until I can just pass in the interface.
                var worldEntityInterface = worldEntity as IWorldEntity;
                worldEntityInterface.Initialize(_world, _stateListener);

                worldEntity.Destroyed += WorldEntity_Destroyed;

                if (EntitySpawned != null)
                    EntitySpawned(worldEntity);

                _listeners.ForEach(x => x.OnSpawn(worldEntity));

                _entities.Add(worldEntity);
            }
        }

        private void Unregister(WorldEntity worldEntity)
        {
            _entities.Remove(worldEntity);
            worldEntity.Destroyed -= WorldEntity_Destroyed;

            if (EntityRemoved != null)
                EntityRemoved(worldEntity);

            _listeners.ForEach(x => x.OnRemove(worldEntity));
        }

        private void WorldEntity_Destroyed(WorldEntity worldEntity)
        {
            if (!_entities.Contains(worldEntity))
            {
                Debug.LogError("A GameEntity was destroyed but it was never registered.");
            }
            else
            {
                Unregister(worldEntity);
            }
        }

        public T Spawn<T>(T original) where T : WorldEntity
        {
            var instance = GameObject.Instantiate<WorldEntity>(original);
            Register(instance);
            return instance as T;
        }

        public T Get<T>() where T : WorldEntity
        {
            return _entities.FirstOrDefault(x => x.GetType() == typeof(T)) as T;
        }

        /// <summary>
        /// Adds a listener which will callback when a GameEntity of the specified type is Spawned or Removed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public WorldEntityListener<T> AddListener<T>() where T : WorldEntity
        {
            var worldEntityEvent = new WorldEntityListener<T>();
            _listeners.Add(worldEntityEvent);
            return worldEntityEvent;
        }
    }
}
