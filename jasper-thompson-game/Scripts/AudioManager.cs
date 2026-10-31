using Godot;
using System;

public partial class AudioManager : Node
{
    public static AudioManager Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }

    public void UpdateVolume(double newVolume)
    {
        GlobalVariables.GameVolume = (float)newVolume;
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), GlobalVariables.GameVolume);
    }
}
