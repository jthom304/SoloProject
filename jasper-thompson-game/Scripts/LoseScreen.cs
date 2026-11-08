using Godot;
using System;

public partial class LoseScreen : Control
{
    [Export]
    private TextureButton _menuButton;

    [Export]
    private TextureButton _exitButton;

    public override void _Ready()
    {
        _menuButton.Pressed += MenuPressed;
        _exitButton.Pressed += ExitGame;
    }

    public void MenuPressed()
    {
        LevelLoader.Instance.SwitchScene(LevelLoader.Instance.MenuScene);
    }

    public void ExitGame()
    {
        GetTree().Quit();
    }
}
