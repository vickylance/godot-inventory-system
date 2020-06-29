using Godot;
using Godot.Collections;

public class Item : Godot.Object {
    public string ItemSlot;
    public string ItemPath;
}

public class ItemDB : Node
{
    [Export]
    public string iconPath = "res://assets/sprites/items/";
    public static Dictionary<string, Item> items = new Dictionary<string, Item>();

    public override void _Ready()
    {
        items.Add("sword", new Item() { ItemSlot = "LeftHand", ItemPath = iconPath + "sword.png" });
        items.Add("breastplate", new Item() { ItemSlot = "Chest", ItemPath = iconPath + "breastplate.png" });
        items.Add("potato", new Item() { ItemSlot = "None", ItemPath = iconPath + "potato.png" });
        items.Add("error", new Item() { ItemSlot = "None", ItemPath = iconPath + "error.png" });
    }

    public static Item GetItem(string ItemName)
    {
        if (items.ContainsKey(ItemName))
        {
            return items[ItemName];
        }
        else
        {
            return items["error"];
        }
    }
}
