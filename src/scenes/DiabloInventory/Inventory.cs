using Godot;
using System;

public class Inventory : Control
{
	[Export]
	public int ID;
	[Export]
	public PackedScene itemBase;

	public Panel inventoryBase;
	public Panel equipmentSlots;
	public GridBackPack gridBackpack;
	public TextureRect itemHeld;
	public Vector2 itemOffset = Vector2.Zero;
	public Control lastContainer;
	public Vector2 lastPos = Vector2.Zero;

	public override void _Ready()
	{
		inventoryBase = GetNode<Panel>("InventoryBase");
		equipmentSlots = (Panel)FindNode("EquipmentSlots");
		gridBackpack = (GridBackPack)FindNode("GridBackPack");

		PickUpItem("sword");
		PickUpItem("potato");
		PickUpItem("potato");
		PickUpItem("sword");
		PickUpItem("breastplate");
		PickUpItem("breastplate");
		PickUpItem("asdfgadf");
	}

	public override void _GuiInput(InputEvent @event)
	{
		GD.Print("Window: ", ID);
		var cursorPos = GetGlobalMousePosition();
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.IsPressed() && mouseEvent.ButtonIndex == (int)ButtonList.Left)
			{
				// start dragging
				TryGrabbing(cursorPos);
				Raise();
			}
			else
			{
				// end dragging
				Release(cursorPos);
			}
			AcceptEvent();
		}
	}

	public override void _Process(float delta)
	{
		var cursorPos = GetGlobalMousePosition();
		if (itemHeld != null)
		{
			itemHeld.RectGlobalPosition = cursorPos + itemOffset;
		}
	}

	public void TryGrabbing(Vector2 cursorPos)
	{
		var c = GetContainerUnderCursor(cursorPos);
		if (c != null && c.HasMethod("GrabItem"))
		{
			itemHeld = (TextureRect)c.Call("GrabItem", cursorPos);
			if (itemHeld != null)
			{
				lastContainer = (Control)c;
				lastPos = itemHeld.RectGlobalPosition;
				itemOffset = itemHeld.RectGlobalPosition - cursorPos;
				MoveChild(itemHeld, GetChildCount());
			}
		}
	}

	public Control GetContainerUnderCursor(Vector2 cursorPos)
	{
		Control[] containers = { gridBackpack, equipmentSlots, inventoryBase };
		for (int i = 0; i < containers.Length; i++)
		{
			if (containers[i].GetGlobalRect().HasPoint(cursorPos))
			{
				return containers[i];
			}
		}
		return null;
	}

	public void Release(Vector2 cursorPos)
	{
		if (itemHeld == null)
		{
			return;
		}
		var c = GetContainerUnderCursor(cursorPos);
		if (c == null)
		{
			DropItem();
		}
		else if (c.HasMethod("InsertItem"))
		{
			if ((bool)c.Call("InsertItem", itemHeld))
			{
				itemHeld = null;
			}
			else
			{
				ReturnItem();
			}
		}
		else
		{
			ReturnItem();
		}
	}

	public void DropItem()
	{
		itemHeld.QueueFree();
		itemHeld = null;
	}

	public void ReturnItem()
	{
		itemHeld.RectGlobalPosition = lastPos;
		if (lastContainer.HasMethod("InsertItem"))
		{
			lastContainer.Call("InsertItem", itemHeld);
		}
		itemHeld = null;
	}

	public bool PickUpItem(string itemId)
	{
		var item = (TextureRect)itemBase.Instance();
		item.SetMeta("id", itemId);
		item.Texture = (Texture)GD.Load(ItemDB.GetItem(itemId).ItemPath);
		AddChild(item);
		if (!gridBackpack.InsertItemAtFirstAvailableSpot(item))
		{
			item.QueueFree();
			return false; // no space available in the inventory
		}
		return true; // item picked up successfully
	}
}
