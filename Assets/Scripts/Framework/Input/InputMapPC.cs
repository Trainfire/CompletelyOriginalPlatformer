using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
    public class InputMapPC : InputMap
    {
        private Dictionary<string, KeyCode> _bindings;
        private float _mouseX;
        private float _mouseY;

        public void Awake()
        {
            _bindings = new Dictionary<string, KeyCode>();
        }

        public void AddBinding(string action, KeyCode key)
        {
            if (_bindings.ContainsKey(action))
            {
                Debug.LogErrorFormat("InputMapPC: '{0}' is already bound to '{1}'", action, key);
            }
            else
            {
                _bindings.Add(action, key);
            }
        }

        public void LateUpdate()
        {
            foreach (var kvp in _bindings)
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

            _mouseX = Input.GetAxis("Mouse X");
            _mouseY = Input.GetAxis("Mouse Y");

            if (Mathf.Abs(_mouseX) > 0f)
                FireTrigger(new InputActionEvent(Horizontal, InputActionType.Axis, _mouseX));

            if (Mathf.Abs(_mouseY) > 0f)
                FireTrigger(new InputActionEvent(Vertical, InputActionType.Axis, _mouseY));

            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                FireTrigger(new InputActionEvent(ScrollUp, InputActionType.Down));

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                FireTrigger(new InputActionEvent(ScrollDown, InputActionType.Down));

            if (Input.GetMouseButtonDown(0))
                FireTrigger(new InputActionEvent(LeftClick, InputActionType.Down));

            if (Input.GetMouseButtonDown(1))
                FireTrigger(new InputActionEvent(RightClick, InputActionType.Down));
        }
    }
}
