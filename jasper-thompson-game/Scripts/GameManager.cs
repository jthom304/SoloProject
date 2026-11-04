using Godot;
using System;

public partial class GameManager : Node2D
{
    [Export]
    private LoadManager _loadManger;

    [Export]
    private Node2D _zoneContainer;

    [Export]
    private RunTimer _runTimer;

    //private Mapzone _activeZone;

    private Node2D _spawnPoint;

    public PlayerController PlayerRef { get; private set; }

    public CameraController CameraObj { get; private set; }


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

    public void FindPlayer(Node newLevel)
    {
        PlayerRef = newLevel.FindChild("PlayerChar", true, false) as PlayerController; //taken 11/2/26 https://docs.godotengine.org/en/stable/classes/class_node.html?utm_source=chatgpt.com#class-node-method-find-child
                                                                                             //basically goes through tree order looking to see if a node is a child and if owned
        if (PlayerRef == null)
        {
            GD.Print("Player Not Found");
            return;
        }
        GD.Print("Player found");
        //move player outside of level as to not delete it in the load
        PlayerRef.Reparent(GetParent(), true);
        GD.Print("Player transition safe");
    }

    public void LoadNextLevel(string scenePath)
    {
        LevelLoader.Instance.SwitchScene(scenePath);
    }

    public void MovePlayer(Node newLevel)
    {
        Node2D spawnPoint = newLevel.FindChild("SpawnLocation", true, false) as Node2D;

        if (spawnPoint == null || PlayerRef == null)
        {
            GD.PrintErr("SpawnPoint or PlayerRef missing!");
            return;
        }

        PlayerRef.IsTransitioning = true;
        PlayerRef.Velocity = Vector2.Zero;
        PlayerRef.GlobalPosition = spawnPoint.GlobalPosition;
        PlayerRef.Velocity = Vector2.Zero;
        PlayerRef.IsTransitioning = false;
    }

    public void FindCamera(Node newLevel)
    {
        CameraObj = newLevel.FindChild("Camera2D", true, false) as CameraController;

        if (CameraObj == null)
        {
            GD.Print("camera null");
            return;
        }
        if (PlayerRef == null)
        {
            GD.Print("Player null");
            return;
        }
        CameraObj.Reparent(GetParent(), true);
        CameraObj.SetTarget(PlayerRef);
        CameraObj.MakeCurrent(); //makes sure this camera is active in viewport
    }
}
