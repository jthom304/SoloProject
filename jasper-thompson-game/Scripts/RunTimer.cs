using Godot;
using System;

public partial class RunTimer : Control
{
    //timer refs for label and timer obj
    [Export]
    private Label _timeDisplay;

    [Export]
    private Timer _timerObj;

    //ref to player
    [Export]
    private GameManager _gameManager;

    public float TimeLeft; //TimeLeft in timer countdown

    /*
        Name: _Ready
        Description: set time var and update TimeLeft, when key is pressed send signal
        Parameters: None
        Returns: None
    */
    public override void _Ready()
    {
        TimeLeft = 60;
        UpdateTime(TimeLeft);

        _timerObj.Timeout += TimerDone;
        _timerObj.Start();
    }

    /*
        Name: TimerDone
        Description: When time is up check timer and stop it when it reaches 0
        Parameters: None
        Returns: None
    */
    private void TimerDone()
    {
        TimeLeft--;

        UpdateTime(TimeLeft);

        _gameManager.CheckTimer(TimeLeft);

        if (TimeLeft <= 0)
        {
            _timerObj.Stop();
        }
    }

    /*
        Name: UpdateTime
        Description: Updates currentTime to HUD element
        Parameters: float currentTime
        Returns: None
    */
    public void UpdateTime(float currentTime)
    {
        _timeDisplay.Text = "Time: " + currentTime.ToString("0");
    }
}


