using Godot;
using System;

public partial class MainMenu : Control
{
    //button ref
    [Export]
    private TextureButton _startButton;
    [Export]
    private TextureButton _optionsButton;
    [Export]
    private TextureButton _creditsButton;
    [Export]
    private TextureButton _exitButton;
    //[Export]
    //private Button _backButton;

    /*
        Name: _Ready
        Description: when key is pressed send signal
        Parameters: None
        Returns: None
    */
    public override void _Ready()
    {
        _startButton.Pressed += OnStartPressed; //call to start game
        _optionsButton.Pressed += OptionsPressed; // call to show options menu
        _creditsButton.Pressed += CreditsPressed; // call to show credits menu
        _exitButton.Pressed += ExitGame; //call to end game
        //_backButton.Pressed += BackPressed; //call to show main menu and hide others
    }

    private void OnStartPressed()
    {
        GD.Print("Start pressed!");

        if (LevelLoader.Instance == null)
        {
            GD.PrintErr("LevelLoader Instance is NULL!");
            return;
        }

        GD.Print("LevelLoader found!");

        LevelLoader.Instance.SwitchScene(LevelLoader.Instance.StartLevelScene);
    }

    public void OptionsPressed()
    {
        LevelLoader.Instance.SwitchScene(LevelLoader.Instance.SettingsMenu);
    }

    public void CreditsPressed()
    {
        LevelLoader.Instance.SwitchScene(LevelLoader.Instance.CreditsMenu);
    }

    public void ExitGame()
    {
        GetTree().Quit();
    }
}
