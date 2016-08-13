using UnityEngine;
using System.Collections.Generic;
using Framework;

public class HUD : GameEntity
{
    [SerializeField] private HUDWorldPopups _hudPopups;
    [SerializeField] private HUDTokens _hudTokens;

    private List<GameObject> _hudElements;

    protected override void OnInitialize()
    {
        _hudElements = new List<GameObject>();
        _hudElements.Add(_hudPopups.gameObject);
        _hudElements.Add(_hudTokens.gameObject);

        // Listen for the World to be spawned.
        var _worldListener = GameEntityManager.AddListener<World>();
        _worldListener.OnSpawn(WorldListener_Spawned);
        _worldListener.OnRemove(WorldListener_Removed);
    }

    private void WorldListener_Removed(World obj)
    {
        obj.TokensChanged -= UpdateTokenHUD;
    }

    private void WorldListener_Spawned(World world)
    {
        world.TokensChanged += UpdateTokenHUD;

        // Get existing Token Data.
        UpdateTokenHUD(world.TokenData);
    }

    private void UpdateTokenHUD(TokenData tokenData)
    {
        if (_hudTokens == null)
        {
            Debug.LogError("HUDTokens is null.");
        }
        else
        {
            _hudTokens.Collected = tokenData.Collected;
            _hudTokens.Total = tokenData.Total;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _hudElements.Clear();
    }
}
