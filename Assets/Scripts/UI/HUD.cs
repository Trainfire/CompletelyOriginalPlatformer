using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Framework;

public class HUD : GameEntity
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private HUDWorldPopups _hudPopups;
    [SerializeField] private HUDTokens _hudTokens;
    [SerializeField] private HUDInteractableAreas _hudInteractableArea;

    private TokenListener _tokenListener;
    private List<IHUDWorldElement> _hudElements;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        if (_canvas == null)
        {
            Debug.LogError("HUD must have a Canvas assigned.");
            return;
        }

        _hudElements = new List<IHUDWorldElement>();
        _hudElements.Add(_hudPopups);
        _hudElements.Add(_hudInteractableArea);
        _hudElements.ForEach(x => x.Initialize(_canvas));

        // Listen for the World to be spawned.
        _tokenListener = FindObjectOfType<TokenListener>();
        _tokenListener.TokensChanged += UpdateTokenHUD;

        // Get existing Token Data.
        UpdateTokenHUD(_tokenListener.TokenData);
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
