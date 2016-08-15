using System;

namespace Framework
{
    public interface IWorldEntityListener
    {
        void OnSpawn(WorldEntity gameEntity);
        void OnRemove(WorldEntity gameEntity);
        void Destroy();
    }

    public class WorldEntityListener<T> : IWorldEntityListener where T : WorldEntity
    {
        private Action<T> _spawned;
        private Action<T> _removed;

        public void OnSpawn(Action<T> onSpawn)
        {
            _spawned = onSpawn;
        }

        public void OnRemove(Action<T> onRemove)
        {
            _removed = onRemove;
        }

        void IWorldEntityListener.OnSpawn(WorldEntity worldEntity)
        {
            if (MatchesType(worldEntity))
                _spawned.InvokeSafe(worldEntity as T);
        }

        void IWorldEntityListener.OnRemove(WorldEntity worldEntity)
        {
            if (MatchesType(worldEntity))
                _removed.InvokeSafe(worldEntity as T);
        }

        bool MatchesType(WorldEntity worldEntity)
        {
            return worldEntity as T != null;
        }

        void IWorldEntityListener.Destroy()
        {
            _spawned = null;
            _removed = null;
        }
    }
}
