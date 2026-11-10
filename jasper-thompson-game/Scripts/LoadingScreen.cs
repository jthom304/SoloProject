/*******************************************************************/
/*Filename: LoadingScreen.cs
/ * Author: Prof. Jacklitsch
/ * Editor: Jasper Thompson
/ * Major: Game Dev/ Animation
/ * Date Created: 10/28/26
/ * Last Modified: 11/08/26
/ * Due Date: 10/28/26
/ * Course: Cpsc 320
/ * Professor Name: Prof. Jacklitsch
/ * Assignment: Project One - Solo Project
/ * Purpose: 
/*******************************************************************/
using Godot;
using System;

public partial class LoadingScreen : CanvasLayer
{
    [Signal]
    public delegate void FadedOutEventHandler();

    [Signal]
    public delegate void FadedInEventHandler();

    private string _loadingScene;

    [Export]
    public AnimationPlayer screenAnimator;

    /*
    Name: _Ready
    Description: connect signals
    Parameters: None
    Returns: None
    */
    public override void _Ready()
    {
        this.Visible = false;
        screenAnimator.AnimationFinished += OnFadeFinished;
    }

    /*
    Name: FadeIn
    Description: plays fade in animation
    Parameters: None
    Returns: None
    */
    public void FadeIn()
    {
        this.Visible = true;
        screenAnimator.Play("FadeIn");
    }

    /*
    Name: FadeOut
    Description: plays fade out animation
    Parameters: None
    Returns: None
    */
    public void FadeOut()
    {
        screenAnimator.Play("FadeOut");
    }

    /*
    Name: OnFadeFinished
    Description: control fade animation, emits signal when necessary
    Parameters: StringName finishedAnimation
    Returns: None
    */
    public void OnFadeFinished(StringName finishedAnimation)
    {
        if (finishedAnimation == "FadeIn")
        {
            EmitSignal(SignalName.FadedIn);
        }
        else if (finishedAnimation == "FadeOut")
        {
            EmitSignal(SignalName.FadedOut);
            this.Visible = false;
        }
    }
}
