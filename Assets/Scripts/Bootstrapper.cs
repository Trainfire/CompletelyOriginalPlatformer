using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using Framework;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private UI _ui;

    private Game _game;
    private Data _data;
    private MonoEventRelay _relay;

    public void Awake()
    {
        var existingBootstrapper = FindObjectOfType<Bootstrapper>();
        if (existingBootstrapper != null && existingBootstrapper != this)
        {
            Debug.LogError("A bootstrapper already exists in the scene...");
            Destroy(gameObject);
        }

        _relay = gameObject.GetOrAddComponent<MonoEventRelay>();
    }

    public void Start()
    {
        Assert.IsNotNull(_ui);

        DontDestroyOnLoad(this);
        DontDestroyOnLoad(_ui);

        _data = new Data();
        _game = gameObject.GetOrAddComponent<Game>();
        _game.Initialize(_data, _ui);
    }
}
