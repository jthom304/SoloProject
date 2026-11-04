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
        //this.Zoom = new Vector2(1.5f, 1.5f);
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
            GD.Print("Camera has no target!");
            return;
        }
        SetGlobalPosition(_trackedObject.GlobalPosition);
    }

    public void SetTarget(PlayerController playerTarget)
    {
        _trackedObject = playerTarget;

        if (_trackedObject == null)
        {
            GD.Print("CAMCNTRL: no trackable obj found");
            return;
        }
        this.GlobalPosition = _trackedObject.GlobalPosition;
    }

    public void SwitchZone(Area2D ncameraZone)
    {
        this.LimitLeft = (int)(ncameraZone.GlobalPosition.X - ncameraZone.GetChild<CollisionShape2D>(0).GetShape().GetRect().Size.X / 2);
        this.LimitRight = (int)(ncameraZone.GlobalPosition.X + ncameraZone.GetChild<CollisionShape2D>(0).GetShape().GetRect().Size.X / 2);
        this.LimitTop = (int)(ncameraZone.GlobalPosition.Y - ncameraZone.GetChild<CollisionShape2D>(0).GetShape().GetRect().Size.Y / 2);
        this.LimitBottom = (int)(ncameraZone.GlobalPosition.Y + ncameraZone.GetChild<CollisionShape2D>(0).GetShape().GetRect().Size.Y / 2);
        GD.Print("ZOne pos: " + ncameraZone.GlobalPosition);
        GD.Print("Cam pos: " + this.GlobalPosition);
        GD.Print("Play pos: " + _trackedObject.GlobalPosition);
    }
}