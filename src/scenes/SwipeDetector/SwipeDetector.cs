using Godot;
using System;

public class SwipeDetector : Node
{
	// signals
	[Signal]
	public delegate void Swiped(Vector2 direction);
	[Signal]
	public delegate void SwipeCanceled(Vector2 startPosition);


	// public variables
	[Export(PropertyHint.Range, "1.0,1.5,0.05")]
	public float maxDiagonalSlope = 1.3f;


	// private variables
	private Timer timer;
	private Vector2 swipeStartPos = Vector2.Zero;

	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		timer.Connect("timeout", this, nameof(_OnTimerTimeout));

	}

	public override void _Input(InputEvent @event)
	{
		if (!(@event is InputEventScreenTouch))
		{
			return;
		}
		if (@event.IsPressed() && @event is InputEventScreenTouch touchEvent)
		{
			_StartDetection(touchEvent.Position);
		}
		else if (!timer.IsStopped() && @event is InputEventScreenTouch touchEventStopped)
		{
			_EndDetection(touchEventStopped.Position);
		}
	}

	public void _OnTimerTimeout()
	{
		EmitSignal(nameof(SwipeCanceled), swipeStartPos);
	}

	public void _StartDetection(Vector2 pos)
	{
		swipeStartPos = pos;
		timer.Start();
	}

	public void _EndDetection(Vector2 pos)
	{
		timer.Stop();
		var direction = (pos - swipeStartPos).Normalized();
		if (Math.Abs(direction.x) + Math.Abs(direction.y) >= maxDiagonalSlope)
		{
			return;
		}
		if (Math.Abs(direction.x) > Math.Abs(direction.y))
		{
			EmitSignal(nameof(Swiped), new Vector2(Mathf.Sign(-direction.x), 0));
		}
		else
		{
			EmitSignal(nameof(Swiped), new Vector2(0, Mathf.Sign(-direction.y)));
		}
	}
}
