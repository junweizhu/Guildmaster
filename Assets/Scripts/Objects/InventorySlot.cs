using UnityEngine;
using System.Collections;

[System.Serializable]
public class InventorySlot
{

	public int id;
	public int itemId=9999;
	public bool filled = false;
	public int durability = 0;
	public int quantity = 0;

	public InventorySlot (int id)
	{
		this.id = id;
	}

	public InventorySlot ()
	{
	}
	public InventorySlot (InventorySlot slot)
	{
		id = slot.id;
		itemId = slot.itemId;
		filled = slot.filled;
		durability = slot.durability;
		quantity = slot.quantity;
	}

	public void FillItem (int id, int durability)
	{
		itemId = id;
		filled = true;
		this.durability = durability;
		this.quantity = 0;
	}

	public void AddQuantity (int quantity)
	{
		this.quantity += quantity;
		if (this.quantity <= 0) {
			EmptyItem ();
		}
	}

	public void Use ()
	{
		if (filled) {
			durability -= 1;
			if (durability == 0 || durability < -1) {
				Debug.Log (Database.items.FindItem (itemId).name + " breaks");
				EmptyItem ();
			}
		}
	}
	public string ItemType(){
		if(filled){
			return Database.items.FindItem(itemId).type;
		}
		return "";
	}
	public void EmptyItem ()
	{
		filled = false;
		durability = 0;
		quantity = 0;
		itemId=9999;
	}
	public void Copy (InventorySlot slot)
	{
		id = slot.id;
		itemId = slot.itemId;
		filled = slot.filled;
		durability = slot.durability;
		quantity = slot.quantity;
	}
}
