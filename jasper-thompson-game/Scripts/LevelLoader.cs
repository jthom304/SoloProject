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

        //Start with showing menu
        //SwitchScene(MenuScene);
    }

    public void SwitchScene(string scenePath)
    {
        _loadManager.BeginLoadProcess(scenePath);
    }

    private void OnLevelLoaded(Resource LevelLoaded)
    {
        GD.Print("LL called");
        PackedScene packedScene = LevelLoaded as PackedScene;

        if (packedScene == null)
        {
            GD.Print("PackedScene == null");
            return;
        }

        GD.Print("Loading scene: " + packedScene.ResourcePath);
        Node newInstance = packedScene.Instantiate();

        sceneContainer.AddChild(newInstance);

        if(CurrentScene != null)
        {
            GD.Print("Removing old scene: " + CurrentScene.Name);
            CurrentScene.QueueFree();
        }

        CurrentScene = newInstance;
        GD.Print("New scene added: " + CurrentScene.Name);
    }
}
