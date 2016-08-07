using UnityEngine;
using System;
using Framework;

public class WorldPopup : GameEntity
{
    public event Action<WorldPopup> TriggerEnter;
    public event Action<WorldPopup> TriggerLeave;

    public string Content;

    private BoxCollider2D _trigger;

    protected override void OnInitialize()
    {
        _trigger = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D()
    {
        TriggerEnter.InvokeSafe(this);
    }

    private void OnTriggerExit2D()
    {
        TriggerLeave.InvokeSafe(this);
    }
}
