using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework;

class World : GameEntity
{
    private TrackingCamera2D _trackingCamera;

    protected override void OnInitialize()
    {
        var player = FindObjectOfType<Player>();

        _trackingCamera = FindObjectOfType<TrackingCamera2D>();
        _trackingCamera.SetTarget(player.gameObject);

        Game.Camera.SetController(_trackingCamera);
    }
}