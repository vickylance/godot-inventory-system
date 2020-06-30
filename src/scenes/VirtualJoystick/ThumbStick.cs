using Godot;
using System;

public class ThumbStick : TouchScreenButton
{
    [Export]
    public float threshold = 10f;
    public Vector2 radius = Vector2.Zero;
    public float joyPadSize = 0f;
    public int onGoingDrag = -1;
    public float returnAccel = 20f;

    public override void _Ready()
    {
        var shape = (CircleShape2D)Shape;
        radius = new Vector2(shape.Radius, shape.Radius);
        joyPadSize = (GetParent<Sprite>().Texture.GetSize().x / 2) * GetParent<Sprite>().GlobalScale.x;
    }

    public override void _Process(float delta)
    {
        if (onGoingDrag == -1)
        {
            var posDifference = (Vector2.Zero - radius) - Position;
            Position += posDifference * returnAccel * delta;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventScreenDrag dragEvent)
        {
            var eventDistanceFromCenter = (dragEvent.Position - GetParent<Sprite>().GlobalPosition).Length();
            if (eventDistanceFromCenter <= joyPadSize || dragEvent.Index == onGoingDrag)
            {
                GlobalPosition = dragEvent.Position - (radius * GlobalScale);
                if (GetPos().Length() > joyPadSize)
                {
                    Position = GetPos().Normalized() * joyPadSize - radius;
                }
                onGoingDrag = dragEvent.Index;
            }
        }
        else if ((@event is InputEventScreenTouch touchEvent && @event.IsPressed()))
        {
            var eventDistanceFromCenter = (touchEvent.Position - GetParent<Sprite>().GlobalPosition).Length();
            if (eventDistanceFromCenter <= joyPadSize || touchEvent.Index == onGoingDrag)
            {
                GlobalPosition = touchEvent.Position - (radius * GlobalScale);
                if (GetPos().Length() > joyPadSize)
                {
                    Position = GetPos().Normalized() * joyPadSize - radius;
                }
                onGoingDrag = touchEvent.Index;
            }
        }
        else if (@event is InputEventScreenTouch evt && !@event.IsPressed() && evt.Index == onGoingDrag)
        {
            onGoingDrag = -1;
        }
    }

    public Vector2 GetPos()
    {
        return Position + radius;
    }

    public Vector2 GetValue()
    {
        if (GetPos().Length() > threshold)
        {
            return GetPos() / joyPadSize;
        }
        else
        {
            return Vector2.Zero;
        }
    }
}
