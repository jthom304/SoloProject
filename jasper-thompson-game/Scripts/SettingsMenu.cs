/*******************************************************************/
/*Filename: SettingsMenu.cs
/ * Author: Jasper Thompson
/ * Major: Game Dev/ Animation
/ * Date Created: 10/31/26
/ * Last Modified: 11/08/26
/ * Due Date: 10/28/26
/ * Course: Cpsc 320
/ * Professor Name: Prof. Jacklitsch
/ * Assignment: Project One - Solo Project
/ * Purpose: Handle settings menu screen commands
/*******************************************************************/
using Godot;
using System;

public partial class SettingsMenu : Control
{
    [Export]
    private HSlider _volumeSlider;

    [Export]
    private TextureButton _backButton;

    /*
    Name: _Ready
    Description: set up reference for slider and button signal
    Parameters: None
    Returns: None
    */
    public override void _Ready()
    {
        if (AudioManager.Instance != null)
        {
            _volumeSlider.SetValueNoSignal(GlobalVariables.GameVolume);
            _volumeSlider.ValueChanged += AudioManager.Instance.UpdateVolume;
            AudioManager.Instance.VolumeChanged += OnVolumeChanged;
        }

        _backButton.Pressed += BackPressed;
    }

    /*
    Name: OnVolumeChanged
    Description: intializes sfx audio stream
    Parameters: float newVolume
    Returns: None
    */
    private void OnVolumeChanged(float newVolume)
    {
        _volumeSlider.SetValueNoSignal(newVolume);
    }

    /*
    Name: _ExitTree
    Description: disconnect signal
    Parameters: None
    Returns: None
    */
    public override void _ExitTree()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.VolumeChanged -= OnVolumeChanged;
        }
    }

    /*
    Name: BackPressed
    Description: loads menu scene
    Parameters: none
    Returns: None
    */
    public void BackPressed()
    {
        LevelLoader.Instance.SwitchScene(LevelLoader.Instance.MenuScene);
    }
}
