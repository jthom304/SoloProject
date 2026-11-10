/*******************************************************************/
/*Filename: ZonePortal.cs
/ * Author: Jasper Thompson
/ * Major: Game Dev/ Animation
/ * Date Created: 10/28/26
/ * Last Modified: 11/08/26
/ * Due Date: 10/28/26
/ * Course: Cpsc 320
/ * Professor Name: Prof. Jacklitsch
/ * Assignment: Project One - Solo Project
/ * Purpose: handles transitions when player enters a portal
/*******************************************************************/
using Godot;
using System;

public partial class ZonePortal : Node2D
{
    [Signal]
    public delegate void EnteredPortalEventHandler(ZonePortal signaledPortal);

    [Export]
    public string ConnectedScene { get; private set; }

    [Export]
    public Area2D LoadZone { get; private set; }

    [Export]
    public Node2D SpawnPoint { get; private set; }

    private bool _portalTriggered = false;

    /*
    Name: _Ready
    Description: connect button signal
    Parameters: None
    Returns: None
    */
    public override void _Ready()
    {
        LoadZone.BodyEntered += OnPlayerEntered;
    }

    /*
    Name: OnPlayerEntered
    Description: detects if player has entered portal, emits signal, 
    calls load next level
    Parameters: Node2D enteringBody
    Returns: None
    */
    public void OnPlayerEntered(Node2D enteringBody)
    {
        PlayerController playerBody = LevelLoader.Instance.GameManagerRef.PlayerRef;
        if (enteringBody == null || enteringBody != playerBody)
        {
            return;
        }    
        
        if(_portalTriggered)
        {
            return;
        }

        _portalTriggered = true;
        GD.Print(LoadZone.GlobalPosition + " " + enteringBody.GlobalPosition);
        EmitSignal(SignalName.EnteredPortal, this);
        LevelLoader.Instance.GameManagerRef.LoadNextLevel(ConnectedScene);
    }

}
