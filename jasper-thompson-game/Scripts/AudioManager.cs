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

        float volumeDb;
        if(newVolume <= 0)//decibel control
        {
            volumeDb = -80f;
        }
        else
        {
            volumeDb = Mathf.LinearToDb((float)newVolume);//searched "how to set up volume for godot slider" google ai gave table breakdown of volume and conversions for audio bus ie LinearToDb 
        }
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), GlobalVariables.GameVolume);
    }
}
