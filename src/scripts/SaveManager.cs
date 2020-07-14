using Godot;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveManager
{
	public static string inventorySavePath = OS.GetUserDataDir() + "/inventory.bin";

	public static void SaveInventory(GenericInventory inventory)
	{
		using (FileStream fs = System.IO.File.Create(inventorySavePath))
		{
			try
			{
				JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
				string output = JsonConvert.SerializeObject(inventory, settings);
				// GenericInventory inventory2 = JsonConvert.DeserializeObject<GenericInventory>(output, settings);
				GD.Print("Inventory: ", output);
				GD.Print("----------------");
				// GD.Print("Inventory: ", inventory2);
			}
			catch (Exception e)
			{
				GD.Print("Deserialization Failed: ", e.Message);
			}
		}
		// using (var stream = new FileStream(inventorySavePath, FileMode.Create))
		// {
		// 	try
		// 	{
		// 		GD.Print(inventorySavePath);
		// 		BinaryFormatter formatter = new BinaryFormatter();
		// 		formatter.Serialize(stream, inventory);
		// 		stream.Close();
		// 	}
		// 	catch (Exception e)
		// 	{
		// 		GD.Print("Inventory save failed: " + e.Message);
		// 	}
		// }
	}

	public static GenericInventory LoadInventory()
	{
		if (System.IO.File.Exists(inventorySavePath))
		{
			using (var stream = new FileStream(inventorySavePath, FileMode.Open))
			{
				try
				{

					BinaryFormatter formatter = new BinaryFormatter();
					GenericInventory inventory = (GenericInventory)formatter.Deserialize(stream);
					stream.Close();
					return inventory;
				}
				catch (Exception e)
				{
					GD.Print("Loading inventory failed: ", e.Message);
					return null;
				}
			}
		}
		else
		{
			return null;
		}
	}
}
