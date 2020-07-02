using Godot;
using System;

public class DamagePopup : CanvasLayer
{
	private Label popupText;
	private Tween floatingTween;

	public override void _Ready()
	{
		popupText = FindNode("PopupText") as Label;
		floatingTween = FindNode("FloatingTween") as Tween;
	}

	public void Init(string damageValue, Vector2 position)
	{
		popupText.Text = damageValue;
	}
}
