/*******************************************************************/
/*Filename: RunTimer.cs
/ * Author: Jasper Thompson
/ * Major: Game Dev/ Animation
/ * Date Created: 10/15/26
/ * Last Modified: 11/08/26
/ * Due Date: 10/28/26
/ * Course: Cpsc 320
/ * Professor Name: Prof. Jacklitsch
/ * Assignment: Project One - Solo Project
/ * Purpose: Handle game timer countdown
/*******************************************************************/
using Godot;
using System;

public partial class RunTimer : Control
{
    //timer refs for label and timer obj
    [Export]
    private Label _timeDisplay;

    [Export]
    private Timer _timerObj;

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
        _gameManager = LevelLoader.Instance.GameManagerRef;
        if (_gameManager == null )
        {
            GD.Print("No manager bro");
            return;
        }

        if (_timerObj == null || _timeDisplay == null)
        {
            GD.Print("No timer or label");
        }

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

        //speed up enemy every 5 seconds 
        if (TimeLeft % 10 == 0)
        {
            _gameManager.SpeedUpEnemy();
        }

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


