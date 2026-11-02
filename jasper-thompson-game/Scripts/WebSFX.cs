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
