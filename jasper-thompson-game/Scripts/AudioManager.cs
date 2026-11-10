/*******************************************************************/
/*Filename: AudioManager.cs
/ * Author: Jasper Thompson
/ * Major: Game Dev/ Animation
/ * Date Created: 10/31/26
/ * Last Modified: 11/08/26
/ * Due Date: 10/28/26
/ * Course: Cpsc 320
/ * Professor Name: Prof. Jacklitsch
/ * Assignment: Project One - Solo Project
/ * Purpose: Handle audio switching
/*******************************************************************/
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

    /*
    Name: _Ready
    Description: intializes instance, calls music methods
    Parameters: None
    Returns: None
    */
    public override void _Ready()
    {
        Instance = this;
        PlayMusic(_mainMusic);
        UpdateVolume(GlobalVariables.GameVolume);
    }

    /*
    Name: _ExitTree
    Description: when node leaves tree it is null
    Parameters: None
    Returns: None
    */
    public override void _ExitTree()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }

    /*
    Name: PlayMusic
    Description: Handles playing music by calling stream player
    Parameters: AudioStream currentMusic
    Returns: None
    */
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

    /*
    Name: PlayMainMenuMusic
    Description: calls play music to play main music audio stream 
    Parameters: None
    Returns: None
    */
    public void PlayMainMenuMusic() => PlayMusic(_mainMusic);
    /*
    Name: PlayLevelOneMusic
    Description: calls play music to play level one music audio stream 
    Parameters: None
    Returns: None
    */
    public void PlayLevelOneMusic() => PlayMusic(_levelOneMusic);
    /*
    ame: PlayLevelTwoMusic
    Description: calls play music to play level two music audio stream
    Parameters: None
    Returns: None
    */
    public void PlayLevelTwoMusic() => PlayMusic(_levelTwoMusic);

    /*
    Name: UpdateVolume
    Description: Change global variable volume value 
    Parameters: double newVolume
    Returns: None
    */
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
