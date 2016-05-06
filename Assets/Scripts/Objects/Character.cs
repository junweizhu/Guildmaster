using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Character
{
	public int id;
	public int guildnr;
	public string name;
	public string nickname;
	public string gender;
	public int level;
	public int exp = 0;
	public string status = "Idle";
	public string statusAdd = "";
	public Dictionary<string,int> baseStats;
	public Dictionary<string,int> bonusStats = new Dictionary<string, int> ();
	public Dictionary<string,int> equipmentStats = new Dictionary<string, int> ();
	private Dictionary<string,int> statGrowth = new Dictionary<string,int> ();
	public Dictionary<string,int> totalStats = new Dictionary<string, int> ();
	public Dictionary<int,int> skillLevel = new Dictionary<int,int > ();
	public Dictionary<int,int> skillExp = new Dictionary<int,int > ();
	public List<InventorySlot> equipment = new List<InventorySlot> ();
	public List<int> abilities;
	public bool recruited = false;
	public bool recruitable = false;
	public bool levelUp = false;
	public Dictionary<string,int> levelUpStats = new Dictionary<string, int> ();
	public bool skillUp = false;
	public List<string> leveledSkill = new List<string> ();
	public bool didDamage = false;
	public int blockItem = 0;
	public bool canAttack = false;
	public bool isEnemy = false;
	public List<InventorySlot> tempEquipment = new List<InventorySlot> ();
	public int totalStatCount = 0;

	public Character ()
	{

	}

	public Character (int id, string name, bool male, int level, string type = "", Dictionary<string,int> statgrowth=null)
	{
		this.id = id;
		this.name = name;
		if (male == true)
			gender = "Male";
		else
			gender = "Female";
		this.level = 1;
		if (statgrowth == null) {
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

			statGrowth ["Health"] = (Random.Range (60, 120) + Random.Range (60, 120)) / 2 + healthmanamod;
			statGrowth ["Mana"] = (Random.Range (40, 120) + Random.Range (40, 120)) / 2 - healthmanamod;
			statGrowth ["Strength"] = (Random.Range (15, 55) + Random.Range (15, 55)) / 2 + strintmod;
			statGrowth ["Intelligence"] = (Random.Range (15, 55) + Random.Range (15, 55)) / 2 - strintmod;
			statGrowth ["Dexterity"] = (Random.Range (35, 65) + Random.Range (35, 65)) / 2;
			statGrowth ["Agility"] = (Random.Range (35, 65) + Random.Range (35, 65)) / 2;
			if (statGrowth ["Strength"] > statGrowth ["Intelligence"]) {
				if (statGrowth ["Strength"] < 50) {
					statGrowth ["Strength"] += 10;
				}
				if (statGrowth ["Mana"] > statGrowth ["Health"]) {
					statGrowth ["Mana"] -= 20;
				}
			} else {
				if (statGrowth ["Intelligence"] < 50) {
					statGrowth ["Intelligence"] += 10;
					if (statGrowth ["Health"] > statGrowth ["Mana"]) {
						statGrowth ["Health"] -= 20;
					}
				}
			}
		} else {
			statGrowth = statgrowth;
		}
		baseStats = new Dictionary<string, int> ();
		baseStats ["Health"] = 5;
		if (statGrowth ["Mana"] > 0) {
			baseStats ["Mana"] = 5;
		} else {
			baseStats ["Mana"] = 0;
		}
		baseStats ["Strength"] = 1;
		baseStats ["Dexterity"] = 1;
		baseStats ["Agility"] = 1;
		if (statGrowth ["Intelligence"] > 0) {
			baseStats ["Intelligence"] = 1;
		} else {
			baseStats ["Intelligence"] = 0;
		}
		baseStats ["Fame"] = 0;
		SetUpDefaultStats (level);

	}

	public Character (Monster monster, int level = 1, bool isEnemy = true)
	{
		id = monster.id;
		name = monster.name;
		nickname = monster.name;
		this.level = 1;
		statGrowth = monster.statGrowth;
		baseStats = monster.baseStats.ToDictionary(entry=>entry.Key,entry=>(int)entry.Value);
		Debug.Log ("Before "+baseStats ["Health"] + " " + baseStats ["Mana"] + " " + baseStats ["Strength"] + " " + baseStats ["Dexterity"] + " " + baseStats ["Agility"] + " " + baseStats ["Intelligence"]);
		SetUpDefaultStats (level);
		this.isEnemy = isEnemy;
		Debug.Log ("After "+baseStats ["Health"] + " " + baseStats ["Mana"] + " " + baseStats ["Strength"] + " " + baseStats ["Dexterity"] + " " + baseStats ["Agility"] + " " + baseStats ["Intelligence"]);
	}

	public void SetUpDefaultStats (int level, Monster monster = null)
	{
		totalStats ["MaxHealth"] = 0;
		totalStats ["MaxMana"] = 0;
		totalStats ["PAttack"] = 0;
		totalStats ["MAttack"] = 0;
		totalStats ["PDefense"] = 0;
		totalStats ["MDefense"] = 0;
		totalStats ["Accuracy"] = 50;
		totalStats ["Evade"] = 10;
		totalStats ["Block"] = 0;
		totalStats ["BlockChance"] = 0;
		totalStats ["Speed"] = 0;
		totalStats ["Weight"] = 0;
		totalStats ["CarrySize"] = 0;
		foreach (string stat in baseStats.Keys) {
			bonusStats [stat] = 0;
		}
		foreach (string stat in totalStats.Keys) {
			equipmentStats [stat] = totalStats [stat];
		}
		foreach (string stat in statGrowth.Keys)
			levelUpStats [stat] = 0;
		totalStats ["CurrentHealth"] = 0;
		totalStats ["CurrentMana"] = 0;
		DistributeStats (16);
		Heal (100, "percent", "Health");
		Heal (100, "percent", "Mana");
		for (int i = 0; i < 5; i++) {
			equipment.Add (new InventorySlot (i));
			tempEquipment.Add (new InventorySlot (i));
		}
		for (int i = 0; i < Database.skills.SkillList ().Count; i++) {
			skillLevel.Add (i, 1);
			skillExp.Add (i, 0);
		}
		abilities = new List<int> (){ 0, 1, 2, 3 };
		if (level > 1)
			GiveExp (Database.skills.SkillList ().Count, (level - 1) * 100);
		UpdateStats ();
	}

	public Ability ChooseAttack (Character target)
	{
		List<Ability> abilities = GetUsableAbilities ();
		Ability chosenAbility = null;
		if (abilities.Count > 1) {
			for (int i = 0; i < abilities.Count; i++) {
				if (chosenAbility == null) {
					chosenAbility = abilities [i];
				} else { 
					int chosendamage = chosenAbility.CalculateDamage (totalStats ["PAttack"], totalStats ["MAttack"], target, GetWeaponType ());
					int idamage = abilities [i].CalculateDamage (totalStats ["PAttack"], totalStats ["MAttack"], target, GetWeaponType ());
					if (chosendamage < idamage || chosendamage == idamage && Random.Range (0, 10) < 5) {
						chosenAbility = abilities [i];
					} 	
				}
			}
		} else {
			chosenAbility = abilities [0];
		}
		Debug.Log (nickname + " uses " + chosenAbility.name + " " + System.DateTime.Now.Millisecond.ToString ());
		canAttack = true;
		return chosenAbility;
	}

	/*public int CalculateDamage(Ability ability){
		int damage;
		if (ability.element == "Physical") {
			damage = totalStats ["PAttack"];
			if (ability.statBonus != null && ability.statBonus.ContainsKey ("PAttack")) {
				damage += ability.statBonus ["PAttack"];
			}
		} else {
			damage = totalStats ["MAttack"];
			if (ability.statBonus != null && ability.statBonus.ContainsKey ("MAttack")) {
				damage += ability.statBonus ["MAttack"];
			}
		}
		return damage;
	}*/
	public string ChooseBattleAction (List<Character> allies, Adventure adventure)
	{
		if ((float)totalStats ["CurrentHealth"] / totalStats ["MaxHealth"] * 100 < 30 && HasHealingItems ()) {
			UseHealingItem ();
			return "Heal";
		} else if (allies.NeedsHealing (this)) {
			Heal (allies.GetCharacterToHeal (this),adventure);
			return "Heal";
		} else {
			return "Fight";
		}
	}

	public Ability ChooseCounterAttack (int range, Character target)
	{
		List<Ability> abilities = GetUsableAbilities (false, range);
		Ability chosenAbility = null;
		if (abilities.Count == 0) {
			Debug.Log (nickname + " can't attack from this range! " + System.DateTime.Now.Millisecond.ToString ());
			canAttack = false;
		} else {
			if (abilities.Count > 1) {
				for (int i = 0; i < abilities.Count; i++) {
					if (chosenAbility == null) {
						chosenAbility = abilities [i];
					} else { 
						int chosendamage = chosenAbility.CalculateDamage (totalStats ["PAttack"], totalStats ["MAttack"], target, GetWeaponType ());
						int idamage = abilities [i].CalculateDamage (totalStats ["PAttack"], totalStats ["MAttack"], target, GetWeaponType ());
						if (chosendamage < idamage || chosendamage == idamage && Random.Range (0, 10) < 5) {
							chosenAbility = abilities [i];
						} 	
					}
				}
			} else if (abilities.Count == 1) {
				chosenAbility = abilities [0];
			}
			Debug.Log (nickname + " uses " + chosenAbility.name + " as counterattack " + System.DateTime.Now.Millisecond.ToString ());
			canAttack = true;
		}
		return chosenAbility;
	}


	public List<Ability> GetUsableAbilities (bool attacking = true, int range = 1,bool alsoHealing=false)
	{
		List<Ability> usableAbilities = new List<Ability> ();
		string weapontype = GetWeaponType ();
		for (int i = 0; i < abilities.Count; i++) {
			Ability ability = Database.skills.GetAbility (abilities [i]);
			if ((ability.weaponType == null || (ability.weaponType != null && ability.weaponType.Contains (weapontype))) && ability.manaCost <= totalStats ["CurrentMana"] && (ability.element != "Healing" ||alsoHealing)) {
				Debug.Log (ability.manaCost + " " + totalStats ["CurrentMana"]);
				if (attacking || (!attacking && ability.range == range))
					usableAbilities.Add (ability);
			}
		}
		if (usableAbilities.Count < 1 && attacking) {
			usableAbilities.Add (Database.skills.GetAbility (0));
		}
		return usableAbilities;
	}

	public void Attack (Character target, Ability chosenAbility, Adventure adventure)
	{

		bool hit = false;
		if (chosenAbility.element == "Physical") {
			adventure.actionList.Add (nickname + " attacks");
			hit = target.Defend (chosenAbility.CalculateDamage (totalStats ["PAttack"], totalStats ["MAttack"]), totalStats ["Accuracy"], "P", GetWeaponType (), GetWeaponElement (),adventure);
		} else {
			adventure.actionList.Add (nickname + " attacks");
			hit = target.Defend (chosenAbility.CalculateDamage (totalStats ["PAttack"], totalStats ["MAttack"]), totalStats ["Accuracy"], "M", "", chosenAbility.element,adventure);
		}
		if (hit) {
			if (tempEquipment [0].filled) {
				tempEquipment [0].Use ();
				if (tempEquipment [1].durability <= 0) {
					canAttack = false;
				}
			}
			didDamage = true;
		}
	}

	public string GetWeaponType ()
	{
		if (tempEquipment [0].filled) {
			return Database.items.FindItem (tempEquipment [0].itemId).subType;
		}
		return "Fist";
	}

	public string GetWeaponElement ()
	{
		if (tempEquipment [0].filled) {
			return Database.items.FindItem (tempEquipment [0].itemId).element;
		}
		return "";
	}

	public bool Defend (int damage, int hitrate, string damageType, string damageSubType, string element, Adventure adventure)
	{
		int RNG = ExtensionMethods.Calculate (Random.Range (0, 101) + Random.Range (0, 101), 0.5f) + totalStats ["Evade"];

		if (hitrate > RNG) {
			int defense = totalStats [damageType + "Defense"];
			if (hitrate - totalStats ["BlockChance"] < RNG && totalStats ["BlockChance"] > 0) {
				defense += totalStats ["Block"];
				adventure.actionList.Add (nickname + " blocked the attack ");
				if (tempEquipment [blockItem].filled) {
					tempEquipment [blockItem].Use ();
				}
			}
			if (damageSubType == "Mace" || element == "None") {
				defense = Calculate (defense, 0.75f);
			}

			damage -= defense;
			if (damage < 0) {
				damage = 0;
			}
			totalStats ["CurrentHealth"] -= damage;
			if (totalStats ["CurrentHealth"] < 0) {
				totalStats ["CurrentHealth"] = 0;
			}
			adventure.actionList.Add (nickname + " received " + damage.ToString () + " damage.");
			if (tempEquipment [1].filled) {
				tempEquipment [1].Use ();
			}
			for (int i = 2; i < tempEquipment.Count; i++) {
				if (tempEquipment [i].filled && i != blockItem && tempEquipment [i].ItemType () != "Consumable") {
					if (Random.Range (0, 101) < 35) {
						tempEquipment [i].Use ();
					}
				}
			}
			return true;
		} else {
			adventure.actionList.Add (nickname + " dodged the attack");
			return false;
		}
	}

	public void CheckEquipment ()
	{//checking if an equipment breaks during adventuring.
		for (int i = 0; i < tempEquipment.Count; i++) {
			if (tempEquipment [i].filled != equipment [i].filled) {
				equipmentStats.UpdateEquipmentStats (tempEquipment, ref blockItem);
				return;
			}
		}
	}

	public void Equip (int id, int itemId, int durability)
	{
		equipment [id].FillItem (itemId, durability);
		equipmentStats.UpdateEquipmentStats (equipment, ref blockItem);
		UpdateStats ();
	}

	public void UnEquip (int id)
	{
		equipment [id].EmptyItem ();
		equipmentStats.UpdateEquipmentStats (equipment, ref blockItem);
		UpdateStats ();
	}

	public void Use (InventorySlot slot)
	{
		if (slot != null) {
			slot.Use ();
		}
	}

	public void GiveExp (int skillId, int exp)
	{
		if (skillExp.ContainsKey (skillId)) {
			skillExp [skillId] += exp;
			while (skillExp [skillId] >= 100) {
				LevelUp (skillId);
				skillExp [skillId] -= 100;
				skillLevel [skillId] += 1;
			}
		} else if (level < 60) {
			this.exp += exp;
			while (this.exp >= 100) {
				level += 1;
				LevelUp (Database.skills.SkillList ().Count);
				this.exp -= 100;
				if (level == 60) {
					this.exp = 0;
				}
			}
		}
	}

	public void CreateTempEquipment ()
	{
		for (int i = 0; i < equipment.Count; i++) {
			tempEquipment [i].Copy (equipment [i]);
		}

	}

	public void EquipmentAfterAdventure ()
	{
		for (int i = 0; i < equipment.Count; i++) {
			equipment [i].Copy (tempEquipment [i]);
		}
	}

	public void UpdateStats (bool temporary = false)
	{
		totalStats.UpdateStats (baseStats, bonusStats, equipmentStats, status);
	}

	public int Calculate (int totalstat, float multiplier)
	{
		float result = totalstat * multiplier;
		if (result >= Mathf.FloorToInt (result) + 0.5f) {
			return Mathf.CeilToInt (result);
		} else {
			return Mathf.FloorToInt (result);
		}
	}

	public void DistributeStats (int points)
	{
		Dictionary<string,int> rng = new Dictionary<string, int> (statGrowth);
		int statgains = 0;
		while (true) {
			foreach (KeyValuePair<string,int> growth in statGrowth) {
				rng [growth.Key] += (Random.Range (10, statGrowth [growth.Key]) + Random.Range (10, statGrowth [growth.Key])) / 2;
				while (rng [growth.Key] >= 100) {
					baseStats [growth.Key] += 1;
					statgains += 1;
					totalStatCount += 1;
					if (levelUp) {
						if (levelUpStats.ContainsKey (growth.Key)) {
							levelUpStats [growth.Key] += 1;
						} else {
							levelUpStats [growth.Key] = 1;
						}
					}
					rng [growth.Key] -= 120;
					if (statgains >= points) {
						UpdateStats ();
						return;
					}
				}
			}
		}
	}

	public void LevelUp (int skillid)
	{

		if (!skillLevel.ContainsKey (skillid)) {
			Debug.Log (nickname + " levels Up!");
			levelUp = true;
			DistributeStats (4);
		} else { 
			skillUp = true;
			Skill skill = Database.skills.GetSkill (skillid);
			leveledSkill.Add (skill.name);
			if (skill.statgrowth.Count > 0) {
				foreach (KeyValuePair<string,int> stat in skill.statgrowth) {
					baseStats [stat.Key] += stat.Value;
				}
			}
		}
	}

	public void Heal (int amount, string method, string type)
	{
		if (method == "Flat") {
			totalStats ["Current" + type] += amount;
		} else if (method == "Percent") {
			int heal = Mathf.RoundToInt ((float)totalStats ["Max" + type] * amount / 100);
			Debug.Log (Mathf.RoundToInt ((float)totalStats ["Max" + type] * amount / 100));
			if (heal < 1) {
				heal = 1;
			}
			totalStats ["Current" + type] += heal;
		}
		if (totalStats ["Current" + type] > totalStats ["Max" + type])
			totalStats ["Current" + type] = totalStats ["Max" + type];
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
		List<string> stats = new List<string> (bonusStats.Keys);
		foreach (string stat in stats) {
			bonusStats [stat] = 0;
		}
		UpdateStats ();
	}

	public bool HasHealingItems (string type = "Health")
	{
		if (GetHealingItem (type) != 0) {
			return true;
		}
		return false;
	}

	public int GetHealingItem (string type)
	{
		for (int i = 2; i < tempEquipment.Count; i++) {
			if (tempEquipment [i].filled) {
				Item item = Database.items.FindItem (tempEquipment [i].itemId);
				if (item.subType == "Heal" && item.stats.ContainsKey (type))
					return i;
			}
		}
		return 0;
	}
	public List<int> GetConsumables(){
		List<int> slotsWithConsumables = new List<int> ();
		for (int i = 2; i < tempEquipment.Count; i++) {
			if (tempEquipment [i].filled) {
				Item item = Database.items.FindItem (tempEquipment [i].itemId);
				if (item.type == "Consumable")
					slotsWithConsumables.Add (i);
			}
		}
		return slotsWithConsumables;
	}

	public bool HasConsumables(){
		for (int i = 2; i < tempEquipment.Count; i++) {
			if (tempEquipment [i].filled) {
				Item item = Database.items.FindItem (tempEquipment [i].itemId);
				if (item.type == "Consumable")
					return true;
			}
		}
		return false;
	}

	public void UseConsumable(int slotId, Adventure adventure){
		InventorySlot slot = tempEquipment [slotId];
		Item item = Database.items.FindItem (slot.itemId);
		foreach (KeyValuePair<string,int> stat in item.stats) {
			if (stat.Key == "Health" || stat.Key == "Mana") {
				Heal (stat.Value, "Flat", stat.Key);
				adventure.actionList.Add (nickname + " used " + item.name + " and recovered " + stat.Value + " " + stat.Key + ".");
			}
		}
		slot.Use ();
	}
	public void UseHealingItem (string type = "Health", Adventure adventure=null)
	{
		int slotId = GetHealingItem (type);
		UseConsumable (slotId, adventure);
	}

	public string ShortDescription ()
	{
		if (levelUp || skillUp) {
			return string.Format (Database.strings.GetString ("MemberUp"), nickname, Database.strings.GetString (gender + "Poss"));
		} 
		return null;
	}

	public string Details ()
	{
		string details = "";
		if (levelUp) {
			string stats = "";
			foreach (KeyValuePair<string,int> stat in levelUpStats) {
				if (stat.Value > 0) {
					stats += stat.Value.ToString () + " " + Database.strings.GetString (stat.Key) + "\n";
				}
			}
			details = string.Format (Database.strings.GetString ("LevelUp"), nickname, stats) + "\n";
		}
		if (skillUp) {
			foreach (string skill in leveledSkill) {
				details += string.Format (Database.strings.GetString ("SkillUp"), nickname, skill) + "\n\n";
			}
		}
		return details;
	}

	public Ability GetHealingAbility ()
	{
		for (int i = 0; i < abilities.Count; i++) {
			Ability ability = Database.skills.GetAbility (abilities [i]);
			if (ability.element == "Healing" && totalStats ["CurrentMana"] > ability.manaCost) {
				return ability;
			}
		}
		return null;
	}

	public bool HasHealingAbility ()
	{
		if (GetHealingAbility () != null) {
			return true;
		}
		return false;
	}

	public void Heal (Character character, Adventure adventure)
	{
		character.Heal (Mathf.RoundToInt (totalStats ["MAttack"] / 2), "Flat", "Health");
		Ability ability = GetHealingAbility ();
		totalStats ["CurrentMana"] -= ability.manaCost;
		string targetname = character.nickname;
		if (character == this) {
			targetname = Database.strings.GetString (gender + "Poss") + "self";
		}

		adventure.actionList.Add (nickname + " used " +ability.name+" on " + targetname + ", healed " + Mathf.RoundToInt (totalStats ["MAttack"] / 2).ToString () + " health");
	}
}
