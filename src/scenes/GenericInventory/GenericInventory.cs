using Godot;
using Godot.Collections;
using System;

public class GenericInventory : Resource
{
	[Signal]
	public delegate void InventoryChanged(GenericInventory inventory);

	private Array<InventoryItem> _items = new Array<InventoryItem>();
	[Export]
	public Array<InventoryItem> Items
	{
		get { return _items; }
		set
		{
			_items = value;
			EmitSignal(nameof(InventoryChanged), this);
		}
	}

	public InventoryItem GetItem(int index)
	{
		return _items[index];
	}

	public void AddItem(string itemName, int quantity)
	{
		if (quantity <= 0)
		{
			GD.Print("Cant add a negative number of item");
			return;
		}

		var item = ItemDatabase.Instance.GetItem(itemName);
		if (item == null)
		{
			GD.Print("Could not find the item");
			return;
		}

		var remainingQuantity = quantity;
		var maxStackSize = item.stackable ? item.maxStackSize : 1;

		if (item.stackable)
		{
			for (int i = 0; i < _items.Count; i++)
			{
				// check if we have added the requested number of items to add
				if (remainingQuantity <= 0)
				{
					break;
				}

				// check if the current iteration is the item that we want to add
				var inventoryItem = _items[i];
				if (inventoryItem.itemReference.name != item.name)
				{
					continue;
				}

				// check if there is some quantity left in the item stack to add on top of it
				if (inventoryItem.quantity < maxStackSize)
				{
					var originalQuantity = inventoryItem.quantity;
					inventoryItem.quantity = Math.Min(originalQuantity + remainingQuantity, maxStackSize);
					remainingQuantity -= inventoryItem.quantity - originalQuantity;
				}
			}
		}

		while (remainingQuantity > 0)
		{
			var newItem = new InventoryItem(item, Math.Min(remainingQuantity, maxStackSize));
			_items.Add(newItem);
			remainingQuantity -= newItem.quantity;
		}

		EmitSignal(nameof(InventoryChanged), this);
	}
}