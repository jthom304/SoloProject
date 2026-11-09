/*******************************************************************/
/*Filename: LevelLoader.cs
/ * Author: Jasper Thompson
/ * Major: Game Dev/ Animation
/ * Date Created: 10/10/26
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
    private Node CurrentScene;
    private Node sceneContainer;

    public static LevelLoader Instance { get; private set; }

    public GameManager GameManagerRef { get; private set; }

    private LoadManager _loadManager;

    public override void _EnterTree()
    {
        //set instance
        Instance = this;
    }

    public override void _Ready()
    {
        //Find Parent
        Node gameScene = GetParent();

        //Get GM ref
        GameManagerRef = gameScene.GetNode<GameManager>("GameManager");

        //Load Manager var
        _loadManager = gameScene.GetNode<LoadManager>("LoadManager");

        //scene container
        sceneContainer = GetNode("CurrentScene");

        if (GameManagerRef == null || _loadManager == null || sceneContainer == null)
        {
            GD.PrintErr("Game, Load, or scene container not found");
            return;
        }

        if (sceneContainer.GetChildCount() > 0)
        {
            CurrentScene = sceneContainer.GetChild(0);
            GD.Print("Initial scene registered: " + CurrentScene.Name);
        }

        //listen for completion
        _loadManager.LoadedLevel += OnLevelLoaded;

    }

    public void SwitchScene(string scenePath)
    {
        _loadManager.BeginLoadProcess(scenePath);
    }

    private void OnLevelLoaded(Resource LevelLoaded)
    {
        GD.Print("LL called");
        //what is next level
        PackedScene packedScene = LevelLoaded as PackedScene;

        if (packedScene == null)
        {
            GD.Print("PackedScene == null");
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


        if (CurrentScene != null)
        {
            //remove current scene
            sceneContainer.RemoveChild(CurrentScene);
            //delete scene
            CurrentScene.QueueFree();
            //reset for bug catching
            CurrentScene = null;
        }
        Node newInstance = packedScene.Instantiate();

        GD.Print("LoadingScene: " + packedScene.ResourcePath);
        GD.Print("Scene B4 clidren: " + sceneContainer.GetChildCount());
        //add new scene
        sceneContainer.AddChild(newInstance);
        GD.Print("SceneChildrenAfter: " + sceneContainer.GetChildCount());
        GD.Print("New Scene: " + newInstance.Name);
        GD.Print("Lose Screen Type: " + newInstance.GetType().Name);
        GD.Print("Lose Screen children: " + newInstance.GetChildCount());

        if (newInstance is CanvasItem item)
        {
            GD.Print("Lose screen visible: " + item.IsVisibleInTree());
        }
        else
        {
            GD.Print("New scene is not a CanvasItem");
        }
        Control control = newInstance as Control;

        if (control != null)
        {
            GD.Print("Visible: " + control.IsVisibleInTree());
        }


        //player and camera setup
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

        CurrentScene = newInstance;
    }
}
