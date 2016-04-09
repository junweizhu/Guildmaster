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
		NewMonster (new Dictionary<string,int> (){{"Green Plains",100}}, "Slime", "Slime",1,Stats(0,0,0,0,0,0), Stats(70, 60, 25, 70, 25, 25));
		NewMonster (new Dictionary<string,int> (){{"Green Plains",40}}, "Green Slime", "Slime",1,Stats(0,0,0,0,0,0), Stats(80, 70, 35, 55, 30, 25));
	}

	private void NewMonster (Dictionary<string,int> habitats, string name, string type, int size,Dictionary<string,int> baseStats,Dictionary<string,int> statGrowth, string element="")
	{

		Monster monster=new Monster (monsterList.Count, name, type, size,baseStats, statGrowth, element);
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

	public Dictionary<string,int> Stats (int health, int mana, int strength, int intelligence, int dexterity, int agility){
		Dictionary<string,int> stat = new Dictionary<string,int> ();
		stat ["Health"] = health;
		stat ["Mana"] = mana;
		stat ["Strength"] = strength;
		stat ["Dexterity"] = dexterity;
		stat ["Agility"] = agility;
		stat ["Intelligence"] = intelligence;
		return stat;
	}
}
