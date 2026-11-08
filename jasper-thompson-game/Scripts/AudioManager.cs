using Godot;
using System;

public partial class AudioManager : Node
{
    [Signal]
    public delegate void VolumeChangedEventHandler(float newVolume);
    [Export]
    private AudioStreamPlayer _musicPlayer;
    [Export]
    private AudioStream _mainMusic;
    [Export]
    private AudioStream _levelOneMusic;
    [Export]
    private AudioStream _levelTwoMusic;

    public static AudioManager Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
        PlayMusic(_mainMusic);
        UpdateVolume(GlobalVariables.GameVolume);
    }

    public override void _ExitTree()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }

    public void PlayMusic(AudioStream currentMusic)
    {
        if(currentMusic == null)
        {
            GD.Print("Music Stream not found");
            return;
        }
        
        //Dont restart music if already playing
        if(_musicPlayer.Stream == currentMusic && _musicPlayer.Playing)
        {
            return;
        }

        _musicPlayer.Stream = currentMusic;
        _musicPlayer.Play();
    }

    public void PlayMainMenuMusic() => PlayMusic(_mainMusic);

    public void PlayLevelOneMusic() => PlayMusic(_levelOneMusic);

    public void PlayLevelTwoMusic() => PlayMusic(_levelTwoMusic);

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

        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), volumeDb);
        EmitSignal(SignalName.VolumeChanged, (float)newVolume);   
    }
}
