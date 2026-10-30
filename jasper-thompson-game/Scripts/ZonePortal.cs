using Godot;
using System;

public partial class ZonePortal : Node2D
{
    [Export]
    public string ConnectedScene { get; private set; }

    [Export]
    public Area2D LoadZone { get; private set; }

    [Export]
    public Node2D SpawnPoint { get; private set; }

    [Signal]
    public delegate void EnteredPortalEventHandler(ZonePortal signaledPortal);

    public override void _Ready()
    {
        LoadZone.BodyEntered += OnPlayerEntered;
    }

    public void OnPlayerEntered(Node2D enteringBody)
    {
        EmitSignal(SignalName.EnteredPortal, this);
    }

}
