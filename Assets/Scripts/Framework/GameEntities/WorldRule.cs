namespace Framework
{
    /// <summary>
    /// A type of behaviour associated with a World. Attached to a World and this will be automatically invoked.
    /// Useful for specifying game rules.
    /// </summary>
    abstract class WorldRule : MonoBehaviourEx
    {
        protected GameController GameController { get; private set; }
        protected WorldEntityManager EntityManager { get; private set; }

        public void Initialize(GameController gameController, WorldEntityManager entityManager)
        {
            GameController = gameController;
            EntityManager = entityManager;
            OnInitialize();
        }

        protected virtual void OnInitialize() { }
    }
}