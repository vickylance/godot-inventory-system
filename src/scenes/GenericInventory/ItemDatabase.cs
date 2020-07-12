using Godot;
using Godot.Collections;
using System;

public class ItemDatabase : Node
{
	static ItemDatabase instance;
	public static ItemDatabase Instance { get { return instance; } }

	ItemDatabase()
	{
		instance = this;
	}

	private Array<GenericItem> items = new Array<GenericItem>();

	public override void _Ready()
	{
		var directory = new Directory();

		directory.Open("res://src/scenes/GenericInventory/Items");
		directory.ListDirBegin(false, true);

		var filename = directory.GetNext();
		while (filename != "")
		{
			if (!directory.CurrentIsDir())
			{
				items.Add(GD.Load($"res://src/scenes/GenericInventory/Items/{filename}") as GenericItem);
			}
			filename = directory.GetNext();
		}
	}

	public GenericItem GetItem(string itemName)
	{
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].name == itemName)
			{
				return items[i];
			}
		}
		return null;
	}
}
