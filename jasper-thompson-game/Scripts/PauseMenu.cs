/*******************************************************************/
/*Filename: PauseMenu.cs
/ * Author: Jasper Thompson
/ * Major: Game Dev/ Animation
/ * Date Created: 10/31/26
/ * Last Modified: 11/08/26
/ * Due Date: 10/28/26
/ * Course: Cpsc 320
/ * Professor Name: Prof. Jacklitsch
/ * Assignment: Project One - Solo Project
/ * Purpose: Handle pause screen commands
/*******************************************************************/
using Godot;
using System;

public partial class PauseMenu : Control
{
    [Export]
    private HSlider _volumeSlider;

    [Export]
    private TextureButton _resumeButton;

    [Export]
    private TextureButton _exitGame;

    /*
    Name: _Ready
    Description: set up reference for slider and button signal
    Parameters: None
    Returns: None
    */
    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Pausable;
        this.Visible = false;
        if (AudioManager.Instance != null)
        {
            _volumeSlider.SetValueNoSignal(GlobalVariables.GameVolume);
            _volumeSlider.ValueChanged += AudioManager.Instance.UpdateVolume;
            AudioManager.Instance.VolumeChanged += VolumeChanged;
        }

        _resumeButton.Pressed += ResumePressed;
        _exitGame.Pressed += ExitPressed;
    }

    /*
    Name: VolumeChanged
    Description: changes volume value
    Parameters: float newVolume
    Returns: None
    */
    //!!there is a slight issue with this
    private void VolumeChanged(float newVolume)
    {
        //so it doesn't affect settings menu
        _volumeSlider.SetValueNoSignal(newVolume); // taken 11/08/26 https://docs.godotengine.org/en/4.5/classes/class_range.html#class-range-method-set-value-no-signal
    }

     /*
     Name: _ExitTree
     Description: disconnects signal
     Parameters: None
     Returns: None
     */
    public override void _ExitTree()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.VolumeChanged -= VolumeChanged;
        }
    }

     /*
     Name: _Process
     Description: check for input action
     Parameters: double delta
     Returns: None
     */
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("PauseGame"))
        {
            TogglePause();
        }
    }

    /*
    Name: TogglePause
    Description: shows menu when paused
    Parameters: None
    Returns: None
    */
    public void TogglePause()
    {
        if (GetTree().Paused == false)
        {
            this.Visible = true;
            ProcessMode = ProcessModeEnum.WhenPaused;
            GetTree().Paused = true;
        }
        else
        {
            this.Visible = false;
            ProcessMode = ProcessModeEnum.Pausable;
            GetTree().Paused = false;
        }
    }

    /*
    Name: ResumePressed
    Description: calls TogglePause
    Parameters: None
    Returns: None
    */
    private void ResumePressed()
    {
        TogglePause();
    }

    /*
    Name: ExitPressed
    Description: close program
    Parameters: None
    Returns: None
    */
    private void ExitPressed()
    {
        GetTree().Quit();
    }
}
