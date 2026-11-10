/*******************************************************************/
/*Filename: BackgroundLoader.cs
/ * Author: Prof. Jacklitsch
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
using System.Collections.Specialized;

public partial class BackgroundLoader : Node
{
    [Signal]
    public delegate void FinishedLoadingEventHandler(Resource loadedResource);

    private string _scenePath;

    /*
    Name: InitalizeObject
    Description: get and load scene
    Parameters: string scenePath
    Returns: None
    */
    public void InitializeObject(string scenePath)
    {
        _scenePath = scenePath;
        ResourceLoader.LoadThreadedRequest(scenePath, useSubThreads: false);
    }

    /*
    Name: _Process
    Description: checks loading status and when it is finsihed, retrieves
    resource, emits signal, and removes bg loader
    Parameters: double delta
    Returns: None
    */
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
