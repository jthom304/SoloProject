using Godot;
using System;

public partial class MainMenu : Control
{
    //button ref
    [Export]
    private Button _startButton;
    [Export]
    private Button _optionsButton;
    [Export]
    private Button _creditsButton;
    [Export]
    private Button _exitButton;
    //[Export]
    //private Button _backButton;

    /*
        Name: _Ready
        Description: when key is pressed send signal
        Parameters: None
        Returns: None
    */
    public override void _Ready()
    {
        //connect buttons to methods
        _startButton.Pressed += StartGame; //call to start game
        _optionsButton.Pressed += OptionsPressed; // call to show options menu
        _creditsButton.Pressed += CreditsPressed; // call to show credits menu
        _exitButton.Pressed += ExitGame; //call to end game
        //_backButton.Pressed += BackPressed; //call to show main menu and hide others
    }

    /*
       Name: StartGame
       Description: hides MainMenu UI element on button press
       Parameters: None
       Returns: None
   */
    public void StartGame()
    {
        Visible = false; //hide ui menu

        if(LevelLoader.Instance == null) //check
        {
            GD.Print("Level loader instance is null");
            return;
        }
        LevelLoader.Instance.SwitchScene(LevelLoader.Instance.StartLevelScene);
        //switch the scene to new scene
    }

    public void OptionsPressed()
    { 
        //optionsMenu.visible = true;
    }

    public void CreditsPressed()
    {
        //creditsMenu.visible = true;
    }

    public void ExitGame()
    {
        GetTree().Quit();
    }

    public void BackPressed()
    {
        //optionsMenu.visible = false;
        //creditsMenu.visible = false;
    }
}
