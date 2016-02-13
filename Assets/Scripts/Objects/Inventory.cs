using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Inventory
{
	public List<InventorySlot> inventory = new List<InventorySlot> ();

	public void RemoveItem (int id, int amount=1)
	{
		Item item = Database.items.FindItem (id);
		if (item == null) {
			return;
		}
		for (int i=0; i<inventory.Count; i++) {
			if (!inventory [i].filled){
				break;
			}
			if (inventory [i].itemId == item.id && inventory [i].durability == item.durability&&inventory [i].quantity>=amount) {
				inventory [i].AddQuantity (-amount);
				if (!inventory [i].filled){
					SortInventory();
				}
			}
		}
	}

	public void RemoveItemFromSlot (int inventoryid, int amount=1)
	{
		for (int i=0; i<inventory.Count; i++) {
			if (inventory [i].id == inventoryid) {
				if (amount < inventory [i].quantity) {
					inventory [i].AddQuantity (-amount);
				} else {
					inventory [i].EmptyItem ();
					SortInventory ();
				}
			}
		}
	}

	public void RemoveItem (Dictionary<int,int> list)
	{
		foreach (KeyValuePair<int,int> item in list) {
			if (item.Value > 0) {
				RemoveItemFromSlot (item.Key, item.Value);
			}
		}
	}

	public void AddItem (int id,int amount=1, int durability=0)
	{

		Item item = Database.items.FindItem (id);
		if (durability==0||durability>item.durability){
			durability=item.durability;
		}
		if (item != null) {
			for (int i=0; i< inventory.Count; i++) {
				if (inventory [i].itemId == id && inventory [i].durability == durability) {
					inventory [i].AddQuantity (amount);
					break;
				}
				if (!inventory [i].filled) {
					inventory [i].FillItem (id,durability);
					inventory [i].AddQuantity (amount);
					SortInventory ();
					break;
				}
			}
		}

	}

	public List<InventorySlot> GetAllItems ()//Gets all filled inventory slots
	{
		List<InventorySlot> list = new List<InventorySlot> ();
		for (int i=0; i<inventory.Count; i++) {
			if (inventory [i].filled) {
				list.Add (inventory [i]);
			}
		}
		return list;
	}
	public List<InventorySlot> GetAllItems(string type)//Gets all filled inventory slots with a particular type of item
	{
		List<InventorySlot> list = new List<InventorySlot> ();
		for (int i=0; i<inventory.Count; i++) {
			if (inventory [i].filled) {
				if(Database.items.FindItem(inventory [i].itemId).type==type){
					list.Add (inventory [i]);
				}
			}
		}
		return list;

	}
	public Dictionary<InventorySlot,int> GetAllItems (Dictionary<int,int> items)//Gets all the inventory slots from each item  indicated in the dictionary
	{
		Dictionary<InventorySlot,int> itemlist = new Dictionary<InventorySlot, int> ();
		foreach (InventorySlot item in inventory) {
			if (items.ContainsKey (item.id)) {
				if (items [item.id] > 0) {
					itemlist [item] = items [item.id];
				}
			}
		}
		return itemlist;
	}

	public Inventory ()
	{
		for (int i=0; i<100; i++) {
			inventory.Add (new InventorySlot (i));
		}
	}

	public bool Contains (int id, int amount=1)
	{
		for (int i=0; i< inventory.Count; i++) {
			if (inventory [i].itemId == id && inventory [i].durability == Database.items.FindItem (id).durability && inventory [i].quantity >= amount)
				return true;
		}
		return false;
	}

	private void SortInventory(){
		inventory=inventory.OrderBy(slot=>slot.itemId).ToList();
	}
	public InventorySlot GetInventorySlot(int id){
		for (int i=0; i<inventory.Count; i++) {
			if (inventory [i].id==id) {
				return inventory [i];
			}
		}
		return null;
	}
}
