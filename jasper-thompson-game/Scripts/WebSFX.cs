/*******************************************************************/
/*Filename: WebSFX.cs
/ * Author: Jasper Thompson
/ * Major: Game Dev/ Animation
/ * Date Created: 10/15/26
/ * Last Modified: 10/15/26
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
    public void InitalizeObject(AudioStream desiredAudio, Vector2 audioPosition)
    {
        this.Stream = desiredAudio;
        this.Position = audioPosition;
        this.Bus = "Master";

        this.Finished += DeleteSound;
    }
    
    public void DeleteSound()
    {
        this.QueueFree();
    }
        
}
