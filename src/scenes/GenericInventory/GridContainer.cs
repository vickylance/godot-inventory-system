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

		for (int i = 0; i < inventory.Items.Count; i++)
		{
			var itemLabel = new Label();
			AddChild(itemLabel);
			itemLabel.Text = $"{inventory.Items[i].itemReference.name} x{inventory.Items[i].quantity}";
		}
	}
}
