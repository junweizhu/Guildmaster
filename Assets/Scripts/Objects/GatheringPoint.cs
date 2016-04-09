using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GatheringPoint{
	public int id;
	public string type;
	public List<int> gatherableItems=new List<int>();
	public int maxQuantity;

	public GatheringPoint(int id,string type,List<int>items,int maxgathering)
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
		for (int i=0;i<gatherableItems.Count;i++){
			currentRNG=Random.Range(1,1000);
			if (currentRNG>lastRNG){
				selected=Database.items.FindItem(gatherableItems[i]);
				lastRNG=currentRNG;
			}
		}
		return selected;
	}
}
