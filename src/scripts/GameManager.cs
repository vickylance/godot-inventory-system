using Godot;
using System;

public class GameManager : Node
{
	[Signal]
	public delegate void PlayerInitialized();

	public Player player;

	public override void _Process(float delta)
	{
		if (player == null)
		{
			InitializePlayer();
		}
	}

	public void InitializePlayer()
	{
		player = GetTree().Root.FindNode("Player", true, false) as Player;
		if (player == null)
		{
			return;
		}

		EmitSignal(nameof(PlayerInitialized), player);

		player.inventory.Connect("InventoryChanged", this, nameof(_OnInventoryChanged));

		GenericInventory existingInventory = ResourceLoader.Load("user://inventory.tres") as GenericInventory;
		// GenericInventory existingInventory = SaveManager.LoadInventory();
		if (existingInventory != null)
		{
			player.inventory.Items = existingInventory.Items;
			GD.Print("Yolo");
			player.inventory.AddItem("Oak Log", 1);
		}
		else
		{
			GD.Print("Yolo2");
			player.inventory.AddItem("Oak Log", 3);
		}
	}

	public void _OnInventoryChanged(GenericInventory inventory)
	{
		// GD.Print("Save inven: ", inventory.Items[0].itemReference.name);
		// SaveManager.SaveInventory(inventory);
		GD.Print("inventory: ", inventory.Items);
		Error err = ResourceSaver.Save("user://inventory.tres", inventory);
		if (err != Error.Ok)
		{
			GD.Print("Save error");
		}
	}
}
