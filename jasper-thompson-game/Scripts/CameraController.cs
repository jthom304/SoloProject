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
        ResetCamera(playerTarget);
    }

    public void ResetCamera(PlayerController playerTarget)
    {
        _trackedObject = playerTarget;

        if (_trackedObject == null)
        {
            GD.Print("Camera has no target");
            return;
        }

        GlobalPosition = _trackedObject.GlobalPosition;
        GD.Print("Camera reset to player: " + GlobalPosition);
    }

    public void SwitchZone(Area2D ncameraZone)
    {
        this.LimitLeft = (int)(ncameraZone.GlobalPosition.X - ncameraZone.GetChild<CollisionShape2D>(0).GetShape().GetRect().Size.X / 2);
        this.LimitRight = (int)(ncameraZone.GlobalPosition.X + ncameraZone.GetChild<CollisionShape2D>(0).GetShape().GetRect().Size.X / 2);
        this.LimitTop = (int)(ncameraZone.GlobalPosition.Y - ncameraZone.GetChild<CollisionShape2D>(0).GetShape().GetRect().Size.Y / 2);
        this.LimitBottom = (int)(ncameraZone.GlobalPosition.Y + ncameraZone.GetChild<CollisionShape2D>(0).GetShape().GetRect().Size.Y / 2);
        CollisionShape2D collision =
            ncameraZone.GetChild<CollisionShape2D>(0);

        GD.Print("ZONE GLOBAL: " + ncameraZone.GlobalPosition);
        GD.Print("COLLISION LOCAL: " + collision.Position);
        GD.Print("COLLISION GLOBAL: " + collision.GlobalPosition);
        GD.Print("PLAYER GLOBAL: " + _trackedObject.GlobalPosition);
        Node current = ncameraZone.GetChild(0);

        while (current != null)
        {
            if (current is Node2D node)
            {
                GD.Print(
                    "NODE: " + node.Name +
                    " | Local: " + node.Position +
                    " | Global: " + node.GlobalPosition +
                    " | Scale: " + node.Scale
                );
            }

            current = current.GetParent();
        }
    }
}