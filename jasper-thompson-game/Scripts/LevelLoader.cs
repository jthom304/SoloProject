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

    //currernt scene shown
    private Node currentScene;

    public static LevelLoader Instance { get; private set; }

    public override void _Ready()
    {
        //set instance
        Instance = this;
        //Start with showing menu
        SwitchScene(MenuScene);
    }

    public void SwitchScene(string scenePath)
    {
        PackedScene newScene = GD.Load<PackedScene>(scenePath);
        if (newScene == null)
        {
            GD.Print("Error: failed to load scene" + scenePath);
            return;
        }

        Node newInstance = newScene.Instantiate();

        if (currentScene != null)
        {
            currentScene.QueueFree();
        }
        GetTree().Root.CallDeferred("add_child", newInstance); //an error code told me to use this stated problem of nodes being busy ¯\/(._ .)\/¯
        currentScene = newInstance;

    }
}
