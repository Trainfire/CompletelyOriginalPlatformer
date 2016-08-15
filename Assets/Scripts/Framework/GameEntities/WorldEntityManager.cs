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

        private Dictionary<uint, IWorldEntity> _entities;
        private List<IWorldEntityListener> _listeners;
        private World _world;
        private StateListener _stateListener;

        public WorldEntityManager(World world, StateListener stateListener)
        {
            _world = world;
            _stateListener = stateListener;
            _entities = new Dictionary<uint, IWorldEntity>();
            _listeners = new List<IWorldEntityListener>();
        }

        public void RegisterAllEntities()
        {
            // Register each entity first. We'll trigger the spawn event later.
            foreach (var entity in InterfaceHelper.FindObjects<IWorldEntity>())
            {
                Register(entity, false);
            }

            // Trigger the spawn events now that we have all our entities registered.
            foreach (var entity in _entities)
            {
                _listeners.ForEach(x => x.OnSpawn(entity.Value));
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
                Unregister(entity.Value);
                _entities.Remove(entity.Key);
            }
        }

        private void Register(IWorldEntity worldEntity, bool triggerSpawnEvent = true)
        {
            if (_entities.ContainsKey(worldEntity.ID))
            {
                Debug.LogErrorFormat("The world entity '{0}' has already been registered.", worldEntity.ID);
            }
            else
            {
                worldEntity.Initialize(_world, _stateListener);
                worldEntity.Destroyed += WorldEntity_Destroyed;

                if (EntitySpawned != null)
                    EntitySpawned(worldEntity);

                if (triggerSpawnEvent)
                    _listeners.ForEach(x => x.OnSpawn(worldEntity));

                _entities.Add(worldEntity.ID, worldEntity);
            }
        }

        private void Unregister(IWorldEntity worldEntity)
        {
            _entities.Remove(worldEntity.ID);
            worldEntity.Destroyed -= WorldEntity_Destroyed;

            if (EntityRemoved != null)
                EntityRemoved(worldEntity);

            _listeners.ForEach(x => x.OnRemove(worldEntity));
        }

        private void WorldEntity_Destroyed(IWorldEntity worldEntity)
        {
            if (!_entities.ContainsKey(worldEntity.ID))
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
                .Values
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
