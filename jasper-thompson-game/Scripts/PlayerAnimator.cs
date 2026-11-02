using Godot;
using System;
using System.Threading.Tasks;

public partial class PlayerAnimator : AnimationTree
{
    [Export]
    private PlayerController _playerController;

    public bool isMoving;

    public float FreeFallThresholdVelocity = PlayerController.PixelsPerMeter * 4;
    public float FallingThresholdVelocity = PlayerController.PixelsPerMeter * 10;

    public float yMovementSpeed;

    public bool IsGrounded;

    public bool IsJumping;

    public override void _EnterTree()
    {
        _playerController.StartedJump += StartJump;
    }

    public override void _ExitTree()
    {
        _playerController.StartedJump -= StartJump;
    }

    public override void _Process(double delta)
    {
        isMoving = _playerController.Velocity.X != 0;
        yMovementSpeed = _playerController.Velocity.Y;

        if(isMoving)
        {
            this.Set("parameters/Idle?blend_position", _playerController.Velocity.Normalized().X);
        }

        IsGrounded = _playerController.IsOnFloor();
        if (IsGrounded )
        {
            IsJumping = false;
        }
    }

    public void StartJump()
    {
        IsJumping = true;
    }
}
