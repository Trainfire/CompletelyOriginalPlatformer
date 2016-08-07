using System;

namespace Framework
{
    /// <summary>
    /// Listens for a particular type of GameEntity to be spawned and triggers a callback.
    /// </summary>
    class GameEntityListener<TTargetType> : IDisposable where TTargetType : GameEntity
    {
        public event Action<TTargetType> Spawned;

        public GameEntityListener()
        {
            GameEntityManager.EntitySpawned += GameEntityManager_EntitySpawned;
        }

        private void GameEntityManager_EntitySpawned(GameEntity gameEntity)
        {
            var targetType = gameEntity as TTargetType;
            if (targetType != null)
                Spawned.InvokeSafe(targetType);
        }

        public void Destroy()
        {
            GameEntityManager.EntitySpawned -= GameEntityManager_EntitySpawned;
        }
    }
}
