using Framework;

public sealed class World : GameEntity
{
    public WorldEntityManager Entities { get; private set; }

    protected override void OnInitialize()
    {
        Entities = new WorldEntityManager(this, Game.StateListener);
        Entities.RegisterAllEntities();

        foreach (var worldBehaviour in GetComponents<WorldRule>())
        {
            worldBehaviour.Initialize(Game.Controller, Entities);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (Entities != null)
            Entities.Cleanup();
    }
}
