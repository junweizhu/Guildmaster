using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Member
{
	public int id;
	public int guildnr;
	public string name;
	public string gender;
	public int level;
	public int exp = 0;
	public string status;
	public Dictionary<string,int> stats = new Dictionary<string, int> ();
	public List<Skill> skillList = new List<Skill> ();
	public Dictionary<Skill,int> skillLevel = new Dictionary<Skill,int > ();
	public Dictionary<Skill,int> skillExp = new Dictionary<Skill,int > ();
	public List<InventorySlot> inventory = new List<InventorySlot> ();
	public List<int> itemQuality = new List<int> ();
	public int money;
	public bool recruited = false;
	private Dictionary<string,int> statGrowth = new Dictionary<string,int> ();
	public bool levelUp = false;
	public Dictionary<string,int> levelUpStats = new Dictionary<string, int> ();
	public bool skillUp = false;
	public List<string> leveledSkill = new List<string> ();
	public InventorySlot weapon = null;
	public InventorySlot armor = null;
	public InventorySlot Accessory = null;

	public Member (int id, string name, bool male, int level, int money, List<Skill> skills, string type="")
	{
		this.id = id;
		this.name = name;
		if (male == true)
			gender = "Male";
		else
			gender = "Female";
		this.level = 1;
		int healthmanamod = Random.Range (10, 20);
		int strintmod = Random.Range (10, 20);
		if (type == "Mage") {
			healthmanamod *= -1;
			strintmod *= -1;
		} else if (type == "Adventurer") {
			healthmanamod = Mathf.CeilToInt (0.5f * healthmanamod);
			strintmod = Mathf.CeilToInt (0.5f * strintmod);
		} else if (type == "Social") {
			healthmanamod = Mathf.CeilToInt (-0.5f * healthmanamod);
			strintmod = Mathf.CeilToInt (-0.5f * strintmod);
		} else {
			healthmanamod = 0;
			strintmod = 0;
		}
		statGrowth ["MaxHealth"] = (Random.Range (60, 120) + Random.Range (60, 120)) / 2 + healthmanamod;
		statGrowth ["MaxMana"] = (Random.Range (60, 120) + Random.Range (60, 120)) / 2 - healthmanamod;
		statGrowth ["Strength"] = (Random.Range (15, 55) + Random.Range (15, 55)) / 2 + strintmod;
		statGrowth ["Intelligence"] = (Random.Range (15, 55) + Random.Range (15, 55)) / 2 - strintmod;
		statGrowth ["Dexterity"] = (Random.Range (35, 65) + Random.Range (35, 65)) / 2;
		statGrowth ["Agility"] = (Random.Range (35, 65) + Random.Range (35, 65)) / 2;
		foreach (KeyValuePair<string,int> stat in statGrowth)
			levelUpStats [stat.Key] = 0;
		if (statGrowth ["Strength"] > statGrowth ["Intelligence"]) {
			if (statGrowth ["Strength"] < 50) {
				statGrowth ["Strength"] += 10;
			}
			if (statGrowth ["MaxMana"] > statGrowth ["MaxHealth"]) {
				statGrowth ["MaxMana"] -= 20;
			}
		} else {
			if (statGrowth ["Intelligence"] < 50) {
				statGrowth ["Intelligence"] += 10;
				if (statGrowth ["MaxHealth"] > statGrowth ["MaxMana"]) {
					statGrowth ["MaxHealth"] -= 20;
				}
			}
		}
		stats ["MaxHealth"] = 5;
		stats ["MaxMana"] = 5;
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
		this.money = money;
		skillList = skills;
		status = "Idle";
		foreach (Skill skill in skillList) {
			skillLevel.Add (skill, 1);
			skillExp.Add (skill, 0);
		}
		if (level > 1)
			GiveExp (null, (level - 1) * 100);
		Heal (100, "Percent", "Health");
		Heal (100, "Percent", "Mana");
		for (int i=0; i<10; i++) {
			inventory.Add (new InventorySlot ());
		}
	}

	public void Attack (Monster target)
	{
		int hitrate = 70 + 20 * stats ["Dexterity"] - 17 * target.stats ["Agility"];
		int damage = 0;
		int defense = 0;
		if (Random.Range (0, 101) < hitrate) {
			if (weapon != null) {
				damage = weapon.item.stats ["Attack"];
				if (weapon.item.element != "" && weapon.item.element != null) {
					damage += stats ["Intelligence"];
					defense = target.stats ["MagicDefense"];
					GiveExp (FindSkill ("Magic Skill"), 2);
					
				} else {
					damage += stats ["Strength"];
					defense = target.stats ["Defense"];
					GiveExp (FindSkill ("Weapon Skill"), 2);
				}
			} else {
				damage += stats ["Strength"];
				defense = target.stats ["Defense"];
			}

			target.stats ["CurrentHealth"] -= damage - defense;
			Debug.Log (name + "Attacks " + target.name + " and deals " + (damage - defense).ToString () + " Damage. (" + target.stats ["CurrentHealth"] + ")");
			GiveExp (FindSkill ("Combat Skill"), 1);
		} else {
			Debug.Log (name + "Attacks, but " + target.name + " dodges ");
		}
	}

	public void GiveExp (Skill skill, int exp)
	{
		if (skill != null) {
			skillExp [skill] += exp;
			while (skillExp [skill] >= 100) {
				LevelUp (skill);
				skillExp [skill] -= 100;
				skillLevel [skill] += 1;
			}
		} else if (level < 60) {
			this.exp += exp;
			while (this.exp>=100) {
				level += 1;
				LevelUp (null);
				this.exp -= 100;
				if (level == 60) {
					this.exp = 0;
				}
			}
		}
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
					if (levelUp) {
						levelUpStats [growth.Key] += 1;
					}
					rng [growth.Key] -= 120;
					if (statgains >= points) {
						return;
					}
				}
			}
		}
		/*int sum=0;
		int totalpoints=0;
		foreach (KeyValuePair<string,int> growth in statGrowth)
		{
			rng[growth.Key]=Random.Range(statGrowth[growth.Key],100+statGrowth[growth.Key]);
			sum+=rng[growth.Key];
		}
		foreach (KeyValuePair<string,int> growth in statGrowth)
		{
			float number=rng[growth.Key]*points/sum;
			int statgain=0;
			if (number>=Mathf.Floor(number)+0.5f)
				statgain=Mathf.CeilToInt(number);
			else
				statgain=Mathf.FloorToInt(number);
			stats[growth.Key]+=statgain;
			totalpoints+=statgain;
		}
		if (totalpoints<points)
			DistributeStats(points-totalpoints);*/
	}

	public void LevelUp (Skill skill)
	{
		if (skill == null) {
			levelUp = true;
			DistributeStats (4);
		} else { 
			skillUp = true;
			leveledSkill.Add (skill.name);
			if (skill.statgrowth.Count > 0) {
				foreach (KeyValuePair<string,int> stat in skill.statgrowth) {
					stats [stat.Key] += stat.Value;
				}
			}
		}
	}

	public void AddItem (Item item, int quality)
	{
		for (int i=0; i<inventory.Count; i++) {
			if (inventory [i].item.name == null) {
				inventory [i].FillItem (item, quality);
				inventory [i].AddQuantity (1);
				if (inventory [i].item.type == "Weapon" || inventory [i].item.type == "Armor" || inventory [i].item.type == "Accessory") {
					Equip (inventory [i]);
				}
				return;
			}
		}
	}

	public void Heal (int amount, string method, string type)
	{
		if (method == "Flat") {
			stats ["Current" + type] += amount;
		} else if (method == "Percent") {
			stats ["Current" + type] += Mathf.RoundToInt (stats ["Max" + type] * amount / 100);
		}
		if (stats ["Current" + type] > stats ["Max" + type])
			stats ["Current" + type] = stats ["Max" + type];
	}

	public Skill FindSkill (string skillname)
	{
		for (int i=0; i<skillList.Count; i++) {
			if (skillList [i].name == skillname)
				return skillList [i];
		}
		return null;
	}

	public int FindSkillLevel (string skillname)
	{
		return skillLevel [FindSkill (skillname)];
	}

	public void NextDayResets ()
	{
		levelUp = false;
		skillUp = false;
		if (levelUpStats.Count > 0) {
			foreach (KeyValuePair<string,int> stat in statGrowth) {
				levelUpStats [stat.Key] = 0;
			}
		}
		leveledSkill.Clear ();
	}

	public bool HasHealingItems ()
	{
		foreach (InventorySlot slot in inventory) {
			if (slot.item.name != null) {
				if (slot.item.stats.ContainsKey ("Heal"))
					return true;
			}
		}
		return false;
	}

	public void UseHealingItem ()
	{
		foreach (InventorySlot slot in inventory) {
			if (slot.item.name != null) {
				if (slot.item.stats.ContainsKey ("Heal")) {
					Heal (slot.item.stats ["Heal"], "Flat", "Health");
					Debug.Log (name + " used " + slot.item.name + " and healed. (" + stats ["CurrentHealth"] + ")");
					slot.EmptyItem ();
					inventory.SortInventory ();
					return;
				}
			}
		}
	}

	public void Equip (InventorySlot slot)
	{
		if (slot.item.type == "Weapon") {
			if (weapon == null) {
				weapon = slot;
			} else if (weapon.item.stats ["Attack"] < slot.item.stats ["Attack"]) {
				weapon = slot;
			}
		}
	}

	public List<InventorySlot> GetInventory ()
	{
		List<InventorySlot> list = new List<InventorySlot> ();
		for (int i=0; i<inventory.Count; i++) {
			if (inventory [i].item.name != null) {
				list.Add (inventory [i]);
			}
		}
		return list;
	}
}
