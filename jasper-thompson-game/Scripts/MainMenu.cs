/*******************************************************************/
/*Filename: MainMenu.cs
/ * Author: Jasper Thompson
/ * Major: Game Dev/ Animation
/ * Date Created: 10/10/26
/ * Last Modified: 11/08/26
/ * Due Date: 10/28/26
/ * Course: Cpsc 320
/ * Professor Name: Prof. Jacklitsch
/ * Assignment: Project One - Solo Project
/ * Purpose: set and define button signals
/*******************************************************************/
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
