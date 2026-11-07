using Godot;
using System;

public partial class CameraZone : Area2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        BodyEntered += PlayerEntered;
    }

    public void PlayerEntered(Node2D playerBody)
    {
        if (playerBody is PlayerController playerObj && playerObj == LevelLoader.Instance.GameManagerRef.PlayerRef)
        {
            ActivateZone();
        }
    }

    public void ActivateZone()
    {
        CameraController cameraBody = LevelLoader.Instance.GameManagerRef.CameraObj;

        if (cameraBody == null)
        {
            GD.Print("No camera");
            return;
        }
        cameraBody.SwitchZone(this);
        GD.Print("Camera Zone Activated: " + this.Name);
    }
}
