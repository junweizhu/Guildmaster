using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GatheringPoint{
	public int id;
	public string type;
	public List<Item> gatherableItems=new List<Item>();
	public int maxQuantity;

	public GatheringPoint(int id,string type,List<Item>items,int maxgathering)
	{
		this.id=id;
		this.type=type;
		gatherableItems=items;
		maxQuantity=maxgathering;
	}
	public Item FindRandomItem(){
		Item selected=null;
		int lastRNG=0;
		int currentRNG=0;
		foreach (Item item in gatherableItems){
			currentRNG=Random.Range(1,1000);
			if (currentRNG>lastRNG){
				selected=item;
				lastRNG=currentRNG;
			}
		}
		return selected;
	}
}
