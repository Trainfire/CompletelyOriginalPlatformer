using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Framework;

public class HUD : WorldEntity
{
    [SerializeField] private HUDWorldPopups _hudPopups;
    [SerializeField] private HUDTokens _hudTokens;
    [SerializeField] private HUDInteractableAreas _hudInteractableArea;

    private List<IHUDWorldElement> _hudElements;

    protected override void OnInitialize()
    {
        _hudElements = new List<IHUDWorldElement>();
        _hudElements.Add(_hudPopups);
        _hudElements.Add(_hudInteractableArea);
        _hudElements.ForEach(x => x.Initialize(World.Entities));

        // Listen for the World to be spawned.
        var _worldListener = World.Entities.AddListener<TokenListener>();
        _worldListener.OnSpawn(TokenListener_Spawned);
        _worldListener.OnRemove(TokenListener_Removed);
    }

    private void TokenListener_Removed(TokenListener obj)
    {
        obj.TokensChanged -= UpdateTokenHUD;
    }

    private void TokenListener_Spawned(TokenListener world)
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
