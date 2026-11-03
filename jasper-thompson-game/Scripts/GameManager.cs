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
        Node2D spawnPoint =
    newLevel.FindChild("SpawnLocation", true, false) as Node2D;

        if (spawnPoint == null || PlayerRef == null)
        {
            GD.PrintErr("SpawnPoint or PlayerRef missing!");
            return;
        }

        GD.Print("Parent position: " + spawnPoint.GetParent<Node2D>().GlobalPosition);
        GD.Print("Spawn local: " + spawnPoint.Position);
        GD.Print("Spawn global: " + spawnPoint.GlobalPosition);

        PlayerRef.Velocity = Vector2.Zero;
        PlayerRef.GlobalPosition = spawnPoint.GlobalPosition;

        GD.Print("Player position: " + PlayerRef.GlobalPosition);
        //if (newLevel == null)
        //{
        //    GD.PrintErr("New level is null!");
        //    return;
        //}

        //if (PlayerRef == null)
        //{
        //    GD.PrintErr("PlayerRef is null!");
        //    return;
        //}

        //_spawnPoint = newLevel.GetNode<Node2D>("Portals/LeftPortal/SpawnLocation");

        //if (_spawnPoint == null)
        //{
        //    GD.PrintErr("SpawnLocation not found in new level!");
        //    GD.Print("New level name: " + newLevel.Name);
        //    return;
        //}
        ////stop movement
        //PlayerRef.Velocity = Vector2.Zero;
        //GD.Print("spawn found: " + _spawnPoint.GlobalPosition);
        //PlayerRef.GlobalPosition = _spawnPoint.GlobalPosition;
        //CameraController cameraObj = newLevel.FindChild("CameraController", true, false) as CameraController;
        //if (cameraObj != null)
        //{
        //    cameraObj.SetTarget(PlayerRef);
        //    GD.Print("Camera Found");
        //}
        //else
        //{
        //    GD.Print("No camera found");
        //}

        //    GD.Print("Player successfully moved to Level 2!" + PlayerRef.GlobalPosition);
    }
}
