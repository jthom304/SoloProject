/*******************************************************************/
/*Filename: CameraZone.cs
/ * Author: Prof. Jacklitsch
/ * Editor: Jasper Thompson
/ * Major: Game Dev/ Animation
/ * Date Created: 10/28/26
/ * Last Modified: 11/08/26
/ * Due Date: 10/28/26
/ * Course: Cpsc 320
/ * Professor Name: Prof. Jacklitsch
/ * Assignment: Project One - Solo Project
/ * Purpose:  detects when player enters camera area and switches to 
/ * active zone
/*******************************************************************/
using Godot;
using System;

public partial class CameraZone : Area2D
{
    /*
    Name: _Ready
    Description: connects signals
    Parameters: None
    Returns: None
    */
    public override void _Ready()
    {
        BodyEntered += PlayerEntered;
    }

    /*
    Name: PlayerEntered
    Description: if player managed by game manager enters area activate zone
    Parameters: Node2D playerBody
    Returns: None
    */
    public void PlayerEntered(Node2D playerBody)
    {
        if (playerBody is PlayerController playerObj && playerObj == LevelLoader.Instance.GameManagerRef.PlayerRef)
        {
            ActivateZone();
        }
    }

    /*
    Name: ActivateZone
    Description: search for valid camera and switch its zone
    Parameters: None
    Returns: None
    */
    public void ActivateZone()
    {
        CameraController cameraBody = LevelLoader.Instance.GameManagerRef.CameraObj;

        if (cameraBody == null)
        {
            GD.Print("No camera");
            return;
        }
        cameraBody.SwitchZone(this);
        GD.Print("Camera Zone Activated: " + this.Name);
    }
}
