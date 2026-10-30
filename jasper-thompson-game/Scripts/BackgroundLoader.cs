using Godot;
using System;
using System.Collections.Specialized;

public partial class BackgroundLoader : Node
{
    [Signal]
    public delegate void FinishedLoadingEventHandler(Resource loadedResource);

    private string _scenePath;

    public void InitializeObject(string scenePath)
    {
        _scenePath = scenePath;
        ResourceLoader.LoadThreadedRequest(scenePath, useSubThreads: false);
    }

    public override void _Process(double delta)
    {
        ResourceLoader.ThreadLoadStatus loadStatus = ResourceLoader.LoadThreadedGetStatus(_scenePath);
        if(loadStatus == ResourceLoader.ThreadLoadStatus.Loaded)
        {
            EmitSignal(SignalName.FinishedLoading, ResourceLoader.LoadThreadedGet(_scenePath));
            QueueFree();
        }
    }
}
