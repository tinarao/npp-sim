using Godot;
using System;

public partial class Player : CharacterBody3D
{
	private Node3D _head;
	private Camera3D _camera;
	private float BobFrequency = 2f;
	private const float BOB_AMPL = 0.05f;

	public float BobPos = 0f;
	public float Speed = 0.0f;
	public const float WalkSpeed = 3.0f;
	public const float SprintSpeed = 6.0f;
	public const float JumpVelocity = 4.5f;

    public override void _Ready()
    {
        base._Ready();
		_head = GetNode<Node3D>("Head");
		_camera = _head.GetNode<Camera3D>("Camera3D");
    }

	private Vector3 headBob(float time)
	{
		var _pos = Vector3.Zero;
		_pos.Y = (float)Math.Sin(time * BobFrequency) * BOB_AMPL;
		_pos.X = (float)Math.Cos(time * BobFrequency / 2) * BOB_AMPL;
		return _pos;
	}

    public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		if (Input.IsActionPressed("run") && IsOnFloor())
		{
			Speed = SprintSpeed;
			BobFrequency = 3.0f;
		} 
		else 
		{
			Speed = WalkSpeed;
			BobFrequency = 2.0f;
		}

		Vector2 inputDir = Input.GetVector("walk_left", "walk_right", "walk_front", "walk_back");
		Vector3 direction = (_head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		if (IsOnFloor())
		{
			if (direction != Vector3.Zero)
			{
				velocity.X = direction.X * Speed;
				velocity.Z = direction.Z * Speed;
			}
			else
			{
			velocity.X = Mathf.Lerp(velocity.X, direction.X * Speed, (float)delta * 7f);
			velocity.Z = Mathf.Lerp(velocity.Z, direction.Z * Speed, (float)delta * 7f);
			}
		}
		else
		{
			velocity.X = Mathf.Lerp(velocity.X, direction.X * Speed, (float)delta * 4f);
			velocity.Z = Mathf.Lerp(velocity.Z, direction.Z * Speed, (float)delta * 4f);
		}

		Velocity = velocity;
		float _isOnFloorFloat = IsOnFloor() ? 1f : 0f;
		BobPos += (float)delta * velocity.Length() * _isOnFloorFloat;

		var _transform = _camera.Transform;
		_transform.Origin = headBob(BobPos);
		_camera.Transform = _transform;

		MoveAndSlide();
	}
}
