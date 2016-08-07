using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Framework;

class GameEntityManager
{
    private List<GameEntity> _gameEntities;

    private Game _game;

    public GameEntityManager(Game game)
    {
        _game = game;

        _gameEntities = new List<GameEntity>();

        LevelManager.LevelLoaded += LevelManager_LevelLoaded;
        LevelManager.LevelUnloaded += LevelManager_LevelUnloaded;
    }

    public void Initialize()
    {
        InitializeEntities();
    }

    private void InitializeEntities()
    {
        foreach (var gameEntity in GameObject.FindObjectsOfType<GameEntity>())
        {
            _gameEntities.Add(gameEntity);
        }

        _gameEntities
            .Cast<IGameEntity>()
            .ToList()
            .ForEach(x => x.Initialize(_game));
    }

    private void CleanupEntities()
    {
        _gameEntities.ForEach(x => GameObject.Destroy(x.gameObject));
        _gameEntities.Clear();
    }

    private void LevelManager_LevelUnloaded(LevelManager.LevelLoadEvent levelLoadEvent)
    {
        CleanupEntities();
    }

    private void LevelManager_LevelLoaded(LevelManager.LevelLoadEvent levelLoadEvent)
    {
        InitializeEntities();
    }
}