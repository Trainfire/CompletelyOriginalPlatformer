using UnityEngine;
using System;
using System.Collections.Generic;
using Framework;

public struct TokenData
{
    public int Collected { get; set; }
    public int Total { get; set; }
}

class World : GameEntity
{
    public event Action<TokenData> TokensChanged;

    private TrackingCamera2D _trackingCamera;
    private List<GameEntity> _tokens;
    private TokenData _tokenData;
    private HUD _hud;

    public TokenData TokenData
    {
        get { return _tokenData; }
    }

    protected override void OnInitialize()
    {
        var player = FindObjectOfType<Player>();

        // Find the tracking camera which causes the camera to follow a target.
        _trackingCamera = FindObjectOfType<TrackingCamera2D>();
        _trackingCamera.SetTarget(player.gameObject);

        // Set our camera's controller so it starts tracking the player.
        Game.Camera.SetController(_trackingCamera);

        // Find all the tokens.
        _tokens = new List<GameEntity>();
        foreach (var token in FindObjectsOfType<Token>())
        {
            _tokens.Add(token);
            token.Collected += Token_Collected;
        }
        
        // Cache how many tokens there were at level start.
        _tokenData.Total = _tokens.Count;

        // For those who care...
        OnTokensChanged();
    }

    private void Token_Collected(Token token)
    {
        // Cleanup.
        token.Collected -= Token_Collected;
        _tokens.Remove(token);
        Destroy(token.gameObject);

        _tokenData.Collected++;

        // Tell anything that's interested.
        OnTokensChanged();

        if (_tokenData.Collected == _tokenData.Total)
        {
            Game.GameController.LoadMainMenu();
        }
    }

    private void OnTokensChanged()
    {
        TokensChanged.InvokeSafe(_tokenData);
    }
}
