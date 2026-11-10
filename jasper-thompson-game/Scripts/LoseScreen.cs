/*******************************************************************/
/*Filename: LoseScreen.cs
/ * Author: Jasper Thompson
/ * Major: Game Dev/ Animation
/ * Date Created: 11/07/26
/ * Last Modified: 10/08/26
/ * Due Date: 10/28/26
/ * Course: Cpsc 320
/ * Professor Name: Prof. Jacklitsch
/ * Assignment: Project One - Solo Project
/ * Purpose: Handle lose screen commands
/*******************************************************************/
using Godot;
using System;

public partial class LoseScreen : Control
{
    [Export]
    private TextureButton _menuButton;

    [Export]
    private TextureButton _exitButton;

    /*
    Name: _Ready
    Description: connect button signals
    Parameters: None
    Returns: None
    */
    public override void _Ready()
    {
        _menuButton.Pressed += MenuPressed;
        _exitButton.Pressed += ExitGame;
    }

    /*
    Name: MenuPressed
    Description: load menu screen
    Parameters: None
    Returns: None
    */
    public void MenuPressed()
    {
        LevelLoader.Instance.SwitchScene(LevelLoader.Instance.MenuScene);
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
