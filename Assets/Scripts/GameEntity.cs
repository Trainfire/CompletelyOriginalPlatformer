using UnityEngine;
using Framework;

public interface IGameEntity
{
    void Initialize(Game game);
}

public class GameEntity : MonoBehaviourEx, IGameEntity
{
    protected Game Game { get; private set; }

    public void Initialize(Game game)
    {
        Game = game;
        OnInitialize();
    }

    protected virtual void OnInitialize() { }
}
