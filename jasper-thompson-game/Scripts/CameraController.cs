//Author Jacklitsch Editor Jasper
using Godot;
using System;

public partial class CameraController : Camera2D
{
    [Export]
    private PlayerController _trackedObject; //the char obj being tracked

    [Export]
    private GameManager _gameManager; //game manager 

    [Export]
    private Area2D _cameraZone;

    /*
    Name: _Ready
    Description: called when node enters scene tree for first time
    Parameters: None
    Returns: None
    */
    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;

        if (_trackedObject == null)
        {
            GD.Print("CAMCNTRL: no trackable obj found");
        }
    }

    /*
     Name: _Process
     Description: check for action to shoot projectile
     Parameters: doube delta
     Returns: None
    */
    public override void _Process(double delta)
    {
        Vector2 lastPosition = this.Position;
        Vector2 desiredPosition = _trackedObject.Position;

        this.Position = desiredPosition;
    }

    public void SwitchZone(Area2D ncameraZone)
    {
        this.LimitLeft = (int)(ncameraZone.Position.X - ncameraZone.GetChild<CollisionShape2D>(0).GetShape().GetRect().Size.X / 2);
        this.LimitRight = (int)(ncameraZone.Position.X + ncameraZone.GetChild<CollisionShape2D>(0).GetShape().GetRect().Size.X / 2);
        this.LimitTop = (int)(ncameraZone.Position.Y - ncameraZone.GetChild<CollisionShape2D>(0).GetShape().GetRect().Size.X / 2);
        this.LimitBottom = (int)(ncameraZone.Position.Y + ncameraZone.GetChild<CollisionShape2D>(0).GetShape().GetRect().Size.X / 2);
    }
}