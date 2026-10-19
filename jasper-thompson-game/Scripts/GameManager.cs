using Godot;
using System;

public partial class GameManager : Node2D
{
    [Export]
    private PlayerController _playerController;

    [Export]
    private CameraController _cameraController;
}
