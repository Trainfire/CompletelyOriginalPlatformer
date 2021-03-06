using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Framework;
using System;
using Framework.UI;
using UnityEngine.Events;

namespace Framework.UI
{
    public class ListNavigation : MonoBehaviour, IInputHandler
    {
        public event UnityAction<UIDataViewList> Focused;
        public event UnityAction<UIDataViewList> Unfocused;

        private InputHoldBehaviour holdBehaviourDown;
        private InputHoldBehaviour holdBehaviourUp;
        private List<DataViewList> lists;
        private int index;

        public void Awake()
        {
            lists = new List<DataViewList>();

            holdBehaviourDown = new InputHoldBehaviour(InputMap.Down);
            holdBehaviourDown.OnTrigger += HoldBehaviourDown_OnTrigger;

            holdBehaviourUp = new InputHoldBehaviour(InputMap.Up);
            holdBehaviourUp.OnTrigger += HoldBehaviourUp_OnTrigger;
        }

        public void Register(DataViewList list)
        {
            if (lists.Contains(list))
            {
                Debug.LogErrorFormat("List '{0}' is already registered.", list.DataView.name);
            }
            else
            {
                lists.Add(list);
            }
        }

        public void Unregister(DataViewList list)
        {
            if (!lists.Contains(list))
            {
                Debug.LogErrorFormat("List '{0}' has not been registered.", list.DataView.name);
            }
            else
            {
                lists.Remove(list);
            }
        }

        public void Focus(DataViewList list)
        {
            if (!lists.Contains(list))
            {
                Debug.LogWarningFormat("Cannot focus on list '{0}' as it has not been registered. Call Register first.", list.DataView.name);
            }
            else
            {
                index = lists.IndexOf(list);
                Focus(index);
            }
        }

        public void Focus(int index)
        {
            if (index < 0 || index > lists.Count - 1)
            {
                Debug.LogError("Index is out of range.");
            }
            else
            {
                if (Unfocused != null)
                    Unfocused(lists[this.index].DataView);

                var focusable = lists[this.index].DataView.GetComponent<Focusable>();
                if (focusable != null)
                    focusable.Unfocus();

                this.index = index;

                focusable = lists[this.index].DataView.GetComponent<Focusable>();
                if (focusable != null)
                    focusable.Focus();

                if (Focused != null)
                    Focused(lists[this.index].DataView);
            }
        }

        public void HandleInput(InputActionEvent action)
        {
            if (lists.Count == 0)
                return;

            holdBehaviourDown.HandleInput(action);
            holdBehaviourUp.HandleInput(action);

            if (action.Type == InputActionType.Down)
            {
                switch (action.Action)
                {
                    case InputMap.Up:
                        lists[index].MovePrev();
                        break;
                    case InputMap.Down:
                        lists[index].MoveNext();
                        break;
                    case InputMap.ScrollUp:
                        lists[index].MovePrev();
                        break;
                    case InputMap.ScrollDown:
                        lists[index].MoveNext();
                        break;
                    case InputMap.Right:
                        FocusNext();
                        break;
                    case InputMap.Left:
                        FocusPrev();
                        break;
                    default:
                        break;
                }
            }
        }

        private void HoldBehaviourUp_OnTrigger()
        {
            lists[index].MovePrev();
        }

        private void HoldBehaviourDown_OnTrigger()
        {
            lists[index].MoveNext();
        }

        private void FocusPrev()
        {
            if ((index - 1) >= 0)
            {
                int nextIndex = index - 1;
                Focus(nextIndex);
            }
        }

        private void FocusNext()
        {
            if ((index + 1) < lists.Count)
            {
                int nextIndex = index + 1;
                Focus(nextIndex);
            }
        }

        public void OnDestroy()
        {
            holdBehaviourDown.OnTrigger -= HoldBehaviourDown_OnTrigger;
            holdBehaviourUp.OnTrigger -= HoldBehaviourUp_OnTrigger;

            holdBehaviourDown.Destroy();
            holdBehaviourUp.Destroy();
        }
    }
}
