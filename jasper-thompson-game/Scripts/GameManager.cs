/*******************************************************************/
/*Filename: GameManager.cs
/ * Author: Jasper Thompson
/ * Major: Game Dev/ Animation
/ * Date Created: 10/15/26
/ * Last Modified: 11/08/26
/ * Due Date: 10/28/26
/ * Course: Cpsc 320
/ * Professor Name: Prof. Jacklitsch
/ * Assignment: Project One - Solo Project
/ * Purpose: Manages player, enemy, and camera references, level transitions
/ * and character positions, and the win condition
/*******************************************************************/
using Godot;
using System;
using System.Threading.Tasks;

public partial class GameManager : Node2D
{
    [Export]
    private LoadManager _loadManger;

    [Export]
    private Node2D _zoneContainer;

    [Export]
    private RunTimer _runTimer;

    private WebCounter _webCounter;

    private Node2D _spawnPoint;

    public PlayerController PlayerRef { get; private set; }

    public EnemyController EnemyRef { get; private set; }

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
            WinGame();
            this.QueueFree();
            return;
        }
    }

    /*
    Name: FindCounter
    Description: finds refernce for counter
    Parameters: WebCounter counterObj
    Returns: None
    */
    public void FindCounter(WebCounter counterObj)
    {
        _webCounter = counterObj;
        if (PlayerRef != null)
        {
            _webCounter.UpdateCounter(PlayerRef.WebCount);
        }
    }

    /*
    Name: ClearCounter
    Description: clear referenec when level removed
    Parameters: None
    Returns: None
    */
    public void ClearCounter()
    {
        _webCounter = null;
    }

    /*
    Name: UpdateWebCounter
    Description: updates web counter value
    Parameters: int webAmount
    Returns: None
    */
    public void UpdateWebCounter(int webAmount)
    {
        if (_webCounter != null)
        {
            _webCounter.UpdateCounter(webAmount);
        }
    }

    /*
    Name: FindPlayer
    Description: find player in new level
    Parameters: Node newLevel
    Returns: None
    */
    public void FindPlayer(Node newLevel)
    {
        if(PlayerRef != null && IsInstanceValid(PlayerRef))
        {
            GD.Print("Player already exists");
            return;
        }
        PlayerRef = newLevel.FindChild("PlayerChar", true, false) as PlayerController; //taken 11/2/26 https://docs.godotengine.org/en/stable/classes/class_node.html?utm_source=chatgpt.com#class-node-method-find-child
                                                                                             //basically goes through tree order looking to see if a node is a child and if owned
        if (PlayerRef == null)
        {
            GD.Print("Player Not Found");
            return;
        }
        PlayerRef.CrystalCollectedSignal += OnCrystalCollected;
        //move player outside of level as to not delete it in the load
        PlayerRef.Reparent(GetParent(), true);
    }

    /*
    Name: FindEnemy 
    Description: find enemy in new level
    Parameters: Node newLevel
    Returns: None
    */
    public void FindEnemy(Node newLevel)
    {
        if(EnemyRef != null && IsInstanceValid(EnemyRef))
        {
            GD.Print("Enemey already Exsists");
            return;
        }

        EnemyRef = newLevel.FindChild("EnemyController", true, false) as EnemyController;

        if(EnemyRef == null)
        {
            GD.Print("Enemy Not Found");
            return;
        }
  
        //keep enemy alive during level change
        EnemyRef.Reparent(GetParent(), true);
        //Give enemy player reference
        EnemyRef.SetPlayer(PlayerRef);
    }

    /*
    Name: LoadNextLevel
    Description: loads next level
    Parameters: string scenePath
    Returns: None
    */
    public void LoadNextLevel(string scenePath)
    {
        LevelLoader.Instance.SwitchScene(scenePath);
    }

    /*
    Name: MovePlayer
    Description: spawn player at spawn point in next level
    Parameters: Node newLevel
    Returns: None
    */
    public async Task MovePlayer(Node newLevel)
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

        //find pickups
        PlayerRef.FindPickups();

        PlayerRef.IsTransitioning = false;
    }

    /*
    Name: MoveEnemy
    Description: spawn enemy at spawn point in next level
    Parameters: Node newLevel
    Returns: None
    */
    public void MoveEnemy(Node newLevel)
    {
        if (EnemyRef == null)
        {
            return;
        }

        Node2D spawnPoint = newLevel.FindChild("EnemySpawnLocation", true, false) as Node2D;

        NavigationRegion2D navRegion = newLevel.FindChild("NavigationRegion2D", true, false) as NavigationRegion2D;

        if(spawnPoint == null)
        {
            GD.Print("spawn not found");
            return;
        }
        if (navRegion == null)
        {
            GD.Print("nav not found");
            return;
        }

        EnemyRef.MoveToLevel(spawnPoint.GlobalPosition, navRegion);
    }

    /*
    Name: FindCamera
    Description: find camera in new level
    Parameters: Node newLevel
    Returns: None
    */
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

        //save camera
        CameraObj.Reparent(GetParent(), true);
        //give target
        CameraObj.SetTarget(PlayerRef);
        CameraObj.MakeCurrent(); //makes sure this camera is active in viewport
    }

    /*
    Name: OnCrystalCollected
    Description: if crystal collected is true enemy can start moving
    Parameters: None
    Returns: None
    */
    public void OnCrystalCollected()
    {
        if (EnemyRef != null)
        {
            EnemyRef.StartChasing();
        }
    }

    /*
    Name: SpeedUpEnemy
    Description: calls to speed up enemy
    Parameters: None
    Returns: None
    */
    public void SpeedUpEnemy()
    {
        if (EnemyRef != null)
        {
            EnemyRef.SpeedUp();
        }
    }

    /*
    Name: KillPlayer
    Description: if players dies call lose condition
    Parameters: None
    Returns: None
    */
    public void KillPlayer()
    {
        LoseGame();
    }

    //!! needed to add this because characters would show up after win/lose menu was called
    /*
    Name: RemoveCharacters
    Description: makes sure charcters are not active 
    Parameters: None
    Returns: None
    */
    public void RemoveCharacters()
    {
        if (CameraObj != null)
        {
            CameraObj.SetProcess(false);
            CameraObj.Enabled = false;
            CameraObj.QueueFree();
            CameraObj = null;
        }
        if (EnemyRef != null)
        {
            EnemyRef.QueueFree();
            EnemyRef = null;
        }
        if (PlayerRef != null)
        {
            PlayerRef.QueueFree();
            PlayerRef = null;
        }
        CameraObj = null;
        PlayerRef = null;
        EnemyRef = null;
    }

    /*
    Name: LoseGame
    Description: removes characters, pauses tree, loads lose screen
    Parameters: none
    Returns: None
    */
    public void LoseGame()
    {
        RemoveCharacters();
        GetTree().Paused = false;
        LevelLoader.Instance.SwitchScene(LevelLoader.Instance.LoseScreen);
    }

    /*
    Name: WinGame
    Description: removes characters, pauses tree, loads win screen
    Parameters: Nonel
    Returns: None
    */
    public void WinGame()
    {
        RemoveCharacters();
        GetTree().Paused = false;
        LevelLoader.Instance.SwitchScene(LevelLoader.Instance.WinScreen);
    }
}
