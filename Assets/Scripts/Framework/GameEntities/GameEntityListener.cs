using UnityEngine;
using System;

namespace Framework
{
    /// <summary>
    /// Listens for a particular type of GameEntity to be spawned and triggers a callback.
    /// Make sure you destroy it's no longer needed!
    /// </summary>
    class GameEntityListener<TTargetType> where TTargetType : GameEntity
    {
        public event Action<TTargetType> Spawned;
        public event Action<TTargetType> Removed;

        public GameEntityListener()
        {
            GameEntityManager.EntitySpawned += GameEntityManager_EntitySpawned;
            GameEntityManager.EntityRemoved += GameEntityManager_EntityRemoved;
        }

        private void GameEntityManager_EntitySpawned(GameEntity gameEntity)
        {
            var targetType = gameEntity as TTargetType;
            if (targetType != null)
                Spawned.InvokeSafe(targetType);
        }

        private void GameEntityManager_EntityRemoved(GameEntity gameEntity)
        {
            var targetType = gameEntity as TTargetType;
            if (targetType != null)
                Removed.InvokeSafe(targetType);
        }

        public void Destroy()
        {
            GameEntityManager.EntitySpawned -= GameEntityManager_EntitySpawned;
            GameEntityManager.EntityRemoved -= GameEntityManager_EntityRemoved;
        }
    }
}
