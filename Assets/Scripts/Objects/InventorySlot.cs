using UnityEngine;
using System.Collections;

public class InventorySlot {
	public Item item;
	public int quantity=0;
	public int quality=0;

	public InventorySlot(){
		item=new Item();
	}

	public void FillItem(Item item, int quality=100){
		this.item=item;
		this.quality=quality;
		this.quantity=0;
	}

	public void AddQuantity(int quantity){
		this.quantity+=quantity;
		if (quantity<=0){
			EmptyItem();
		}
	}
	public void EmptyItem(){
		item=new Item();
		quality=0;
		quantity=0;
	}
}
