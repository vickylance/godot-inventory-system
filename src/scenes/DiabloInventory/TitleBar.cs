using Godot;
using System;

public class TitleBar : TextureRect
{
    private Vector2 dragPosition = Vector2.Zero;
    public override void _Ready()
    {
        HintTooltip = "Inventory (HotKey: I)";
        Connect("gui_input", this, nameof(_OnWindowGuiInput));
    }

    public void _OnWindowGuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if(mouseEvent.IsPressed() && mouseEvent.ButtonIndex == (int)ButtonList.Left)
            {
                // start dragging
                dragPosition = GetGlobalMousePosition() - GetOwner<Control>().RectGlobalPosition;
                GetOwner<Control>().Raise();
            }
            else
            {
                // end dragging
                dragPosition = Vector2.Zero;
            }
        }
        if (@event is InputEventMouseMotion mouseMotion && dragPosition != Vector2.Zero)
        {
            GetOwner<Control>().RectGlobalPosition = GetGlobalMousePosition() - dragPosition;
        }
    }
}
