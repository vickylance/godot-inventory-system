using Godot;
using System;

public class Player : KinematicBody2D
{
	[Signal]
	public delegate void Aim();
	[Signal]
	public delegate void Shoot();
	[Signal]
	public delegate void Interact();

	[Export]
	public int maxSpeed = 200;
	[Export]
	public int acceleration = 500;
	[Export]
	public int friction = 500;
	[Export]
	public float turnSpeed = 0.25f;

	public GenericInventory inventory = new GenericInventory();

	private Vector2 velocity = Vector2.Zero;
	private Tween tween;

	public override void _Ready()
	{
		tween = new Tween();
		AddChild(tween);
	}

	public override void _PhysicsProcess(float delta)
	{
		var direction = GetInputAxis();
		MoveCharacter(direction, delta);
		RotateCharacter(direction);
		HandleInput();

		velocity = MoveAndSlide(velocity);
	}

	public Vector2 GetInputAxis()
	{
		var direction = Vector2.Zero;
		direction.x = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
		direction.y = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up");
		return direction.Normalized();
	}

	public void MoveCharacter(Vector2 direction, float delta)
	{
		if (Mathf.IsZeroApprox(direction.Length()))
		{
			velocity = velocity.MoveToward(Vector2.Zero, friction * delta);
		}
		else
		{
			velocity = velocity.MoveToward(direction * maxSpeed, acceleration * delta);
		}
	}

	public void RotateCharacter(Vector2 direction)
	{
		if (Input.IsActionPressed("aim"))
		{
			LookAt(GetGlobalMousePosition());
			EmitSignal(nameof(Aim));
		}
		else if (direction != Vector2.Zero)
		{
			Rotation = Mathf.LerpAngle(Rotation, direction.AngleToPoint(Vector2.Zero), turnSpeed);
		}
	}

	public void HandleInput()
	{
		if (Input.IsActionPressed("shoot"))
		{
			EmitSignal(nameof(Shoot));
		}
		if (Input.IsActionJustPressed("interact"))
		{
			EmitSignal(nameof(Interact));
		}
	}
}
