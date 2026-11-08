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

    public override void _Ready()
    {
        this.Visible = false;
        screenAnimator.AnimationFinished += OnFadeFinished;
    }

    public void FadeIn()
    {
        this.Visible = true;
        screenAnimator.Play("FadeIn");
    }

    public void FadeOut()
    {
        screenAnimator.Play("FadeOut");
    }

    public void OnFadeFinished(StringName finishedAnimation)
    {
        GD.Print("Animation Finished: " + finishedAnimation);
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
