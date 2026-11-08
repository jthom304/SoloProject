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
        this.Visible = false;
        if (AudioManager.Instance != null)
        {
            _volumeSlider.Value = GlobalVariables.GameVolume;
            _volumeSlider.ValueChanged += AudioManager.Instance.UpdateVolume;
            AudioManager.Instance.VolumeChanged;
        }

        _resumeButton.Pressed += ResumePressed;
        _exitGame.Pressed += ExitPressed;
    }

    private void VolumeChanged(float newVolume)
    {
        _volumeSlider.SetValueNoSignal(newVolume);
    }

    public override void _ExitTree()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.VolumeChanged -= OnVolumeChanged;
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("PauseGame"))
        {
            TogglePause();
        }
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
