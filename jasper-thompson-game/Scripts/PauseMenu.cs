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

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Pausable;

        if(Input.IsActionJustPressed("PauseGame"))
        {
            TogglePause();
        }

        _volumeSlider.Value = GlobalVariables.GameVolume;
        _volumeSlider.ValueChanged += AudioManager.Instance.UpdateVolume;

        _resumeButton.Pressed += ResumePressed;
        _exitGame.Pressed += ExitPressed;
    }

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

    private void ResumePressed()
    {
        TogglePause();
    }

    private void ExitPressed()
    {
        GetTree().Quit();
    }
}
