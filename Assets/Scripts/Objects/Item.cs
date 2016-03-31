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
	public float weight;
	public Dictionary<string,int> stats;
	public Dictionary<string,float> baseStat;
	
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

	public Item(int id, Item baseEquip, Item baseMaterial){
		this.id=id;
		if (baseMaterial.name=="Wood"){
			name=baseMaterial.name+"en "+baseEquip.name;
		} else{
			name=baseMaterial.name+" "+baseEquip.name;
		}
		type=baseEquip.type;
		subType=baseEquip.subType;
		sellValue=ExtensionMethods.Calculate(baseEquip.baseStat["Money"],baseMaterial.baseStat["Money"]);
		weight=baseEquip.baseStat["Weight"]*baseMaterial.baseStat["Weight"];
		this.durability=Mathf.RoundToInt(baseEquip.baseStat["Durability"]*baseMaterial.baseStat["Durability"]);
		stats=new Dictionary<string,int>();
		foreach(KeyValuePair<string,float> stat in baseEquip.baseStat){
			if (baseMaterial.baseStat.ContainsKey(stat.Key)){
				if ((stat.Key=="Accuracy"||stat.Key=="Evade")&&baseEquip.type!="Weapon"){
					if (stat.Value<0){
						stats.Add(stat.Key,ExtensionMethods.Calculate(stat.Value,1f/Mathf.Pow(baseMaterial.baseStat[stat.Key],2)));
					} else{
						stats.Add(stat.Key,ExtensionMethods.Calculate(stat.Value,Mathf.Pow(baseMaterial.baseStat[stat.Key],2)));
					}
				} else{
					stats.Add(stat.Key,Mathf.RoundToInt(stat.Value*baseMaterial.baseStat[stat.Key]));
				}
			} else{
				stats.Add(stat.Key,Mathf.RoundToInt(stat.Value));
			}
		}
	}

	public Item (int id,string name, string type, string description, string subType="", Dictionary<string,float> baseStat=null){//for material and base equipment
		this.id = id;
		this.name = name;
		this.type = type;
		this.subType = subType;
		this.description = description;
		this.baseStat = baseStat;
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
