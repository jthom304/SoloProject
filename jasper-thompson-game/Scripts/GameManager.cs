using Godot;
using System;

public partial class GameManager : Node2D
{
    [Export]
    private LoadManager _loadManger;

    [Export]
    private Node2D _zoneContainer;

    [Export]
    private PlayerController _playerController;

    [Export]
    private CameraController _cameraController;

    [Export]
    private RunTimer _runTimer;

    //private Mapzone _activeZone;

    private Node2D _spawnPoint;

    /*
       Name: CheckTimer
       Description: checks to see if win condition is satisfied
       Parameters: float timeLeft
       Returns: None
   */
    public void CheckTimer(float timeLeft) //float timeCount is the amount of time the timer will count down from
    {
        if (timeLeft <= 0) //if time runs out
        {
            //show win menu
            //pause game 
            this.QueueFree();
            return;
        }
    }
}
