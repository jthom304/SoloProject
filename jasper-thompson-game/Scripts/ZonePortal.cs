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

    public override void _Ready()
    {
        LoadZone.BodyEntered += OnPlayerEntered;
    }

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
