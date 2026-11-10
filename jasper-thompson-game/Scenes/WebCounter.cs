/*******************************************************************/
/*Filename: WebCounter.cs
/ * Author: Jasper Thompson
/ * Major: Game Dev/ Animation
/ * Date Created: 11/09/26
/ * Last Modified: 11/09/26
/ * Due Date: 10/28/26
/ * Course: Cpsc 320
/ * Professor Name: Prof. Jacklitsch
/ * Assignment: Project One - Solo Project
/ * Purpose: Handle update of HUD element
/*******************************************************************/
using Godot;
using System;

public partial class WebCounter : Control
{
    [Export]
    private Label _webCountLabel;

    /*
    Name: UpdateCounter
    Description: updates HUD label for web counter
    Parameters: int webAmount
    Returns: None
    */
    public void UpdateCounter(int webAmount)
    {
        _webCountLabel.Text = webAmount.ToString();
    }
}
