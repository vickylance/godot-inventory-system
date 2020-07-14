using Godot;
using Newtonsoft.Json;
using System;

public enum ItemType
{
	GENERIC,
	CONSUMABLE,
	QUEST,
	EQUIPMENT,
}

public class GenericItem : Resource
{
	[Export] public string name = "";
	[Export] public bool stackable = false;
	[Export] public int maxStackSize = 1;
	[Export] public ItemType itemType;
	[Export] public Texture texture;

	public string itemResourcePath;

	public Godot.Collections.Dictionary<string, object> Save()
	{
		var result = new Godot.Collections.Dictionary<string, object>()
		{
			{ "name", name },
			{ "stackable", stackable },
			{ "maxStackSize", maxStackSize },
			{ "itemType", itemType },
			{ "itemResourcePath", itemResourcePath },
		};
		return result;
	}
}
