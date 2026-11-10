/*******************************************************************/
/*Filename: LevelLoader.cs
/ * Author: Jasper Thompson
/ * Major: Game Dev/ Animation
/ * Date Created: 10/21/26
/ * Last Modified: 11/08/26
/ * Due Date: 10/28/26
/ * Course: Cpsc 320
/ * Professor Name: Prof. Jacklitsch
/ * Assignment: Project One - Solo Project
/ * Purpose: Find and change scenes
/*******************************************************************/
using Godot;
using System;

public partial class LevelLoader : Node
{
    //path to scenes
    [Export]
    public string MenuScene { get; set; } = "res://Scenes/main_menu.tscn";

    [Export]
    public string StartLevelScene { get; set; } = "res://Scenes/level_one.tscn";

    [Export]
    public string GameLevelScene { get; set; } = "res://Scenes/level_two.tscn";

    [Export]
    public string SettingsMenu { get; set; } = "res://Scenes/settings_menu.tscn";

    [Export]
    public string CreditsMenu { get; set; } = "res://Scenes/credits_menu.tscn";

    [Export]
    public string WinScreen { get; set; } = "res://Scenes/win_screen.tscn";

    [Export]
    public string LoseScreen { get; set; } = "res://Scenes/lose_screen.tscn";

    [Export]
    private PauseMenu _pauseMenu;

    //currernt scene shown
    private Node _currentScene;
    private Node _sceneContainer;

    public static LevelLoader Instance { get; private set; }

    public GameManager GameManagerRef { get; private set; }

    private LoadManager _loadManager;

    /*
    Name: _EnterTree
    Description: set this instance
    Parameters: None
    Returns: None
    */
    public override void _EnterTree()
    {
        //set instance
        Instance = this;
    }

    /*
    Name: _Ready
    Description: set up references and gets inital scene
    Parameters: None
    Returns: None
    */
    public override void _Ready()
    {
        //Find Parent
        Node gameScene = GetParent();

        //Get GM ref
        GameManagerRef = gameScene.GetNode<GameManager>("GameManager");

        //Load Manager var
        _loadManager = gameScene.GetNode<LoadManager>("LoadManager");

        //scene container
        _sceneContainer = GetNode("CurrentScene");

        //listen for completion
        _loadManager.LoadedLevel += OnLevelLoaded;

        //get intial scene
        if (_sceneContainer.GetChildCount() > 0)
        {
            _currentScene = _sceneContainer.GetChild(0);
        }

    }

     /*
     Name: SwitchScene
     Description: calls load for next scene
     Parameters: string scenePath
     Returns: None
     */
    public void SwitchScene(string scenePath)
    {
        _loadManager.BeginLoadProcess(scenePath);
    }

    /*
    Name: OnLevelLoaded
    Description: gets new scene, changes music, sets up pause menu,
    removes previous scene, loads new scene, sets up player, enemy and camera,
    updates current scene
    Parameters: resource LevelLoaded
    Returns: None
    */
    private void OnLevelLoaded(Resource LevelLoaded)
    {
        //what is next level
        PackedScene packedScene = LevelLoaded as PackedScene;

        if (packedScene == null)
        {
            return;
        }

        //switch music based on scenes
        if(packedScene.ResourcePath == MenuScene)
        {
            AudioManager.Instance.PlayMainMenuMusic();
        }
        if (packedScene.ResourcePath == StartLevelScene)
        {
            AudioManager.Instance.PlayLevelOneMusic();
        }
        if (packedScene.ResourcePath == GameLevelScene)
        {
            AudioManager.Instance.PlayLevelTwoMusic();
        }

        //what scenes can pause 
        bool CanPause = packedScene.ResourcePath == StartLevelScene || packedScene.ResourcePath == GameLevelScene;
        _pauseMenu.Visible = false;
        _pauseMenu.ProcessMode = ProcessModeEnum.Pausable;
        _pauseMenu.SetProcess(CanPause);


        if (_currentScene != null)
        {
            //remove web count/counter
            GameManagerRef.ClearCounter();
            //remove current scene
            _sceneContainer.RemoveChild(_currentScene);
            //delete scene
            _currentScene.QueueFree();
            //reset for bug catching
            _currentScene = null;
        }
        Node newInstance = packedScene.Instantiate();

        //add new scene
        _sceneContainer.AddChild(newInstance);

        //register counter
        WebCounter webCounter = newInstance.FindChild("WebCounter", true, false) as WebCounter;
        if (webCounter != null)
        {
        //player and camera setup
            GameManagerRef.FindCounter(webCounter);
        }
        Node2D spawn = newInstance.FindChild("SpawnLocation", true, false) as Node2D;
        Node2D zone = newInstance.FindChild("CameraZone", true, false) as Node2D;

        //find player and keep it from being destroyed by level load
        if (packedScene.ResourcePath == StartLevelScene)
        {
            GameManagerRef.FindPlayer(newInstance);
            GameManagerRef.FindEnemy(newInstance);
            GameManagerRef.FindCamera(newInstance);
        }

        //move player to level 2
        else if(packedScene.ResourcePath == GameLevelScene)
        {
            GameManagerRef.MovePlayer(newInstance);
            GameManagerRef.MoveEnemy(newInstance);

            GameManagerRef.CameraObj.ResetCamera(GameManagerRef.PlayerRef);

            CameraZone cameraZone = newInstance.FindChild("CameraZone", true, false) as CameraZone;

            if (cameraZone != null)
            {
                cameraZone.ActivateZone();
            }
        }

        _currentScene = newInstance;
    }
}
