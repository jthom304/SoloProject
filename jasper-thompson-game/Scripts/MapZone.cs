/*******************************************************************/
/*Filename: MapZone.cs
/ * Author: Prof. Jacklitsch
/ * Editor: Jasper Thompson
/ * Major: Game Dev/ Animation
/ * Date Created: 10/28/26
/ * Last Modified: 11/08/26
/ * Due Date: 10/28/26
/ * Course: Cpsc 320
/ * Professor Name: Prof. Jacklitsch
/ * Assignment: Project One - Solo Project
/ * Purpose: maintain portals emit signal when triggered
/*******************************************************************/
using Godot;
using System;

public partial class MapZone : Node2D
{
    public string SceneName { get; private set; }

    [Export]
    public Area2D CameraZone { get; private set; }

    [Export]
    public Area2D ZoneBoundary { get; private set; }

    [Export]
    public ZonePortal[] ZonePortals { get; private set; }

    [Signal]
    public delegate void EnteredPortalEventHandler(string nextScene);

    /*
    Name: _Ready
    Description: get scene path and loop through portals
    Parameters: None
    Returns: None
    */
    public override void _Ready()
    {
        SceneName = this.SceneFilePath;
        foreach (ZonePortal currentPortal in ZonePortals)
        {
            currentPortal.EnteredPortal += PortalTriggered;
        }
    }

    /*
    Name: PortalTriggered
    Description: emits signal when portal is triggered and sends scene var
    Parameters: ZonePortal triggereingPortal
    Returns: None
    */
    public void PortalTriggered(ZonePortal triggereingPortal)
    {
        EmitSignal(SignalName.EnteredPortal,  triggereingPortal.ConnectedScene);
    }

}
