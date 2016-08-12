using UnityEngine;
using System;

namespace Framework
{
    // Maps bindings to an input
    public abstract class InputMap : MonoBehaviour
    {
        #region Core Generic Bindings
        public const string Horizontal = "Horizontal";
        public const string Vertical = "Vertical";
        public const string Up = "Up";
        public const string Right = "Right";
        public const string Down = "Down";
        public const string Left = "Left";
        public const string ScrollUp = "ScrollUp";
        public const string ScrollDown = "ScrollDown";
        public const string Pause = "Pause";
        public const string LeftClick = "LeftClick";
        public const string RightClick = "RightClick";
        public const string MiddleClick = "MiddleClick";
        #endregion

        public event EventHandler<InputActionEvent> Trigger;

        protected void FireTrigger(InputActionEvent actionEvent)
        {
            if (Trigger != null)
                Trigger(this, actionEvent);
        }
    }
}
