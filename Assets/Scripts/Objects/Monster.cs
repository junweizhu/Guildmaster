using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster
{

	public int id;
	public string name;
	public int level;
	public int exp = 0;
	public int size;
	public string status;
	public string type;
	public string element;
	public Dictionary<string,int> baseStats = new Dictionary<string, int> ();
	public Dictionary<string,int> statGrowth = new Dictionary<string,int> ();
	public Dictionary<string,int> totalStats = new Dictionary<string, int> ();
	public List<InventorySlot> equipment = new List<InventorySlot> ();
	public List<int> abilities;

	public Monster ()
	{
	}

	public Monster (int id, string name, string type, int size,Dictionary<string,int> basestats, Dictionary<string,int> statgrowth, string element="")
	{
		this.id = id;
		this.name = name;
		this.size = size;
		this.type = type;
		baseStats = basestats;
		statGrowth = statgrowth;
		this.element = element;
	}

	public Monster (int id, string name,int basemonster, int weapon, int armor, int accessory1, int accessory2, int accessory3){

	}
}
