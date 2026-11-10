/*******************************************************************/
/*Filename: CameraController.cs
/ * Author: Prof Jacklitsch
/ * Editor: Jasper Thompson 
/ * Major: Game Dev/ Animation
/ * Date Created: 10/19/26
/ * Last Modified: 11/08/26
/ * Due Date: 10/28/26
/ * Course: Cpsc 320
/ * Professor Name: Prof. Jacklitsch
/ * Assignment: Project One - Solo Project
/ * Purpose: 
/*******************************************************************/
using Godot;
using System;

public partial class CameraController : Camera2D
{
    private PlayerController _trackedObject; //the char obj being tracked

    /*
    Name: _Ready
    Description: sets process mode
    Returns: None
    */
    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
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
            SetProcess(false);
            return;
        }
        SetGlobalPosition(_trackedObject.GlobalPosition);
    }

    /*
    Name: SetTarget
    Description: find player
    Parameters: PlayerController playerTarget
    Returns: None
    */
    public void SetTarget(PlayerController playerTarget)
    {
        ResetCamera(playerTarget);
    }

    /*
    Name: ResetCamera
    Description: set camera to follow player
    Parameters: PlayerController playerTarget
    Returns: None
    */
    public void ResetCamera(PlayerController playerTarget)
    {
        if (playerTarget == null)
        {
            GD.Print("Camera has no target");
            _trackedObject = null;
            SetProcess(false);
            return;
        }
        _trackedObject = playerTarget;
        SetProcess(true);

        GlobalPosition = _trackedObject.GlobalPosition;
        GD.Print("Camera reset to player: " + GlobalPosition);
    }

    /*
    Name: SwitchZone
    Description: sets camera limits based on camera zone
    Parameters: Area2D ncameraZone
    Returns: None
    */
    public void SwitchZone(Area2D ncameraZone)
    {
        this.LimitLeft = (int)(ncameraZone.GlobalPosition.X - ncameraZone.GetChild<CollisionShape2D>(0).GetShape().GetRect().Size.X / 2);
        this.LimitRight = (int)(ncameraZone.GlobalPosition.X + ncameraZone.GetChild<CollisionShape2D>(0).GetShape().GetRect().Size.X / 2);
        this.LimitTop = (int)(ncameraZone.GlobalPosition.Y - ncameraZone.GetChild<CollisionShape2D>(0).GetShape().GetRect().Size.Y / 2);
        this.LimitBottom = (int)(ncameraZone.GlobalPosition.Y + ncameraZone.GetChild<CollisionShape2D>(0).GetShape().GetRect().Size.Y / 2);
    }
}