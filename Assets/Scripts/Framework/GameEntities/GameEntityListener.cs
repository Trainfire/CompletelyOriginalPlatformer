using System;

namespace Framework
{
    public interface IGameEntityListener
    {
        void OnSpawn(GameEntity gameEntity);
        void OnRemove(GameEntity gameEntity);
        void Destroy();
    }

    public class GameEntityListener<T> : IGameEntityListener where T : GameEntity
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

        void IGameEntityListener.OnSpawn(GameEntity gameEntity)
        {
            if (MatchesType(gameEntity))
                _spawned.InvokeSafe(gameEntity as T);
        }

        void IGameEntityListener.OnRemove(GameEntity gameEntity)
        {
            if (MatchesType(gameEntity))
                _removed.InvokeSafe(gameEntity as T);
        }

        bool MatchesType(GameEntity gameEntity)
        {
            return gameEntity as T != null;
        }

        void IGameEntityListener.Destroy()
        {
            _spawned = null;
            _removed = null;
        }
    }
}
