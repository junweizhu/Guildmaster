using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory
{

	public List<Item> inventory = new List<Item> ();
	public List<int> itemQuality = new List<int> ();
	public List<int> itemQuantity = new List<int> ();
	private ItemDatabase database;

	public void RemoveItem (int id, int quality=100, int amount=1)
	{
		Item item = database.FindItem (id);
		if (item==null)
		{
			return;
		}
		for (int i=0; i<inventory.Count; i++) {
			if (inventory [id].itemId == item.itemId && itemQuality [i] == quality) {
				if (amount < itemQuantity [id]) {
					itemQuantity [id] -= amount;
				} else {
					inventory.RemoveAt (id);
					inventory.Add (new Item ());
					itemQuality.RemoveAt (id);
					itemQuality.Add (0);
					itemQuantity.RemoveAt (id);
					itemQuantity.Add (0);
				}
			}
		}
	}

	public void AddItem (int id, int quality=100, int amount=1)
	{
		Item item = database.FindItem (id);
		if (item != null) {
			for (int i=0; i< inventory.Count; i++) {
				if (inventory [i].itemName == item.itemName && itemQuality [i] == quality) {
					itemQuantity [i] += amount;
					break;
				}
				if (inventory [i].itemName == null) {
					inventory [i] = item;
					itemQuality [i] = quality;
					itemQuantity [i] = amount;
					break;
				}
			}
		}

	}

	public Inventory (ItemDatabase db)
	{
		for (int i=0; i<100; i++) {
			inventory.Add (new Item ());
			itemQuality.Add (0);
			itemQuantity.Add (0);
		}
		database = db;
	}

	bool Contains (int id, int quality=100, int amount=1)
	{
		for (int i=0; i< inventory.Count; i++) {
			if (inventory [i].itemId == id && itemQuality [i] == quality && itemQuantity [i] >= amount)
				return true;
		}
		return false;
	}
}
