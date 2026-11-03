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
    public delegate void EnteredPortalEventHandler(Area2D cameraZone);

    [Signal]
    public delegate void StartedJumpEventHandler();

    [Export]
    private PlayerAnimator _playerAnimator;

    public const float PixelsPerMeter = 200f; //units of distance
    public const float JumpVelocity = -2.5f; // 
    public const float PlayerSpeed = 1f; //speed of travel
    public const float WorldGravity = 5f; //gravity var
    public const float FallGravity = 7.5f; //fall gravity var 
    public float WebSpeed = 10;
    private int WebCount = 0;

    private bool CrystalCollected = false;

    private Area2D nearItem = null;

    public override void _Ready()
    {
        foreach (Node childItems in GetParent().GetChildren()) //call node above playerchar
        {
            if(childItems is Area2D pickupArea)
            {
                if(pickupArea.Name == "CrystalItem" || pickupArea.IsInGroup("WebItem"))
                {
                    pickupArea.BodyEntered += (body) => //10/30/26 taken from
                    {
                        if (body == this)
                        {
                            nearItem = pickupArea;
                        }
                    };

                    pickupArea.BodyExited += (body) =>
                    {
                        if (body == this && nearItem == pickupArea)
                        {
                            nearItem = null;
                        }
                    };
                }
            }
        }
    }

    /*
     Name: _Process
     Description: check for action to shoot projectile or pick up item
     Parameters: doube delta
     Returns: None
 */
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("UseItem"))
        {
            UseItem();
        }

        if (Input.IsActionJustPressed("PickUp") && nearItem != null)
        {
            PickUp();
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
        Vector2 movementVelocity = Velocity;

        movementVelocity = HandleMovement(movementVelocity);
        movementVelocity = HandleJump(movementVelocity, deltaTime);

        Velocity = movementVelocity;
        MoveAndSlide();

        GD.Print("Player position: ", GlobalPosition);
        //Vector2 movementVelocity = this.Velocity;

        //movementVelocity = HandleMovement(movementVelocity);

        //movementVelocity = HandleJump(movementVelocity, deltaTime);

        //Velocity = movementVelocity;
        //MoveAndSlide();
    }

    /*
       Name: HandleMovement
       Description: handle movement of char
       Parameters: doube deltaTime
       Returns: None
   */
    private Vector2 HandleMovement(Vector2 movementVelocity)
    {
        movementVelocity.X = 0f;

        //action map inputs and outcomes
        if (Input.IsActionPressed("MoveBack"))
        {
            movementVelocity.X -= PlayerSpeed * PixelsPerMeter;
            //move left animation
            GD.Print("moveleft");
        }
        if (Input.IsActionPressed("MoveForward"))
        {
            movementVelocity.X += PlayerSpeed * PixelsPerMeter;
            //move right animation
            GD.Print("moveright");
        }

        return movementVelocity;
    }

    /*
       Name: HandleJump
       Description: handle jump event and send signal to animator
       Parameters: doube deltaTime
       Returns: None
   */
    private Vector2 HandleJump(Vector2 movementVelocity, double deltaTime)
    {
        if (IsOnFloor())
        {
            if (movementVelocity.Y > 0)
            {
                //stop on floor
                movementVelocity.Y = 0;
            }
        }
        else
        {
            float currentGravity = WorldGravity; //normal gravity
            if (movementVelocity.Y > 0)
            {
                currentGravity = FallGravity; //if char is falling move faster
            }
            //converting to pixels
            movementVelocity.Y += currentGravity * PixelsPerMeter * (float)deltaTime;
        }
        //Jump Action
        if (Input.IsActionJustPressed("JumpUp") && IsOnFloor())
        {
            movementVelocity.Y = JumpVelocity * PixelsPerMeter;
            
        }  

        if(CrystalCollected == true)
        {
            EmitSignal(SignalName.StartedJump);
        }
        return movementVelocity;
    }

    /*
       Name: UseItem
       Description: handle spawn and movement of projectile
       Parameters: doube deltaTime
       Returns: None
   */
    private void UseItem()
    {
        if (WebCount > 0)
        {
            GetNode<AudioStreamPlayer2D>("SplatSFX").Play();
            //get spawn location
            Vector2 spawnLocation = this.GetNode<Node2D>("ProjectileLocation").GlobalPosition;
            //so projectile shoots in right direction
            Vector2 projectileVelocity = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * PixelsPerMeter * WebSpeed;
            //set and spawn projectile
            Node newProjectile = WebProjectile.InitializeObject(spawnLocation, projectileVelocity);
            this.AddChild(newProjectile);
            WebCount--;
            //GD.Print("Shot1");
        }
    }

    private void HandleMouse()
    {
        LookAt(GetGlobalMousePosition()); //looking at where the mouse is
    }

    private void PickUp()
    {
        if (nearItem == null)
        {
            return;
        }

        if(nearItem.Name == "CrystalItem")
        {
            CrystalCollected = true;
            nearItem.QueueFree();
            //switch animation to coin run
            //trigger BFOF spawn
        }
        else if (nearItem.IsInGroup("WebItem"))
        {
            WebCount++;
            nearItem.QueueFree();
        }

        nearItem = null;
    }
}
