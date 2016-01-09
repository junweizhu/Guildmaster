using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Item {
	public int id;
	public string name;
	public string type;
	public string element;
	public int sellValue;
	public string description;
	public Dictionary<string,int> stats;
	
	public Item(int id,string name, string type, int money, string description,string element="", Dictionary<string,int> stat=null)
	{
		this.id=id;
		this.name=name;
		this.type=type;
		sellValue=money;
		this.description=description;
		stats=stat;
	}

	public Item()
	{}
}
