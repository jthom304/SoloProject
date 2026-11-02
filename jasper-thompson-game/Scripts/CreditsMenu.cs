using Godot;
using System;

public partial class CreditsMenu : Control
{
    [Export]
    private TextureButton _backButton;

    public override void _Ready()
    {
        _backButton.Pressed += BackPressed;
    }

    public void BackPressed()
    {
        LevelLoader.Instance.SwitchScene(LevelLoader.Instance.MenuScene);
    }
}
