using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster
{

	public int id;
	public string name;
	public int level;
	public int exp = 0;
	public string status;
	public string type;
	public string element;
	public Dictionary<string,int> baseStats = new Dictionary<string, int> ();
	public Dictionary<string,int> bonusStats = new Dictionary<string, int> ();
	public Dictionary<string,int> equipmentStats = new Dictionary<string, int> ();
	public Dictionary<string,int> statGrowth = new Dictionary<string,int> ();
	public Dictionary<string,int> totalStats = new Dictionary<string, int> ();
	public List<InventorySlot> equipment = new List<InventorySlot> ();
	public List<int> abilities;
	public int blockItem;
	public bool canAttack=false;

	public Monster ()
	{
	}

	public Monster (int id, string name, string type, int size, Dictionary<string,int> statgrowth=null, string element="")
	{
		this.id = id;
		this.name = name;
		statGrowth = statgrowth;
		this.element = element;
	}

	public Monster (int id, string name,int basemonster, int weapon, int armor, int accessory1, int accessory2, int accessory3){

	}
}
