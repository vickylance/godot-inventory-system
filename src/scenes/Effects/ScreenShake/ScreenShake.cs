using Godot;
using System;

public class ScreenShake : Node
{
	[Export]
	public Tween.TransitionType transType = Tween.TransitionType.Sine;
	[Export]
	public Tween.EaseType easeType = Tween.EaseType.InOut;

	private float amplitude = 0f;
	private int priority = 0;
	private Camera2D cam;
	private Tween shakeTween;
	private Timer frequency;
	private Timer duration;

	public override void _Ready()
	{
		cam = GetParent() as Camera2D;
		shakeTween = FindNode("ShakeTween") as Tween;
		frequency = FindNode("Frequency") as Timer;
		duration = FindNode("Duration") as Timer;

		frequency.Connect("timeout", this, nameof(_FrequencyTimeout));
		duration.Connect("timeout", this, nameof(_DurationTimeout));
	}

	public void _FrequencyTimeout()
	{
		NewShake();
	}

	public void _DurationTimeout()
	{
		ResetShake();
		frequency.Stop();
	}

	public void Start(float duration = 0.2f, float frequency = 15f, float amplitude = 16f, int priority = 0)
	{
		if (priority >= this.priority)
		{
			this.amplitude = amplitude;
			this.priority = priority;

			this.frequency.WaitTime = 1 / frequency;
			this.duration.WaitTime = duration;
			this.duration.Start();
			this.frequency.Start();
			NewShake();
		}
	}

	public void NewShake()
	{
		GD.Randomize();
		var rand = Vector2.Zero;
		rand.x = (float)GD.RandRange(-amplitude, amplitude);
		rand.y = (float)GD.RandRange(-amplitude, amplitude);
		if (IsInstanceValid(cam) && cam is Camera2D)
		{
			shakeTween.InterpolateProperty(cam, "offset", cam.Offset, rand, frequency.WaitTime, transType, easeType);
			shakeTween.Start();
		}
		else
		{
			GD.Print("Invalid camera target");
		}
	}

	public void ResetShake()
	{
		shakeTween.InterpolateProperty(cam, "offset", cam.Offset, Vector2.Zero, frequency.WaitTime, transType, easeType);
		shakeTween.Start();
		this.priority = 0;
	}
}
