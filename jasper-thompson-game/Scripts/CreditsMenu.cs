/*******************************************************************/
/*Filename: CreditsMenu.cs
/ * Author: Jasper Thompson
/ * Major: Game Dev/ Animation
/ * Date Created: 11/02/26
/ * Last Modified: 11/02/26
/ * Due Date: 10/28/26
/ * Course: Cpsc 320
/ * Professor Name: Prof. Jacklitsch
/ * Assignment: Project One - Solo Project
/ * Purpose: Handle credits screen commands
/*******************************************************************/
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

    //load menu
    public void BackPressed()
    {
        LevelLoader.Instance.SwitchScene(LevelLoader.Instance.MenuScene);
    }
}
