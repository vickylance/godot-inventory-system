using Godot;
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
	[Export]
	public string name = "";
	[Export]
	public bool stackable = false;
	[Export]
	public int maxStackSize = 1;

	// Examples of more properties that can be there
	[Export]
	public ItemType itemType;
	[Export]
	public Texture texture;
	[Export]
	public Mesh mesh;
}
