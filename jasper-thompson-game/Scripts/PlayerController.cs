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

    [Signal]
    public delegate void CrystalCollectedSignalEventHandler();

    [Export]
    private AnimatedSprite2D _playerAnimator;

    public const float PixelsPerMeter = 200f; //units of distance
    public const float JumpVelocity = -2.5f; // 
    public const float PlayerSpeed = 1f; //speed of travel
    public const float WorldGravity = 5f; //gravity var
    public const float FallGravity = 7.5f; //fall gravity var 
    public float WebSpeed = 6;
    public int WebCount { get; private set; } = 0;

    private bool _crystalCollected = false;

    public bool IsTransitioning { get; set; } = false;

    private Area2D _nearItem = null;

    /*
    Name: _Ready
    Description: sets animator reference, and finds children for pickups
    Parameters: None
    Returns: None
    */
    public override void _Ready()
    {
        _playerAnimator = GetNode<AnimatedSprite2D>("Sprite2D");

        FindPickups();
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

        if (Input.IsActionJustPressed("PickUp") && _nearItem != null)
        {
            GD.Print("Q pressed");
            if (_nearItem != null)
            {
                PickUp();
            }
            else
            {
                GD.Print("no item");
            }
            
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
        if (IsTransitioning)
        {
            this.Velocity = Vector2.Zero;
            return;
        }
        Vector2 movementVelocity = this.Velocity;

        movementVelocity = HandleMovement(movementVelocity);
        movementVelocity = HandleJump(movementVelocity, deltaTime);

        Velocity = movementVelocity;
        MoveAndSlide();
        UpdateAnimation();
    }

    /*
    Name: HandleMovement
    Description: handle movement of char
    Parameters: Vector2 movementVelocity
    Returns: None
    */
    private Vector2 HandleMovement(Vector2 movementVelocity)
    {
        movementVelocity.X = 0f;

        //action map inputs and outcomes
        if (Input.IsActionPressed("MoveBack"))
        {
            movementVelocity.X -= PlayerSpeed * PixelsPerMeter;
        }
        if (Input.IsActionPressed("MoveForward"))
        {
            movementVelocity.X += PlayerSpeed * PixelsPerMeter;
            //GD.Print("moveright");
        }

            for (int i = 0; i < GetSlideCollisionCount(); i++)
            {
                KinematicCollision2D collision = GetSlideCollision(i);
            }

        return movementVelocity;
    }

    /*
    Name: HandleJump
    Description: handle jump event and send signal to animator
    Parameters: Vector2 movementVelocity, double deltaTime
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

        return movementVelocity;
    }

    /*
    Name: UpdateAnimation
    Description: handles animation frames and calls animator
    Parameters: None
    Returns: None
    */
    private void UpdateAnimation()
    {
        //dont change in load
        if (IsTransitioning)
        {
            return;
        }
        //flip sprite
        if (Velocity.X < 0f)
        {
            _playerAnimator.FlipH = true;
        }
        else if (Velocity.X > 0f)
        {
            _playerAnimator.FlipH = false;
        }

        if (IsOnFloor())
        {
            if (Velocity.X == 0)
            {
                _playerAnimator.Play("Idle");
            }
            else if (_crystalCollected)
            {
                _playerAnimator.Play("Run");
            }
            else
            {
                _playerAnimator.Play("Walk");
            }
        }
        else if (!IsOnFloor())
        {
            if (_crystalCollected)
            {
                _playerAnimator.Play("Jump");
            }
            return;
        }
    }

    /*
    Name: UseItem
    Description: handle spawn and movement of projectile
    Parameters: None
    Returns: None
    */
    private void UseItem()
    {
        if (WebCount > 0)
        {
            //get audio clip
            GetNode<AudioStreamPlayer2D>("SplatSFX").Play();
            //get spawn location
            Vector2 spawnLocation = this.GetNode<Node2D>("ProjectileLocation").GlobalPosition;
            //so projectile shoots in right direction
            Vector2 itemDirectioN = (GetGlobalMousePosition() - spawnLocation).Normalized();
            //calculate velocity
            Vector2 projectileVelocity = itemDirectioN * PixelsPerMeter * WebSpeed;
            //set and spawn projectile
            Node newProjectile = WebProjectile.InitializeObject(spawnLocation, projectileVelocity);
            this.AddChild(newProjectile);
            //update count
            WebCount--;
            LevelLoader.Instance.GameManagerRef.UpdateWebCounter(WebCount);
        }
    }

    /*
    Name: FindPickups
    Description: combs through tree for children nodes
    Parameters: None
    Returns: None
    */
    public void FindPickups()
    {
        foreach (Node childItems in GetTree().GetNodesInGroup("PickupItems")) //call node above playerchar
        {
            if (childItems is Area2D pickupArea)
            {
                pickupArea.BodyEntered += (body) => //10/30/26 taken from https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_signals.html
                                                    //"how to handle a Godot signal without creating another method c# for checking body entered"
                {
                    if (body == this)
                    {
                        _nearItem = pickupArea;
                    }
                };

                pickupArea.BodyExited += (body) => 
                {
                    if (body == this && _nearItem == pickupArea)
                    {
                        _nearItem = null;
                    }
                };
            }
        }
    }

    /*
    Name: PickUp
    Description: Handles pick up input control
    Parameters: None
    Returns: None
    */
    private void PickUp()
    {
        if (_nearItem == null)
        {
            return;
        }

        if(_nearItem.IsInGroup ("CrystalItem"))
        {
            _crystalCollected = true;
            EmitSignal(SignalName.CrystalCollectedSignal);
        }
        else if (_nearItem.IsInGroup("WebItem"))
        {
            WebCount++;
            LevelLoader.Instance.GameManagerRef.UpdateWebCounter(WebCount);
        }
        _nearItem.QueueFree();
        _nearItem = null;
    }
}
