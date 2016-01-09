using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Inventory
{
	public List<InventorySlot> inventory=new List<InventorySlot>();
	private Guild guild;
	private ItemDatabase database;

	public void RemoveItem (int id, int quality=100, int amount=1)
	{
		Item item = database.FindItem (id);
		if (item==null)
		{
			return;
		}
		for (int i=0; i<inventory.Count; i++) {
			if (inventory [id].item.id == item.id && inventory [i].quality == quality) {
				if (amount < inventory [i].quantity) {
					inventory[i].AddQuantity(-amount);
				}else{
					inventory[id]=new InventorySlot();
					inventory.SortInventory();
				}
			}
		}
	}

	public void AddItem (int id, int quality=100, int amount=1)
	{
		Item item = database.FindItem (id);
		if (item != null) {
			for (int i=0; i< inventory.Count; i++) {
				if (inventory [i].item.name == item.name && inventory [i].quality == quality) {
					inventory [i].AddQuantity(amount);
					break;
				}
				if (inventory [i].item.name == null) {
					inventory [i].FillItem(item,quality);
					inventory [i].AddQuantity(amount);
					inventory.SortInventory();
					break;
				}
			}
		}

	}


	public Inventory (ItemDatabase db,Guild guild)
	{
		for (int i=0; i<100; i++) {
			inventory.Add (new InventorySlot());
		}
		database = db;
	}

	public bool Contains (int id, int amount=1, int quality=100)
	{
		for (int i=0; i< inventory.Count; i++) {
			if (inventory [i].item.id == id && inventory [i].quality == quality && inventory [i].quantity >= amount)
				return true;
		}
		return false;
	}
}
