using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
    // The type of action
    public enum InputActionType
    {
        None,
        Down,
        Up,
        Held,
    }

    public interface IInputHandler
    {
        void HandleInput(InputActionEvent action);
    }

    public class InputActionEvent : EventArgs
    {
        public string Action { get; private set; }
        public InputActionType Type { get; private set; }

        public InputActionEvent(string action, InputActionType type)
        {
            Action = action;
            Type = type;
        }
    }

    // Handles input from an input map and relays to a handler
    public static class InputManager
    {
        private static List<IInputHandler> handlers;
        private static List<InputMap> maps;

        static InputManager()
        {
            handlers = new List<IInputHandler>();
            maps = new List<InputMap>();
        }

        public static void RegisterHandler(IInputHandler handler)
        {
            if (!handlers.Contains(handler))
                handlers.Add(handler);
        }

        public static void UnregisterHandler(IInputHandler handler)
        {
            if (handlers.Contains(handler))
                handlers.Remove(handler);
        }

        public static void RegisterMap(InputMap inputMap)
        {
            if (!maps.Contains(inputMap))
            {
                maps.Add(inputMap);
                inputMap.Trigger += Relay;
            }
        }

        public static void UnregisterMap(InputMap inputMap)
        {
            if (maps.Contains(inputMap))
            {
                maps.Remove(inputMap);
                inputMap.Trigger -= Relay;
            }
        }

        static void Relay(object sender, InputActionEvent action)
        {
            handlers.ForEach(x => x.HandleInput(action));
        }
    }
}
