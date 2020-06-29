using Godot;
using Godot.Collections;
using System.Linq;

public class EquipmentSlots : Panel
{
    public Array<Panel> slots;
    Dictionary<string, TextureRect> items = new Dictionary<string, TextureRect>();
    
    public override void _Ready()
    {
        slots = new Array<Panel>(GetChildren().OfType<Panel>());           // Create an enumerable object containing Panels
        // slots = GetChildren().OfType<Panel>().ToList();           // Create an enumerable object containing Panels
        
        for (int i = 0; i < slots.Count; i++)
        {
            items[slots[i].Name] = null;
        }
    }

    public bool InsertItem(TextureRect item)
    {
        var itemPos = item.RectGlobalPosition + item.RectSize / 2;
        var slot = GetSlotUnderPos(itemPos);
        if (slot == null)
        {
            return false;
        }

        var itemSlot = ItemDB.GetItem(item.GetMeta("id").ToString()).ItemSlot;
        if (itemSlot != slot.Name)
        {
            return false;
        }
        if (items[itemSlot] != null)
        {
            return false;
        }
        items[itemSlot] = item;
        item.RectGlobalPosition = slot.RectGlobalPosition + slot.RectSize / 2 - item.RectSize / 2;
        return true;
    }

    public TextureRect GrabItem(Vector2 pos)
    {
        var item = GetItemUnderPos(pos);
        if (item == null)
        {
            return null;
        }
        var itemSlot = ItemDB.GetItem(item.GetMeta("id").ToString()).ItemSlot;
        items[itemSlot] = null;
        return item;
    }

    public Panel GetSlotUnderPos(Vector2 pos)
    {
        foreach (var slot in slots)
        {
            if (slot != null && slot.GetGlobalRect().HasPoint(pos))
            {
                return slot;
            }
        }
        return null;
    }

    public TextureRect GetItemUnderPos(Vector2 pos)
    {
        foreach (var item in items.Values)
        {
            if (item != null && item.GetGlobalRect().HasPoint(pos))
            {
                return item;
            }
        }
        return null;
    }
}
