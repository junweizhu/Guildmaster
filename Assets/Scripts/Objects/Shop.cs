using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shop{
	public int id;	
	public string name;
	public string description;
	public Dictionary<int,List<Item>> itemList=new Dictionary<int,List<Item>>();

	public Shop(int id,string name,string description)
	{
		this.id=id;
		this.name=name;
		this.description=description;
	}

	public void AddItem(int level,Item item)
	{
		if (!itemList.ContainsKey(level))
			itemList[level]=new List<Item>();
		itemList[level].Add(item);
	}

}
