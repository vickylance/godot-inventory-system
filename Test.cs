using Godot;
using System;

public class Test : Node2D
{
	public override void _Ready()
	{
		GetNode("../Button").Connect("pressed", this, nameof(_OnButtonPressed));
	}

	public void _OnButtonPressed()
	{
		GetNode<Label>("../Label").Text = "HELLO!";
	}
}
