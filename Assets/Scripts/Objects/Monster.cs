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
	public Item weapon;
	public Dictionary<string,int> stats = new Dictionary<string, int> ();
	private Dictionary<string,int> statGrowth = new Dictionary<string,int> ();

	public Monster(){
	}

	public Monster (int id, string name, string type, int size,Dictionary<string,int> statgrowth=null,string element="")
	{
		this.id = id;
		this.name = name;
		statGrowth=statgrowth;
		this.element=element;
	}

	public Monster(Monster monster, int level){
			id=monster.id;
			name=monster.name;
			level=1;
			status="Idle";
			statGrowth=monster.statGrowth;
			stats ["MaxHealth"] = 0;
			stats ["MaxMana"] = 0;
			stats ["CurrentHealth"] = 0;
			stats ["CurrentMana"] = 0;
			stats ["Strength"] = 0;
			stats ["Dexterity"] = 0;
			stats ["Agility"] = 0;
			stats ["Intelligence"] = 0;
			stats ["Attack"] = 0;
			stats ["Defense"] = 0;
			stats ["MagicDefense"] = 0;
			stats ["Fame"] = 0;
			DistributeStats (20);
			if (level > 1)
				GiveExp ((level - 1) * 100);
			Heal (100, "percent", "Health");
			Heal (100, "percent", "Mana");
	}
	public void Attack (Member target)
	{
		int hitrate = Mathf.FloorToInt(70 + 20*stats ["Dexterity"] -17*target.stats ["Agility"]);
		int damage = 0;
		int defense = 0;
		if (Random.Range (0, 101) < hitrate) {
			damage = stats ["Attack"];
			if (weapon != null) {
				if (weapon.element != "") {
					damage += stats ["Intelligence"];
					defense = target.stats ["MagicDefense"];
				} else {
					damage += stats ["Strength"];
					defense = target.stats ["Defense"];
				}
			} else {
				damage += stats ["Strength"];
				defense = target.stats ["Defense"];
			}

			target.stats ["CurrentHealth"] -= damage - defense;
			Debug.Log(name +"Attacks "+ target.name+" and deals "+(damage-defense).ToString()+" Damage. ("+target.stats["CurrentHealth"]+")");
		}else{
			Debug.Log(name +"Attacks, but "+ target.name+" dodges ");
		}
		target.GiveExp (target.FindSkill ("Combat Skill"), 1);
	}
	public void DistributeStats (int points)
	{
		Dictionary<string,int> rng = new Dictionary<string, int> (statGrowth);
		int statgains = 0;
		while (true) {
			foreach (KeyValuePair<string,int> growth in statGrowth) {
				rng [growth.Key] += (Random.Range (10, statGrowth [growth.Key]) + Random.Range (10, statGrowth [growth.Key])) / 2;
				while (rng[growth.Key]>=100) {
					stats [growth.Key] += 1;
					statgains += 1;
					rng [growth.Key] -= 120;
					if (statgains >= points) {
						return;
					}
				}
			}
		}
	}

	public void Heal (int amount, string method, string type)
	{
		if (method == "flat") {
			stats ["Current" + type] += amount;
		} else if (method == "percent") {
			stats ["Current" + type] += Mathf.RoundToInt (stats ["Max" + type] * amount / 100);
		}
		if (stats ["Current" + type] > stats ["Max" + type])
			stats ["Current" + type] = stats ["Max" + type];
	}

	public void GiveExp (int exp)
	{
		if (level < 60) {
			this.exp += exp;
			while (this.exp>=100) {
				level += 1;
				DistributeStats (4);
				this.exp -= 100;
				if (level == 60) {
					this.exp = 0;
				}
			}
		}
	}
}
