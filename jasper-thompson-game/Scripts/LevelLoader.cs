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

    public override void _EnterTree()
    {
        //set instance
        Instance = this;
    }

    public override void _Ready()
    {
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
        AddChild(newInstance); //add scene to the loader

        if (currentScene != null)
        {
            currentScene.QueueFree();
        }
        currentScene = newInstance;

    }
}
