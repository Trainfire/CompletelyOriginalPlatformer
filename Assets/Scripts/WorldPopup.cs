using UnityEngine;

public class WorldPopup : GameEntity
{
    public string Content;

    private BoxCollider2D _trigger;

    protected override void OnInitialize()
    {
        _trigger = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D()
    {
        Debug.Log("Trigger Entered");
        Game.UI.HUD.ShowPopup(this, transform.position);
    }

    private void OnTriggerExit2D()
    {
        Debug.Log("Trigger Left");
        Game.UI.HUD.HidePopup();
    }
}
