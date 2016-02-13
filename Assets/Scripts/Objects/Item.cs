using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Item
{
	public int id;
	public string name;
	public string type;
	public string subType;
	public string element;
	public int sellValue;
	public string description;
	public int durability;
	public Dictionary<string,int> stats;
	
	public Item (int id, string name, string type, int money, string description, int durability=1, string subType="", string element="", Dictionary<string,int> stat=null)
	{
		this.id = id;
		this.name = name;
		this.type = type;
		this.subType = subType;
		this.durability = durability;
		sellValue = money;
		this.description = description;
		stats = stat;
	}

	public Item ()
	{
	}

	public string GetStatString ()
	{
		List<string> statname = new List<string> (stats.Keys);
		if (stats.Count > 1) {
			string statstring="";
			for (int i=0; i<statname.Count; i++) {
				statstring+=stats[statname[i]].ToString()+" "+ Database.strings.GetString(statname[i]);
				if (i+2==statname.Count){
					statstring+=" "+Database.strings.GetString("And")+" ";
				} else if (i+1!=statname.Count){
					statstring+=", ";
				}
			}
			return statstring;
		}
		return stats[statname[0]].ToString()+" "+ Database.strings.GetString(statname[0]);
	}
}
