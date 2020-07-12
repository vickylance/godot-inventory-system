using Godot;

public class CameraControl : Camera2D
{
	[Export]
	public float panSpeed = 20.0f;
	[Export]
	public float zoomMin = 0.2f;
	[Export]
	public float zoomMax = 3f;
	[Export]
	public float zoomStep = 1.1f;

	private bool panStarted = false;
	private Vector2 panStartPos = Vector2.Zero;
	private int panEdgeThreshold = 10;
	private float zoomFactor = 1.0f;
	private Vector2 zoomPos = Vector2.Zero;
	private KinematicBody2D player;

	public override void _Ready()
	{
		Input.SetMouseMode(Input.MouseMode.Confined);
		player = GetTree().Root.FindNode("Player", true, false) as KinematicBody2D;
		Position = player.Position;
	}

	public override void _Process(float delta)
	{
		// Pan Camera using arrow keys
		var inputX = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
		var inputY = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
		var panVector = new Vector2(Position.x + inputX * panSpeed * Zoom.x, Position.y + inputY * panSpeed * Zoom.y);
		panVector = Position.LinearInterpolate(panVector, panSpeed * delta);

		// pan camera using middle mouse button
		if (Input.IsActionJustPressed("pan_camera"))
		{
			panStarted = true;
			panStartPos = GetGlobalMousePosition();
		}
		else if (Input.IsActionJustReleased("pan_camera"))
		{
			panStarted = false;
		}
		if (panStarted)
		{
			panVector = Position + panStartPos - GetGlobalMousePosition();
		}

		// pan camera by moving mouse to edge of the screen
		var localMousePos = GetViewport().GetMousePosition();
		var viewportSize = GetViewport().Size;

		if (localMousePos.x < panEdgeThreshold)
		{
			panVector = new Vector2(
				Mathf.Lerp(Position.x, Position.x - panSpeed * Zoom.x, panSpeed * delta),
				Position.y
			);
		}
		else if (localMousePos.x > viewportSize.x - panEdgeThreshold)
		{
			panVector = new Vector2(
				Mathf.Lerp(Position.x, Position.x + panSpeed * Zoom.x, panSpeed * delta),
				Position.y
			);
		}
		if (localMousePos.y < -panEdgeThreshold)
		{
			panVector = new Vector2(
				Position.x,
				Mathf.Lerp(Position.y, Position.y - panSpeed * Zoom.y, panSpeed * delta)
			);
		}
		else if (localMousePos.y > viewportSize.y - panEdgeThreshold)
		{
			panVector = new Vector2(
				Position.x,
				Mathf.Lerp(Position.y, Position.y + panSpeed * Zoom.y, panSpeed * delta)
			);
		}

		// update the position to the pan
		panVector = ClampedVector(panVector, player.Position + new Vector2(-200, -200), player.Position + new Vector2(200, 200));
		Position = panVector;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.IsPressed())
			{
				var mousePosition = mouseEvent.Position;
				if (mouseEvent.ButtonIndex == (int)ButtonList.WheelUp)
				{
					ZoomAtPosition(zoomStep, mousePosition);
				}
				if (mouseEvent.ButtonIndex == (int)ButtonList.WheelDown)
				{
					ZoomAtPosition(1 / zoomStep, mousePosition);
				}
			}
		}
	}

	public void ZoomAtPosition(float zoomChange, Vector2 zoomPos)
	{
		var c0 = GlobalPosition; // camera position
		var v0 = GetViewport().Size; // vieport size
		var c1 = Vector2.Zero; // next camera position
		var z0 = Zoom; // current zoom value
		var z1 = z0 * zoomChange; // next zoom value
		c1 = c0 + (-0.5f * v0 + zoomPos) * (z0 - z1);
		Zoom = new Vector2(Mathf.Clamp(z1.x, zoomMin, zoomMax), Mathf.Clamp(z1.y, zoomMin, zoomMax));
		if (z1.x > zoomMin && z1.x < zoomMax)
		{
			GlobalPosition = c1;
		}
	}

	public Vector2 ClampedVector(Vector2 vec, Vector2 minClampVec, Vector2 maxClampVec)
	{
		return new Vector2(
			Mathf.Clamp(vec.x, minClampVec.x, maxClampVec.x),
			Mathf.Clamp(vec.y, minClampVec.y, maxClampVec.y)
		);
	}
}
