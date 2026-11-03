//Author Jacklitsch Editor Jasper
using Godot;
using System;

public partial class CameraController : Camera2D
{
    private PlayerController _trackedObject; //the char obj being tracked

    /*
    Name: _Ready
    Description: called when node enters scene tree for first time
    Parameters: None
    Returns: None
    */
    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
    }

    public void SetTarget(PlayerController playerTarget)
    {
        _trackedObject = playerTarget;

        if (_trackedObject == null)
        {
            GD.Print("CAMCNTRL: no trackable obj found");
            return;
        }

        GD.Print("Player found");
    }

    /*
     Name: _Process
     Description: check for action to shoot projectile
     Parameters: doube delta
     Returns: None
    */
    public override void _Process(double delta)
    {
        if (_trackedObject == null)
        {
            return;
        }
        Vector2 lastPosition = this.GlobalPosition;
        Vector2 desiredPosition = _trackedObject.GlobalPosition;

        this.GlobalPosition = desiredPosition;
    }

    public void SwitchZone(Area2D ncameraZone)
    {
        this.LimitLeft = (int)(ncameraZone.Position.X - ncameraZone.GetChild<CollisionShape2D>(0).GetShape().GetRect().Size.X / 4);
        this.LimitRight = (int)(ncameraZone.Position.X + ncameraZone.GetChild<CollisionShape2D>(0).GetShape().GetRect().Size.X / 4);
        this.LimitTop = (int)(ncameraZone.Position.Y - ncameraZone.GetChild<CollisionShape2D>(0).GetShape().GetRect().Size.X / 4);
        this.LimitBottom = (int)(ncameraZone.Position.Y + ncameraZone.GetChild<CollisionShape2D>(0).GetShape().GetRect().Size.X / 4);
    }
}