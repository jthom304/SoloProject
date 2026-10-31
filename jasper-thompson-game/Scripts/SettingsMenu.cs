using Godot;
using System;

public partial class SettingsMenu : Control
{
    [Export]
    private HSlider _volumeSlider;

    [Export]
    private TextureButton _backButton;

    public override void _Ready()
    {
        GD.Print("SettingsMenu loaded!");

        if (_volumeSlider == null)
        {
            GD.PrintErr("Volume Slider is NULL!");
            return;
        }

        if (_backButton == null)
        {
            GD.PrintErr("Back Button is NULL!");
            return;
        }

        if (AudioManager.Instance == null)
        {
            GD.PrintErr("AudioManager Instance is NULL!");
            return;
        }

        GD.Print("All references found!");

        _volumeSlider.Value = GlobalVariables.GameVolume;

        AudioManager.Instance.UpdateVolume(GlobalVariables.GameVolume);

        _volumeSlider.ValueChanged += AudioManager.Instance.UpdateVolume;

        _backButton.Pressed += BackPressed;
    }

    public void BackPressed()
    {
        LevelLoader.Instance.SwitchScene(LevelLoader.Instance.MenuScene);
    }
}
