using Godot;
using System;

public class Gun : Sprite
{
	[Export]
	public float rotationSpeed = 90f;
	[Export]
	public float offsetAngle = 0f;

	public override void _Ready()
	{
		rotationSpeed = Mathf.Deg2Rad(rotationSpeed);
		offsetAngle = Mathf.Deg2Rad(offsetAngle);
	}

	public override void _Process(float delta)
	{
		float targetAngle = GetGlobalMousePosition().AngleToPoint(Position) + offsetAngle;
		if (Rotation != targetAngle)
		{
			float angleDif = -shortAngleDist(Rotation, targetAngle);
			if (Mathf.Abs(angleDif) < 0.05f)
			{
				Rotation = targetAngle;
			}
			else
			{
				Rotation = Rotation - rotationSpeed * delta * Math.Sign(angleDif);
			}
		}
	}

	// public float Map(float x, float in_min, float in_max, float out_min, float out_max)
	// {
	// 	return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
	// }

	public float shortAngleDist(float from, float to)
	{
		var max_angle = Mathf.Tau;
		var difference = (to - from) % max_angle;
		return ((2 * difference) % max_angle) - difference;
	}
}
