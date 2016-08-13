using UnityEngine;
using System.Collections.Generic;
using Framework;

namespace Framework.UI
{
    public class ButtonList : MonoBehaviour
    {
        [SerializeField] private UIButton _prototype;

        private CyclicalList<UIButton> _buttons;
        private UIButton _last;

        private void Awake()
        {
            _buttons = new CyclicalList<UIButton>();
            _buttons.Moved += OnMove;

            _last = _buttons.Current;
            _last.Selected(true);
        }

        private void OnMove(object sender, CyclicalListEvent<UIButton> cycleEvent)
        {
            _last.Selected(false);
            cycleEvent.Data.Selected(true);
            _last = cycleEvent.Data;
        }

        public void Add(UIButton button)
        {
            _buttons.Add(button);
        }

        public void Prev()
        {
            _buttons.MovePrev();
        }

        public void Next()
        {
            _buttons.MoveNext();
        }
    }
}
