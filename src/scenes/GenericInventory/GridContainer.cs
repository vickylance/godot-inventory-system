using Godot;
using System;

public class GridContainer : Godot.GridContainer
{
	public override void _Ready()
	{
		GetNode<GameManager>("/root/GameManager").Connect("PlayerInitialized", this, nameof(_OnPlayerInitialized));
	}

	public void _OnPlayerInitialized(Player player)
	{
		player.inventory.Connect("InventoryChanged", this, nameof(_OnInventoryChanged));
	}

	public void _OnInventoryChanged(GenericInventory inventory)
	{
		foreach (Node n in GetChildren())
		{
			n.QueueFree();
		}

		foreach (InventoryItem item in inventory.Items)
		{
			var itemLabel = new Label();
			itemLabel.Text = $"{item.itemReference.name} x{item.quantity}";
			AddChild(itemLabel);
		}
	}
}
