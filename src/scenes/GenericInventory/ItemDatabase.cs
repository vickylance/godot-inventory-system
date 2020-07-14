using Godot;
using System.Collections;
using System.Collections.Generic;
using GCollection = Godot.Collections;

public sealed class ItemDatabase : Node
{
	public static ItemDatabase Instance { get { return instance; } }
	private static ItemDatabase instance;

	public GCollection.Dictionary<string, GenericItem> items = new GCollection.Dictionary<string, GenericItem>();

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
				// items2["FIlename"] = item;
				items.Add(item.name, item);
			}
			filename = directory.GetNext();
		}
	}


	public GenericItem GetItem(string itemName)
	{
		return items[itemName];
	}

	public GCollection.Array<GenericItem> GetItems()
	{
		var result = new GCollection.Array<GenericItem>();
		foreach (KeyValuePair<string, GenericItem> itemEntry in items)
		{
			result.Add(itemEntry.Value);
		}
		return result;
	}
}
