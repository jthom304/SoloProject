using Godot;
using System;

public partial class MapZone : Node2D
{
    public string SceneName { get; private set; }

    [Export]
    public Area2D CameraZone { get; private set; }

    [Export]
    public Area2D zoneBoundary { get; private set; }

    [Export]
    public ZonePortal[] ZonePortals { get; private set; }

    [Signal]
    public delegate void EnteredPortalEventHandler(string nextScene);

    public override void _Ready()
    {
        SceneName = this.SceneFilePath;
        foreach (ZonePortal currentPortal in ZonePortals)
        {
            currentPortal.EnteredPortal += PortalTriggered;
        }
    }

    public void PortalTriggered(ZonePortal triggereingPortal)
    {
        EmitSignal(SignalName.EnteredPortal,  triggereingPortal.ConnectedScene);
    }

}
