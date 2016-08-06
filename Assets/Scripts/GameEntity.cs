using UnityEngine;
using Framework;

public interface IGameEntity
{
    void Initialize(Game game);
}

public class GameEntity : MonoBehaviourEx, IGameEntity
{
    protected Game Game { get; private set; }

    public int InstanceID
    {
        get { return GetInstanceID(); }
    }

    void IGameEntity.Initialize(Game game)
    {
        Game = game;
        Game.StateListener.StateChanged += OnStateChanged;

        OnInitialize();
    }

    protected virtual void OnInitialize() { }
    protected virtual void OnStateChanged(State state) { }

    protected virtual void OnDestroy()
    {
        Game.StateListener.StateChanged -= OnStateChanged;
    }
}
