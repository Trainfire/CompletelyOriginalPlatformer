using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
    [System.Serializable]
    public class InputBinding
    {
        [SerializeField] private InputAction _action;
        [SerializeField] private KeyCode _key;

        public InputAction Action
        {
            get { return _action; }
        }

        public KeyCode Key
        {
            get { return _key; }
        }
    }

    public class InputMapPC : InputMap
    {
        [SerializeField] private List<InputBinding> _bindings;

        private Dictionary<InputAction, KeyCode> _bindingsDict;

        public void Awake()
        {
            _bindingsDict = new Dictionary<InputAction, KeyCode>();
            _bindings.ForEach(x => _bindingsDict.Add(x.Action, x.Key));
        }

        public void AddBinding(InputAction action, KeyCode key)
        {
            if (_bindingsDict.ContainsKey(action))
            {
                Debug.LogErrorFormat("InputMapPC: '{0}' is already bound to '{1}'", action, key);
            }
            else
            {
                _bindingsDict.Add(action, key);
            }
        }

        public void LateUpdate()
        {
            foreach (var kvp in _bindingsDict)
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