using Godot;
using Godot.Collections;
using System;

public sealed class ItemDatabase : Node
{
	public static ItemDatabase Instance { get { return instance; } }
	private static ItemDatabase instance;

	private Array<GenericItem> items = new Array<GenericItem>();

	ItemDatabase()
	{
		instance = this;
	}


	public override void _Ready()
	{
		Directory directory = new Directory();

		directory.Open("res://src/scenes/GenericInventory/Items");
		directory.ListDirBegin(true, true);

		string filename = directory.GetNext();
		while (filename != "")
		{
			if (!directory.CurrentIsDir())
			{
				var itemResourcePath = $"res://src/scenes/GenericInventory/Items/{filename}";
				var item = GD.Load(itemResourcePath) as GenericItem;
				item.itemResourcePath = itemResourcePath;
				items.Add(item);
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
