using Godot;
using System;

public partial class LoadManager : Node
{
    [Signal]
    public delegate void LoadedLevelEventHandler(Resource loadedLevel);

    [Signal]
    public delegate void FinishedProcessEventHandler();

    [Export]
    public LoadingScreen _loadingScreen;

    private string _scenePath;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        _loadingScreen.FadedIn += BeginLevelLoad;
        _loadingScreen.FadedOut += CompleteLoadProcess;
    }

    public void BeginLoadProcess(string scenePath)
    {
        _scenePath = scenePath;
        GetTree().Paused = true;
        _loadingScreen.FadeIn();
    }

    public void BeginLevelLoad()
    {
        BackgroundLoader loadHandler = new BackgroundLoader();
        loadHandler.InitializeObject(_scenePath);
        loadHandler.FinishedLoading += CompleteLevelLoad;
        AddChild(loadHandler);
    }

    public void CompleteLevelLoad(Resource loadedLevel)
    {
        EmitSignal(SignalName.LoadedLevel, loadedLevel);
        _loadingScreen.FadeOut();
    }

    public void CompleteLoadProcess()
    {
        GetTree().Paused = false;
        EmitSignal(SignalName.FinishedProcess);
    }
}
