/*******************************************************************/
/* Filename: WebProjectile.cs */
/* Author: Jasper Thompson */
/* Major: Ani/ GameDev */
/* Creation Date: October 19, 2026 */
/* Last Modified: October 19, 2026 */
/* Due Date: October 28, 2026 */
/* Course: CPSC320-010 */
/* Professor Name: Professor Jacklitsch */
/* Assignment: Project One - Solo Project */
/* Purpose: Handles intialization and collison of projectile obj */
/*******************************************************************/
using Godot;
using System;

public partial class WebProjectile : CharacterBody2D
{
    public Vector2 ProjectileVelocity { get; set; } //velocity for projectile

    /*
        Name: InitializeObject
        Description: initialize projectile 
        Parameters: Vector2 spawnPosition - where it will spawn, Vector2 Velocity - speed var 
        Returns: None
    */
    //public static WebProjectile InitializeObject(Vector2 spawnPosition, Vector2 initialVelocity)
    //{
    //    WebProjectile newProjectile = (CharProjectile)ResourceLoader.Load<PackedScene>("res://Scenes/web_projectile.tscn").Instantiate();
    //    newProjectile.Position = spawnPosition;
    //    newProjectile.TopLevel = true;
    //    //GD.Print("Shot");
    //    newProjectile.ProjectileVelocity = initialVelocity;
    //    return newProjectile;
    //}

    /*
        Name: _PhysicsProcess
        Description: handles collision of projectiles
        Parameters: double deltaTime
        Returns: None
    */
    public override void _PhysicsProcess(double deltaTime)
    {
        KinematicCollision2D collidingObject = MoveAndCollide(ProjectileVelocity * (float)deltaTime);
        {
            //checking for collision
            if (collidingObject == null)
            {
                return;

            }
            //get obj that was hit
            Node collidedObj = (Node)collidingObject.GetCollider();

            if (collidedObj == null)
            {
                QueueFree();
                return;
            }
            else if (collidedObj != null)
            {
                // if get collider = BFOF 
                //if (collidedObj.IsInGroup("Targets"))
                //{
                //    //GD.Print("Is Target");
                //    collidedObj.QueueFree();
                //    QueueFree();

                //    int targetsLeft = GetTree().GetNodesInGroup("Targets").Count - 1;
                //    //GD.Print("targets remaining");

                //}
                if (collidedObj.IsInGroup("Wall"))
                {
                    //GD.Print("Is Wall");
                }
                this.QueueFree();
                return;
            }
        }
    }
}