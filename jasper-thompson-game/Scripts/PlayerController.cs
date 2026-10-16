/*
 * Filename: PlayerController.cs
 * Author: Jasper Thompson
 * Major: Game Dev/ Animation
 * Date Created: 10/15/26
 * Last Modified: 10/15/26
 * Due Date: 10/28/26
 * Course: Cpsc 320
 * Professor Name: Prof. Jacklitsch
 * Assignment: Project One - Solo Project
 * Purpose: Handle player movement, animation, and commands
 */

using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
    [Signal]
    public delegate void StartedJumpEventHandler();

    [Export] 
    private PlayerAnimationController _animationController;

    public const float UnitsPerMeter = 200; //units of distance
    public const float JumpVelocity = -25f // 
    public const float PlayerSpeed = 15f //speed of travel
    public float WebSpeed = 10;

    Vector2 StartVelocity = new Vector2();

    /*
     Name: _Process
     Description: check for action to shoot projectile
     Parameters: doube delta
     Returns: None
 */
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("UseItem")
        {
            UseItem();
        }
    }

    /*
      Name: _PhysicsProcess
      Description: call movement and rotation methods
      Parameters: doube deltaTime
      Returns: None
  */
    public override void _PhysicsProcess(double deltaTime)
    {
       HandleMovement(deltaTime);
        HandleJump();

    }

    /*
       Name: HandleMovement
       Description: handle movement of char
       Parameters: doube deltaTime
       Returns: None
   */
    private void HandleMovement(double deltaTime)
    {

        //action map inputs and outcomes
        if (Input.IsActionPressed("MoveBack"))
        {
            movementVelocity += Vector2.Left;
        }
        if (Input.IsActionPressed("MoveForward"))
        {
            movementVelocity += Vector2.Right;
        }
      
        if (movementVelocity != Vector2.Zero)
        {
            //normalized value of velocity movement
            movementVelocity /= (float)Math.Sqrt((movementVelocity.X * movementVelocity.X) + (movementVelocity.Y * movementVelocity.Y));
        }

        //movement method multipled by distance and time
        MoveAndCollide(movementVelocity * UnitsPerMeter * (float)deltaTime);
    }

    /*
       Name: HandleJump
       Description: handle jump event and send signal to animator
       Parameters: doube deltaTime
       Returns: None
   */
    private void HandleJump()
    {
        if (Input.IsActionJustPressed("JumpUp"))
        {
            //add gravity
            if (!IsOnFloor())
            {
                //gravity
            }
            //jump action
            if (Input.IsActionJustPressed("Jump") && IsOnFloor())
            {
                currentVelocity.Y = JumpVelocity * PixelsPerMeter;
                EmitSignal(SignalName.StartedJump);
            }
        }
    }

    /*
       Name: UseItem
       Description: handle spawn and movement of projectile
       Parameters: doube deltaTime
       Returns: None
   */
    private void UseItem()
    {
        //get spane location
        Vector2 spawnLocation = this.GetNode<Node2D>("ProjectileLocation").GlobalPosition;
        //so projectile shoots in right direction
        Vector2 projectileVelocity = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * UnitsPerMeter * ProjectileSpeed;
        //set and spawn projectile
        Node newProjectile = CharProjectile.InitializeObject(spawnLocation, projectileVelocity);
        this.AddChild(newProjectile);
        //GD.Print("Shot1");
    }
}
