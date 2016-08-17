﻿using UnityEngine;
using System;
using Framework;

public class InteractableArea : WorldEntity, IInputHandler
{
    public event Action<InteractableArea> Entered;
    public event Action<InteractableArea> Left;

    [SerializeField] private string _message;
    [SerializeField] private Trigger _trigger;
    [SerializeField] private AreaTrigger2D _triggerArea;

    private bool _playerInArea;

    public string Message
    {
        get { return _message; }
    }

    private void Awake()
    {
        InputManager.RegisterHandler(this);

        _triggerArea.Entered += OnTriggerEntered;
        _triggerArea.Left += OnTriggerLeft;
    }

    private void OnTriggerEntered(AreaTrigger2DEvent arg)
    {
        if (arg.Collider.tag == "Player")
        {
            Entered.InvokeSafe(this);
            _playerInArea = true;
        }
    }

    private void OnTriggerLeft(AreaTrigger2DEvent arg)
    {
        if (arg.Collider.tag == "Player")
        {
            Left.InvokeSafe(this);
            _playerInArea = false;
        }
    }

    void IInputHandler.HandleInput(InputActionEvent action)
    {
        if (_playerInArea && action.IsInteract())
        {
            Debug.Log("Fire Trigger");
            _trigger.Fire();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        InputManager.UnregisterHandler(this);

        _triggerArea.Entered -= OnTriggerEntered;
        _triggerArea.Left -= OnTriggerLeft;
    }
}