using UnityEngine;
using Framework.UI;

public class UIMenuButton : UIButton
{
    public GameObject Active;

    public override void Awake()
    {
        base.Awake();
        Active.SetActive(false);
    }

    public override void Selected(bool selected)
    {
        base.Selected(selected);
        Active.SetActive(selected);
    }
}
