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

    //on load in
    public override void _Ready()
    {
        _menuButton.Pressed += MenuPressed;
        _exitButton.Pressed += ExitGame;
    }

    //load menu
    public void MenuPressed()
    {
        LevelLoader.Instance.SwitchScene(LevelLoader.Instance.MenuScene);
    }

    //close program
    public void ExitGame()
    {
        GetTree().Quit();
    }
}
