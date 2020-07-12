using Godot;
using System;

public class GameManager : Node
{
	[Signal]
	public delegate void PlayerInitialized();

	public Player player;

	public override void _Process(float delta)
	{
		if (player != null)
		{
			InitializePlayer();
		}
	}

	public void InitializePlayer()
	{
		player = GetTree().Root.FindNode("Player") as Player;
		if (player == null)
		{
			return;
		}

		EmitSignal(nameof(PlayerInitialized), player);

		player.inventory.Connect("InventoryChanged", this, nameof(_OnInventoryChanged));

		var existingInventory = GD.Load("user://inventory.tres") as GenericInventory;
		if (existingInventory != null)
		{
			player.inventory.Items = existingInventory.Items;
		}
		else
		{
			player.inventory.AddItem("Oak Log", 3);
		}
	}

	public void _OnInventoryChanged(GenericInventory inventory)
	{
		ResourceSaver.Save("user://inventory.tres", inventory);
	}
}
