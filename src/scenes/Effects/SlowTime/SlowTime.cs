using Godot;
using System;

public class SlowTime : Node
{
	private const int END_VALUE = 1;
	private bool isActive = false;
	private float timeStart;
	private float durationMs;
	private float startValue;

	public override void _Ready()
	{
		Engine.TimeScale = 1;
	}

	public override void _Process(float delta)
	{
		if (isActive)
		{
			var currentTime = OS.GetTicksMsec() - timeStart;
			var tweenedTimeValue = CircleEaseIn(currentTime, startValue, END_VALUE, durationMs);
			if (currentTime >= durationMs)
			{
				isActive = false;
				tweenedTimeValue = END_VALUE;
			}
			Engine.TimeScale = tweenedTimeValue;
		}
	}

	public void Start(float duration = 0.4f, float strength = 0.9f)
	{
		timeStart = OS.GetTicksMsec();
		durationMs = duration * 1000;
		startValue = Mathf.Min(0.1f, 1 - strength);
		Engine.TimeScale = startValue;
		isActive = true;
	}

	public float CircleEaseIn(float currentTime, float startTime, float endTime, float duration)
	{
		currentTime /= duration;
		return -endTime * ((float)Math.Sqrt(1 - currentTime * currentTime) - 1) + startTime;
	}
}
