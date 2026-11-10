/*******************************************************************/
/*Filename: LoadManager.cs
/ * Author: Prof Jacklitsch
/ * Major: Game Dev/ Animation
/ * Date Created: 10/28/26
/ * Last Modified: 11/08/26
/ * Due Date: 10/28/26
/ * Course: Cpsc 320
/ * Professor Name: Prof. Jacklitsch
/ * Assignment: Project One - Solo Project
/ * Purpose:  
/*******************************************************************/
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

    /*
    Name: _Ready
    Description: set process mode and connects button signals
    Parameters: None
    Returns: None
    */
    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        _loadingScreen.FadedIn += BeginLevelLoad;
        _loadingScreen.FadedOut += CompleteLoadProcess;
    }

    /*
    Name: BeginLoadProcess
    Description: starts loading process (stores next scene, pause game,
    call fade animation
    Parameters: string scenePath
    Returns: None
    */
    public void BeginLoadProcess(string scenePath)
    {
        _scenePath = scenePath;
        GetTree().Paused = true;
        _loadingScreen.FadeIn();
    }

    /*
    Name: BeginLevelLoad
    Description: handles bg loading (create new bg loader, give scene,
    connect signal, add loader as child)
    Parameters: None
    Returns: None
    */
    public void BeginLevelLoad()
    {
        BackgroundLoader loadHandler = new BackgroundLoader();
        loadHandler.InitializeObject(_scenePath);
        loadHandler.FinishedLoading += CompleteLevelLoad;
        AddChild(loadHandler);
    }

    /*
    Name: CompleteLevelLoad
    Description: emits signal and starts fade out animation
    Parameters: Resource loadedLevel
    Returns: None
    */
    public void CompleteLevelLoad(Resource loadedLevel)
    {
        EmitSignal(SignalName.LoadedLevel, loadedLevel);
        _loadingScreen.FadeOut();
    }

    /*
    Name: CompleteLoadProcess
    Description: unpause game, emit signal
    Parameters: None
    Returns: None
    */
    public void CompleteLoadProcess()
    {
        GetTree().Paused = false;
        EmitSignal(SignalName.FinishedProcess);
    }
}
