/*******************************************************************/
/*Filename: PlayerController.cs
/ * Author: Jasper Thompson
/ * Major: Game Dev/ Animation
/ * Date Created: 10/15/26
/ * Last Modified: 10/15/26
/ * Due Date: 10/28/26
/ * Course: Cpsc 320
/ * Professor Name: Prof. Jacklitsch
/ * Assignment: Project One - Solo Project
/ * Purpose: Handle player movement, animation, and commands
/*******************************************************************/

using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
    [Signal]
    public delegate void StartedJumpEventHandler();

    //[Export] 
    //private PlayerAnimationController _animationController;

    public const float UnitsPerMeter = 200; //units of distance
    public const float JumpVelocity = -2.5f; // 
    public const float PlayerSpeed = 1.5f; //speed of travel
    public const float WorldGravity = 1000; //gravity var
    public const float FallGravity = 1500; //fall gravity var 
    public float WebSpeed = 10;

    /*
     Name: _Process
     Description: check for action to shoot projectile
     Parameters: doube delta
     Returns: None
 */
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("UseItem"))
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
        Vector2 velocityHold = this.Velocity; //grab velocity
        //apply Velocity to methods
        velocityHold = HandleMovement(velocityHold);
        velocityHold = HandleJump(velocityHold, (float)deltaTime);
        //save var
        this.Velocity = velocityHold;
        //move with new velocity
        MoveAndSlide();
    }

    /*
       Name: HandleMovement
       Description: handle movement of char
       Parameters: doube deltaTime
       Returns: None
   */
    private Vector2 HandleMovement(Vector2 currVelocity)
    {
        Vector2 movementVelocity = new Vector2();

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

        //Update velocity and movement multipled by distance, time, and speed
        currVelocity.X = movementVelocity.X * PlayerSpeed * UnitsPerMeter;
        return currVelocity;
    }

    /*
       Name: HandleJump
       Description: handle jump event and send signal to animator
       Parameters: doube deltaTime
       Returns: None
   */
    private Vector2 HandleJump(Vector2 currentVelocity, double deltaTime)
    {
        if (!IsOnFloor())
        {
            float currentGravity; //placeholder
            if (currentVelocity.Y > 0) 
            {
                currentGravity = FallGravity; //if char is falling move faster
            }
            else
            {
                currentGravity = WorldGravity; // if jumping move slower than descent
            }
            currentVelocity.Y += currentGravity * (float)deltaTime;
        }
        else
        {
            currentVelocity.Y = 0;
        }
        //Jump Action
        if (Input.IsActionJustPressed("JumpUp") && IsOnFloor())
        {
            currentVelocity.Y += JumpVelocity * UnitsPerMeter;
            //EmitSignal(SignalName.StartedJump)
        }
        return currentVelocity;       
    }

    /*
       Name: UseItem
       Description: handle spawn and movement of projectile
       Parameters: doube deltaTime
       Returns: None
   */
    private void UseItem()
    {
        ////get spawn location
        //Vector2 spawnLocation = this.GetNode<Node2D>("ProjectileLocation").GlobalPosition;
        ////so projectile shoots in right direction
        //Vector2 projectileVelocity = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * UnitsPerMeter * ProjectileSpeed;
        ////set and spawn projectile
        //Node newProjectile = CharProjectile.InitializeObject(spawnLocation, projectileVelocity);
        //this.AddChild(newProjectile);
        ////GD.Print("Shot1");
    }

    private void HandleMouse()
    {
        //LookAt(GetGlobalMousePosition()); //looking at where the mouse is
    }
}
