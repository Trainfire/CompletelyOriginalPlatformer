using UnityEngine;
using Framework;
using Framework.UI;

public class MenuBase : GameEntity, IInputHandler
{
    [SerializeField]
    private ButtonList _buttons;

    protected ButtonList Buttons
    {
        get { return _buttons; }
    }

    public override void HandleInput(InputActionEvent action)
    {
        if (action.Type == InputActionType.Down)
        {
            if (action.Action == InputMap.Up)
                _buttons.Prev();

            if (action.Action == InputMap.Down)
                _buttons.Next();
        }
    }
}
