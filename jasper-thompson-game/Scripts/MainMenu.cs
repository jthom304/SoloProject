/*******************************************************************/
/*Filename: MainMenu.cs
/ * Author: Jasper Thompson
/ * Major: Game Dev/ Animation
/ * Date Created: 10/21/26
/ * Last Modified: 11/08/26
/ * Due Date: 10/28/26
/ * Course: Cpsc 320
/ * Professor Name: Prof. Jacklitsch
/ * Assignment: Project One - Solo Project
/ * Purpose: set and define button signals for menu 
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

    /*
    Name: _Ready
    Description: set button signals
    Parameters: None
    Returns: None
    */
    public override void _Ready()
    {
        _startButton.Pressed += OnStartPressed; //call to start game
        _optionsButton.Pressed += OptionsPressed; // call to show options menu
        _creditsButton.Pressed += CreditsPressed; // call to show credits menu
        _exitButton.Pressed += ExitGame; //call to end game
    }

    /*
    Name: OnStartedPressed
    Description: load level one
    Parameters: None
    Returns: None
    */
    private void OnStartPressed()
    {

        if (LevelLoader.Instance == null)
        {
            GD.PrintErr("LevelLoader Instance is NULL!");
            return;
        }

        LevelLoader.Instance.SwitchScene(LevelLoader.Instance.StartLevelScene);
    }

    /*
    Name: OptionsPressed
    Description: load settingmenu
    Parameters: None
    Returns: None
    */
    public void OptionsPressed()
    {
        LevelLoader.Instance.SwitchScene(LevelLoader.Instance.SettingsMenu);
    }

    /*
    Name: CreditsPressed
    Description: load credits menu
    Parameters: None
    Returns: None
    */
    public void CreditsPressed()
    {
        LevelLoader.Instance.SwitchScene(LevelLoader.Instance.CreditsMenu);
    }

    /*
    Name: ExitGame
    Description: close program
    Parameters: None
    Returns: None
    */
    public void ExitGame()
    {
        GetTree().Quit();
    }
}
