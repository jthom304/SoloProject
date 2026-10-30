using Godot;
using System;

public partial class LevelLoader : Node
{
    //path to scenes
    [Export]
    public string MenuScene { get; set; } = "res://Scenes/game_scene.tscn";

    [Export]
    public string StartLevelScene { get; set; } = "res://Scenes/level_one.tscn";

    [Export]
    public string GameLevelScene { get; set; } = "res://Scenes/level_two.tscn";

    //currernt scene shown
    private Node2D currentScene;
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
        Node2D gameScene = GetParent();

        //Get GM ref
        GameManagerRef = gameScene.GetNode<GameManager>("GameManager");

        //Load Manager var
        _loadManager = gameScene.GetNode<LoadManager>("LoadManager");

        //scene container
        sceneContainer = GetNode("CurrentScene");

        if(GameManagerRef || _loadManager || sceneContainer == null)
        {
            GD.PrintErr("Game, Load, or scene container not found");
            return;
        }

        //listen for completion
        _loadManager.LoadedLevel += OnLevelLoaded;

        //Start with showing menu
        SwitchScene(MenuScene);
    }

    public void SwitchScene(string scenePath)
    {
        _loadManager.BeginLoadProcess(scenePath);
    }

    private void OnLevelLoaded(Resource LevelLoaded)
    {
        PackedScene packedScene = LevelLoaded as PackedScene;

        if (packedScene == null)
        {
            return;
        }

        Node newInstance = packedScene.Instantiate();

        sceneContainer.AddChild(newInstance);

        if(currentScene != null)
        {
            currentScene.QueueFree();
        }

        currentScene = newInstance;
    }
}
