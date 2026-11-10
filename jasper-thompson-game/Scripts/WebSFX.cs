/*******************************************************************/
/*Filename: WebSFX.cs
/ * Author: Jasper Thompson
/ * Major: Game Dev/ Animation
/ * Date Created: 11/02/26
/ * Last Modified: 11/02/26
/ * Due Date: 10/28/26
/ * Course: Cpsc 320
/ * Professor Name: Prof. Jacklitsch
/ * Assignment: Project One - Solo Project
/ * Purpose: Play sound effect
/*******************************************************************/
using Godot;
using System;

public partial class WebSFX : AudioStreamPlayer2D
{
    /*
    Name: InitializeObject
    Description: intializes sfx audio stream
    Parameters: AudioStream desiredAudio, Vector2 audioPosition
    Returns: None
    */
    public void InitalizeObject(AudioStream desiredAudio, Vector2 audioPosition)
    {
        this.Stream = desiredAudio;
        this.Position = audioPosition;
        this.Bus = "Master";

        this.Finished += DeleteSound;
    }

    /*
    Name: DeleteSound
    Description: deletes audio stream 
    Parameters: None
    Returns: None
    */
    public void DeleteSound()
    {
        this.QueueFree();
    }
        
}
