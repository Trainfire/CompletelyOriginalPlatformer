using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework
{
    public class WorldEntityManager
    {
        public event Action<IWorldEntity> EntitySpawned;
        public event Action<IWorldEntity> EntityRemoved;

        private List<IWorldEntity> _entities;
        private List<IWorldEntityListener> _listeners;
        private World _world;
        private StateListener _stateListener;

        public WorldEntityManager(World world, StateListener stateListener)
        {
            _world = world;
            _stateListener = stateListener;
            _entities = new List<IWorldEntity>();
            _listeners = new List<IWorldEntityListener>();
        }

        public void RegisterAllEntities()
        {
            // Register each entity first.
            foreach (var entity in InterfaceHelper.FindObjects<IWorldEntity>())
            {
                Register(entity, false);
            }

            // Now we'll initialize each entity.
            _entities.ForEach(e => e.Initialize(_world, _stateListener));

            // Finally, we'll trigger the spawn events now that we have all our entities registered.
            foreach (var entity in _entities)
            {
                _listeners.ForEach(x => x.OnSpawn(entity));
            }
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

        private void Register(IWorldEntity worldEntity, bool initialize = true)
        {
            if (_entities.Contains(worldEntity))
            {
                Debug.LogErrorFormat("The world entity '{0}' has already been registered.", worldEntity.GetHashCode());
            }
            else
            {
                if (initialize)
                    worldEntity.Initialize(_world, _stateListener);

                worldEntity.Destroyed += WorldEntity_Destroyed;

                if (EntitySpawned != null)
                    EntitySpawned(worldEntity);

                if (initialize)
                    _listeners.ForEach(x => x.OnSpawn(worldEntity));

                _entities.Add(worldEntity);
            }
        }

        private void Unregister(IWorldEntity worldEntity)
        {
            _entities.Remove(worldEntity);
            worldEntity.Destroyed -= WorldEntity_Destroyed;

            if (EntityRemoved != null)
                EntityRemoved(worldEntity);

            _listeners.ForEach(x => x.OnRemove(worldEntity));
        }

        private void WorldEntity_Destroyed(IWorldEntity worldEntity)
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
            var entity = _entities
                .FirstOrDefault(x => x.GetType() == typeof(T));

            if (entity == null)
            {
                Debug.LogErrorFormat("Failed to find an Entity of type '{0}'", typeof(T));
                return null;
            }
            else
            {
                return entity as T;
            }
        }

        /// <summary>
        /// Adds a listener which will callback when a WorldEntity of the specified type is Spawned or Removed.
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
