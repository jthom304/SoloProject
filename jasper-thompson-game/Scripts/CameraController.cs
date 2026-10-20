using Godot;
using System;

public partial class CameraController : Camera2D
{
    [Export]
    private PlayerController _trackedObject; //the char obj being tracked

    [Export]
    private GameManager _gameManager; //game manager 

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

    /*
     Name: _Process
     Description: check for action to shoot projectile
     Parameters: doube delta
     Returns: None
    */
    public override void _Process(double delta)
    {
        Vector2 cameraPosition = this.Position;
        Vector2 playerPosition = _trackedObject.Position;

        this.Position = playerPosition;
    }
}