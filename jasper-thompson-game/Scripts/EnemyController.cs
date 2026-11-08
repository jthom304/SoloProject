using Godot;
using System;

public partial class EnemyController : CharacterBody2D
{
    private PlayerController playerObject;

    public const float PixelsPerMeter = 200f; //units of distance

    [Export]
    public float EnemySpeed = 0.5f; //speed of travel
    [Export]
    public float BoostSpeed = 1.0f;//max speed
    [Export]
    public float SpeedIncrease = 0.01f;
    [Export]
    private float currentSpeed; //val for storing speed


    public bool IsTransitioning { get; set; } = false;

    private NavigationAgent2D navAgent; //for nav region | taken 11/7/26 idea from https://forum.godotengine.org/t/how-do-i-make-the-enemy-follow-the-player-in-godot-4-3/83826/2

    public override void _Ready()
    {
        navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");

        navAgent.PathDesiredDistance = 4f;//taken 11/7/26 https://docs.godotengine.org/en/latest/classes/class_navigationagent2d.html#class-navigationagent2d-property-path-desired-distance
        navAgent.TargetDesiredDistance = 10f; //taken from same doc ^

        currentSpeed = EnemySpeed;
    }

    public void SetPlayer(PlayerController newPlayer)
    {
        playerObject = newPlayer;
    }

    public override void _PhysicsProcess(double deltaTime)
    {
        if (IsTransitioning)
        {
            this.Velocity = Vector2.Zero;
            return;
        }

        HandleMovenment();
        //CatchPlayer
        //LoseSpeed
    }

    public void HandleMovenment()
    {
        if (playerObject != null)
        {
            //inform enemy player location
            navAgent.TargetPosition = playerObject.GlobalPosition;
            //find next point
            Vector2 nextPoint = navAgent.GetNextPathPosition();
            //calculate direction
            Vector2 enemyDirection = GlobalPosition.DirectionTo(nextPoint);
            //move enemy
            Velocity = enemyDirection * currentSpeed * PixelsPerMeter;
            MoveAndSlide();
        }
        //check collison
        if (GetSlideCollisionCount() > 0)
        {
            if (GetSlideCollision(0).GetCollider() == playerObject)
            {
                LevelLoader.Instance.GameManagerRef.KillPlayer();
            }
        }
    }

    public void MoveToLevel(Vector2 spawnPosition, NavigationRegion2D navRegion)
    {
        IsTransitioning = true;

        //move enemy
        this.GlobalPosition = spawnPosition;
        //find/update navigation region
        navAgent.SetNavigationMap(navRegion.GetNavigationMap()); // same doc https://docs.godotengine.org/en/latest/classes/class_navigationagent2d.html#class-navigationagent2d-property-target-desired-distance
        //stopenemy
        Velocity = Vector2.Zero;

        IsTransitioning = false;
    }

    public void SpeedUp()
    {
        currentSpeed += SpeedIncrease;
        //dont go over this 
        currentSpeed = Mathf.Min(currentSpeed, BoostSpeed);
    }

    public void SlowDown()
    {
        currentSpeed = EnemySpeed;
    }
}
