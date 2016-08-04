using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
    public class InputMapPC : InputMap
    {
        Dictionary<InputAction, KeyCode> bindings;

        public InputMapPC()
        {
            bindings = new Dictionary<InputAction, KeyCode>();
        }

        public void AddBinding(InputAction action, KeyCode key)
        {
            if (bindings.ContainsKey(action))
            {
                Debug.LogErrorFormat("InputMapPC: '{0}' is already bound to '{1}'", action, key);
            }
            else
            {
                bindings.Add(action, key);
            }
        }

        public void LateUpdate()
        {
            foreach (var kvp in bindings)
            {
                if (Input.anyKey)
                {
                    if (Input.GetKeyDown(kvp.Value))
                    {
                        var e = new InputActionEvent(kvp.Key, InputActionType.Down);
                        FireTrigger(e);
                    }

                    if (Input.GetKey(kvp.Value))
                    {
                        var e = new InputActionEvent(kvp.Key, InputActionType.Held);
                        FireTrigger(e);
                    }
                }

                if (Input.GetKeyUp(kvp.Value))
                {
                    var e = new InputActionEvent(kvp.Key, InputActionType.Up);
                    FireTrigger(e);
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                FireTrigger(new InputActionEvent(InputAction.ScrollUp, InputActionType.Down));
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                FireTrigger(new InputActionEvent(InputAction.ScrollDown, InputActionType.Down));
            }

            if (Input.GetMouseButtonDown(0))
                FireTrigger(new InputActionEvent(InputAction.MouseLeft, InputActionType.Down));

            if (Input.GetMouseButtonDown(1))
                FireTrigger(new InputActionEvent(InputAction.MouseRight, InputActionType.Down));
        }
    }
}