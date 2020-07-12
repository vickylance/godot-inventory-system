using Godot;
using System;

public class InventoryItem : Resource
{
	[Export]
	public GenericItem itemReference;
	[Export]
	public int quantity;

	public InventoryItem(GenericItem item, int quantity)
	{
		this.itemReference = item;
		this.quantity = quantity;
	}
}
