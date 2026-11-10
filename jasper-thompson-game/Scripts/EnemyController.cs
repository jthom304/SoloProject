/*******************************************************************/
/*Filename: EnemyController.cs
/ * Author: Jasper Thompson
/ * Major: Game Dev/ Animation
/ * Date Created: 11/04/26
/ * Last Modified: 11/08/26
/ * Due Date: 10/28/26
/ * Course: Cpsc 320
/ * Professor Name: Prof. Jacklitsch
/ * Assignment: Project One - Solo Project
/ * Purpose: Handle enemy movement and commands
/*******************************************************************/
using Godot;
using System;

public partial class EnemyController : CharacterBody2D
{
    private PlayerController _playerObject;

    public const float PixelsPerMeter = 200f; //units of distance

    [Export]
    public float EnemySpeed = 0.5f; //speed of travel
    [Export]
    public float BoostSpeed = 1.0f;//max speed
    [Export]
    public float SpeedIncrease = 0.01f;
    [Export]
    private float _currentSpeed; //val for storing speed

    private float _previousSpeed; //val for storing speed

    private float _stopTimer = 0f; //time left before enemy can move again
    private bool _isWaiting = false; //if enemy is waiting to speed up 
    private bool _canMove = false; //permission to move sire?


    private bool _isTransitioning { get; set; } = false;

    private NavigationAgent2D _navAgent; //for nav region | taken 11/7/26 idea from https://forum.godotengine.org/t/how-do-i-make-the-enemy-follow-the-player-in-godot-4-3/83826/2

    /*
    Name: _Ready
    Description: get nav agent and enemy speed
    Parameters: None
    Returns: None
    */
    public override void _Ready()
    {
        _navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");

        _navAgent.PathDesiredDistance = 4f;//taken 11/7/26 https://docs.godotengine.org/en/latest/classes/class_navigationagent2d.html#class-navigationagent2d-property-path-desired-distance
        _navAgent.TargetDesiredDistance = 10f; //taken from same doc ^

        _currentSpeed = EnemySpeed;
    }

    /*
    Name: SetPlayer
    Description: sets the player for enemy
    Parameters: PlayerController newPlayer
    Returns: None
    */
    public void SetPlayer(PlayerController newPlayer)
    {
        _playerObject = newPlayer;
    }

    /*
    Name: StartChasing
    Description: movement permission toggle
    Parameters: None
    Returns: None
    */
    public void StartChasing()
    {
        _canMove = true;
    }

    /*
    Name: _PhysicsProcess
    Description: checks for transition and enemy wait, calls handle movement, handles isWaiting
    Parameters: double deltaTime
    Returns: None
    */
    public override void _PhysicsProcess(double deltaTime)
    {
        if (_isTransitioning)
        {
            this.Velocity = Vector2.Zero;
            return;
        }

        HandleMovenment();

        if(_isWaiting)
        {
            _stopTimer -= (float)deltaTime;

            if(_stopTimer <= 0f)
            {
                _currentSpeed = _previousSpeed;
                _isWaiting = false;
            }
        }
    }

    /*
    Name: HandleMovement
    Description: handles enemy movement controls
    Parameters: None
    Returns: None
    */
    public void HandleMovenment()
    {
        //no movement? (megamind ref)
        if(!_canMove)
        {
            Velocity = Vector2.Zero;
            return;
        }
        if (_playerObject != null)
        {
            //inform enemy player location
            _navAgent.TargetPosition = _playerObject.GlobalPosition;
            //find next point
            Vector2 nextPoint = _navAgent.GetNextPathPosition();
            //calculate direction
            Vector2 enemyDirection = GlobalPosition.DirectionTo(nextPoint);
            //move enemy
            Velocity = enemyDirection * _currentSpeed * PixelsPerMeter;
            MoveAndSlide();
        }
        //check collison
        if (GetSlideCollisionCount() > 0) //"how to check collison with collider" taken 11/7/26 https://forum.godotengine.org/t/get-last-slide-collision-and-get-slide-collision-count-only-detecting-once/40125
        {
            if (GetSlideCollision(0).GetCollider() == _playerObject)
            {
                LevelLoader.Instance.GameManagerRef.KillPlayer();
            }
        }
    }

    /*
    Name: MoveToLevel
    Description: move enemy to new nav mesh
    Parameters: Vector2 spawnPosition, NavigationRegion2D navRegion
    Returns: None
    */
    public void MoveToLevel(Vector2 spawnPosition, NavigationRegion2D navRegion)
    {
        _isTransitioning = true;

        //move enemy
        this.GlobalPosition = spawnPosition;
        //find/update navigation region
        _navAgent.SetNavigationMap(navRegion.GetNavigationMap()); // same doc https://docs.godotengine.org/en/latest/classes/class_navigationagent2d.html#class-navigationagent2d-property-target-desired-distance
        //stopenemy
        Velocity = Vector2.Zero;

        _isTransitioning = false;
    }

    /*
    Name: SpeedUp
    Description: speeds enemy up over time
    Parameters: None
    Returns: None
    */
    public void SpeedUp()
    {
        //increase speed over time
        _currentSpeed += SpeedIncrease;
        //dont go over this 
        _currentSpeed = Mathf.Min(_currentSpeed, BoostSpeed);
    }

   /*
   Name: SlowDown
   Description: resets enemy speed and pauses for 2 seconds
   Parameters: None
   Returns: None
   */
    public void SlowDown()
    {
        //reset speed
        _previousSpeed = _currentSpeed;
        _currentSpeed = 0;

        //freeze before moving again
        _stopTimer = 2f;
        _isWaiting = true;
    }
}
