using UnityEngine;
using System.Collections.Generic;
using Framework;
using Framework.UI;

public class HUD : GameControllerDependant
{
    [SerializeField] private HUDWorldPopups _hudPopups;
    [SerializeField] private HUDTokens _hudTokens;

    private List<GameObject> _hudElements;
    private GameEntityListener<World> _worldListener;

    protected override void OnInitialize()
    {
        _hudElements = new List<GameObject>();
        _hudElements.Add(_hudPopups.gameObject);
        _hudElements.Add(_hudTokens.gameObject);

        // Listen for the World to be spawned.
        _worldListener = new GameEntityListener<World>();
        _worldListener.Spawned += WorldListener_Spawned;
        _worldListener.Removed += WorldListener_Removed;
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

    public void OnDestroy()
    {
        _worldListener.Destroy();
        _hudElements.Clear();
    }
}
