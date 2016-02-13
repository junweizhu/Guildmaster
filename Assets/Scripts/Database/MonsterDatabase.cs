using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterDatabase
{
	private Dictionary<string,Dictionary<Monster,int>> monsterList = new Dictionary<string,Dictionary<Monster,int>> ();
	private List<Monster> allMonsters=new List<Monster>();
	// Use this for initialization
	public MonsterDatabase ()
	{
		NewMonster (new Dictionary<string,int> (){{"Green Forest",100}}, "Slime", "Slime",1, 70, 60, 25, 70, 25, 25);
		NewMonster (new Dictionary<string,int> (){{"Green Forest",40}}, "Green Slime", "Slime",1, 80, 120, 35, 70, 30, 25);
	}

	private void NewMonster (Dictionary<string,int> habitats, string name, string type, int size,int healthGrowth, int manaGrowth, int strengthGrowth, int intelligenceGrowth, int DexterityGrowth, int AgilityGrowth, string element="")
	{
		Dictionary<string,int> statGrowth = new Dictionary<string,int> ();
		statGrowth ["Health"] = healthGrowth;
		statGrowth ["Mana"] = manaGrowth;
		statGrowth ["Strength"] = strengthGrowth;
		statGrowth ["Dexterity"] = DexterityGrowth;
		statGrowth ["Agility"] = AgilityGrowth;
		statGrowth ["Intelligence"] = intelligenceGrowth;
		Monster monster=new Monster (monsterList.Count, name, type, size, statGrowth, element);
		allMonsters.Add(monster);
		foreach (KeyValuePair<string,int> habitat in habitats) {
			if (!monsterList.ContainsKey (habitat.Key)) {
				monsterList [habitat.Key] = new Dictionary<Monster,int> ();
			}
			monsterList [habitat.Key][monster]=habitat.Value;
		}

	}
	public Dictionary<Monster,int> GetAreaMonsters(string areaname){
		return monsterList[areaname];
	}
}
