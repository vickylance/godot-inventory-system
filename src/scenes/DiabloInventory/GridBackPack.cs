using Godot;
using Godot.Collections;


public struct Point
{
	public int x;
	public int y;

	public Point(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public override string ToString()
	{
		return string.Format("{0}, {1}", x, y);
	}
}

public class GridBackPack : TextureRect
{
	public Array<TextureRect> items = new Array<TextureRect>();
	public bool[,] grid;
	public int cellSize = 32;
	public int gridWidth = 0;
	public int gridHeight = 0;

	public override void _Ready()
	{
		var s = GetGridSize(this);
		gridWidth = s.x;
		gridHeight = s.y;
		grid = new bool[gridWidth, gridHeight];

		for (int i = 0; i < gridWidth; i++)
		{
			for (int j = 0; j < gridHeight; j++)
			{
				grid[i, j] = false;
			}
		}
	}

	public bool InsertItem(TextureRect item)
	{
		var itemPos = item.RectGlobalPosition + new Vector2(cellSize / 2, cellSize / 2);
		var gPos = PosToGridCoords(itemPos);
		var itemSize = GetGridSize(item);
		if (IsGridSpaceAvailable(gPos.x, gPos.y, itemSize.x, itemSize.y))
		{
			SetGridSpace(gPos.x, gPos.y, itemSize.x, itemSize.y, true);
			item.RectGlobalPosition = RectGlobalPosition + new Vector2(gPos.x, gPos.y) * cellSize;
			items.Add(item);
			return true;
		}
		else
		{
			return false;
		}
	}

	public TextureRect GrabItem(Vector2 pos)
	{
		var item = GetItemUnderPos(pos);
		if (item == null)
		{
			return null;
		}

		var itemPos = item.RectGlobalPosition + new Vector2(cellSize / 2, cellSize / 2);
		var gPos = PosToGridCoords(itemPos);
		var itemSize = GetGridSize(item);
		SetGridSpace(gPos.x, gPos.y, itemSize.x, itemSize.y, false);
		items.Remove(item);
		return item;
	}

	public TextureRect GetItemUnderPos(Vector2 pos)
	{
		foreach (var item in items)
		{
			if (item != null && item.GetGlobalRect().HasPoint(pos))
			{
				return item;
			}
		}
		return null;
	}

	public void SetGridSpace(int x, int y, int w, int h, bool state)
	{
		for (int i = x; i < x + w; i++)
		{
			for (int j = y; j < y + h; j++)
			{
				grid[i, j] = state;
			}
		}
	}

	public bool IsGridSpaceAvailable(int x, int y, int w, int h)
	{
		if (x < 0 || y < 0)
		{
			return false;
		}
		if ((x + w) > gridWidth || (y + h) > gridHeight)
		{
			return false;
		}
		for (int i = x; i < (x + w); i++)
		{
			for (int j = y; j < (y + h); j++)
			{
				if (grid[i, j])
				{
					return false;
				}
			}
		}
		return true;
	}

	public Point PosToGridCoords(Vector2 pos)
	{
		var localPos = pos - RectGlobalPosition;
		Point results;
		results.x = (int)(localPos.x / cellSize);
		results.y = (int)(localPos.y / cellSize);
		return results;
	}

	public Point GetGridSize(TextureRect item)
	{
		Point results;
		var s = item.RectSize;
		results.x = Mathf.Clamp((int)(s.x / cellSize), 1, 500);
		results.y = Mathf.Clamp((int)(s.y / cellSize), 1, 500);
		return results;
	}

	public bool InsertItemAtFirstAvailableSpot(TextureRect item)
	{
		for (int y = 0; y < gridHeight; y++)
		{
			for (int x = 0; x < gridWidth; x++)
			{
				if (!grid[x, y])
				{
					item.RectGlobalPosition = RectGlobalPosition + new Vector2(x, y) * cellSize;
					if (InsertItem(item))
					{
						return true;
					}
				}
			}
		}
		return false;
	}
}
