using Godot;
using System;

public class JoystickTest : KinematicBody2D
{
	public ThumbStick thumbStick;

	public override void _Ready()
	{
		thumbStick = (ThumbStick)GetTree().Root.FindNode("ThumbStick", true, false);
	}

	public override void _Process(float delta)
	{
		MoveAndSlide(thumbStick.GetValue() * 300);
	}
}
