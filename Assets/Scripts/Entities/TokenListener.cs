﻿using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using Framework;

public struct TokenData
{
    public int Collected { get; set; }
    public int Total { get; set; }
}

class TokenListener : MonoBehaviour
{
    public UnityAction<TokenListener> AllCollected;
    public event Action<TokenData> TokensChanged;

    private List<Token> _tokens;
    private TokenData _tokenData;

    public TokenData TokenData
    {
        get { return _tokenData; }
    }

    private void Start()
    {
        // Find all the tokens.
        _tokens = new List<Token>();
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
            AllCollected.Invoke(this);
    }

    private void OnTokensChanged()
    {
        TokensChanged.InvokeSafe(_tokenData);
    }
}
