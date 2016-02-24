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
	public string gender;
	public int level;
	public int exp = 0;
	public string status="Idle";
	public string statusAdd="";
	public Dictionary<string,int> baseStats = new Dictionary<string, int> ();
	public Dictionary<string,int> bonusStats = new Dictionary<string, int> ();
	public Dictionary<string,int> equipmentStats = new Dictionary<string, int> ();
	private Dictionary<string,int> statGrowth = new Dictionary<string,int> ();
	public Dictionary<string,int> totalStats = new Dictionary<string, int> ();
	public Dictionary<int,int> skillLevel = new Dictionary<int,int > ();
	public Dictionary<int,int> skillExp = new Dictionary<int,int > ();
	public List<InventorySlot> equipment = new List<InventorySlot> ();
	public List<int> abilities;
	public bool recruited = false;
	public bool levelUp = false;
	public Dictionary<string,int> levelUpStats = new Dictionary<string, int> ();
	public bool skillUp = false;
	public List<string> leveledSkill = new List<string> ();
	public bool didDamage = false;
	public int blockItem = 0;
	public bool canAttack = false;
	public bool isEnemy=false;

	public Character ()
	{

	}

	public Character (int id, string name, bool male, int level, string type="")
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
		statGrowth ["Health"] = (Random.Range (60, 120) + Random.Range (60, 120)) / 2 + healthmanamod;
		statGrowth ["Mana"] = (Random.Range (60, 120) + Random.Range (60, 120)) / 2 - healthmanamod;
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
		SetUpDefaultStats(level);
	}
	public Character(Monster monster,int level=1,bool isEnemy=true){
		id = monster.id;
		name = monster.name;
		this.level = 1;
		statGrowth = monster.statGrowth;
		SetUpDefaultStats(level);
		this.isEnemy=isEnemy;
	}

	public void SetUpDefaultStats(int level){
		baseStats ["Health"]=5;
		baseStats ["Mana"]=5;
		baseStats ["Strength"] = 0;
		baseStats ["Dexterity"] = 0;
		baseStats ["Agility"] = 0;
		baseStats ["Intelligence"] = 0;
		baseStats ["Attack"] = 0;
		baseStats ["Defense"] = 0;
		baseStats ["MagicDefense"] = 0;
		baseStats ["Fame"] = 0;
		totalStats ["MaxHealth"]=0;
		totalStats ["MaxMana"] = 0;
		totalStats ["PAttack"] = 0;
		totalStats ["MAttack"] = 0;
		totalStats ["PDefense"] = 0;
		totalStats ["MDefense"] = 0;
		totalStats ["Accuracy"] = 70;
		totalStats ["Evade"] = 10;
		totalStats ["Block"] = 0;
		totalStats ["BlockChance"] = 0;
		totalStats ["Speed"] = 0;
		totalStats ["Weight"] = 0;
		totalStats ["CarrySize"]=0;
		foreach(string stat in baseStats.Keys){
			bonusStats[stat]=0;
		}
		foreach (string stat in totalStats.Keys){
			equipmentStats[stat]=totalStats[stat];
		}
		foreach (string stat in statGrowth.Keys)
			levelUpStats [stat] = 0;
		totalStats ["CurrentHealth"] = 0;
		totalStats ["CurrentMana"] = 0;
		DistributeStats (20);
		Heal (100, "percent", "Health");
		Heal (100, "percent", "Mana");
		for (int i=0; i<5; i++) {
			equipment.Add (new InventorySlot (i));
		}
		for (int i=0; i<Database.skills.SkillList().Count; i++) {
			skillLevel.Add (i, 1);
			skillExp.Add (i, 0);
		}
		abilities = new List<int> (){0,1,2,3};
		if (level > 1)
			GiveExp (Database.skills.SkillList ().Count,(level - 1) * 100);
	}

	public Ability ChooseAttack ()
	{
		List<Ability> abilities = GetUsableAbilities ();
		Ability chosenAbility;
		if (abilities.Count > 1) {
			chosenAbility = abilities [Random.Range (0, abilities.Count)];
		} else {
			chosenAbility = abilities [0];
		}
		totalStats["CurrentMana"]-=chosenAbility.manaCost;
		Debug.Log(name+" uses " +chosenAbility.name+" "+System.DateTime.Now.Millisecond.ToString());
		canAttack = true;
		return chosenAbility;
	}

	public Ability ChooseCounterAttack (int range)
	{
		List<Ability> abilities = GetUsableAbilities (false, range);
		Ability chosenAbility=null;
		if (abilities.Count == 0) {
			Debug.Log(name+" can't attack from this range! "+System.DateTime.Now.Millisecond.ToString());
			canAttack = false;
		} else {
			if (abilities.Count > 1) {
				chosenAbility = abilities [Random.Range (0, abilities.Count)];
			} else if (abilities.Count == 1) {
				chosenAbility = abilities [0];
			}
			Debug.Log(name+" uses " +chosenAbility.name+" as counterattack "+System.DateTime.Now.Millisecond.ToString());
			totalStats["CurrentMana"]-=chosenAbility.manaCost;
			canAttack = true;
		}
		return chosenAbility;
	}

	public List<Ability> GetUsableAbilities (bool attacking=true, int range=1)
	{
		List<Ability> usableAbilities = new List<Ability> ();
		string weapontype = GetWeaponType ();
		for (int i=0; i<abilities.Count; i++) {
			Ability ability = Database.skills.GetAbility (abilities [i]);
			if ((ability.weaponType==null || (ability.weaponType!=null&& ability.weaponType.Contains (weapontype))) && ability.manaCost <= totalStats ["CurrentMana"] && ability.element != "Healing") {
				if (attacking || (!attacking && ability.range == range))
					usableAbilities.Add (ability);
			}
		}
		return usableAbilities;
	}

	public void Attack (Character target, Ability chosenAbility)
	{

		bool hit = false;


		if (chosenAbility.element == "Physical") {
			Debug.Log (name + " attacks (" +System.DateTime.Now.Millisecond.ToString() + ")");
			hit = target.Defend (totalStats ["PAttack"], totalStats ["Accuracy"], "P", GetWeaponType (), GetWeaponElement ());
		} else {
			Debug.Log (name + " attacks (" +System.DateTime.Now.Millisecond.ToString() + ")");
			hit = target.Defend (totalStats ["MAttack"], totalStats ["Accuracy"], "M", "", chosenAbility.element);
		}
		if (hit) {
			if (equipment [0].filled) {
				equipment [0].Use ();
				if (equipment [1].durability <= 0) {
					canAttack = false;
				}
			}
			didDamage = true;
		}
	}

	public string GetWeaponType ()
	{
		if (equipment [0].filled) {
			return Database.items.FindItem (equipment [0].itemId).subType;
		}
		return "Fist";
	}

	public string GetWeaponElement ()
	{
		if (equipment [0].filled) {
			return Database.items.FindItem (equipment [0].itemId).element;
		}
		return "";
	}

	public bool Defend (int damage, int hitrate, string damageType, string damageSubType, string element)
	{
		int RNG = ExtensionMethods.Calculate(Random.Range (0, 101)+Random.Range (0, 101),0.5f) + totalStats ["Evade"];
		if (hitrate > RNG) {
			int defense = totalStats [damageType + "Defense"];
			if (hitrate - totalStats ["BlockChance"] < RNG && totalStats ["BlockChance"] > 0) {
				defense += totalStats ["Block"];
				Debug.Log (name + " blocked the attack  (" +System.DateTime.Now.Millisecond.ToString()+ ")");
				if (equipment [blockItem].filled) {
					equipment [blockItem].Use ();
				}
			}
			if (damageSubType == "Mace" || element == "Omni") {
				defense = Calculate (defense, 0.75f);
			}
			damage -= defense;
			if (damage < 0) {
				damage = 0;
			}
			totalStats ["CurrentHealth"] -= damage;
			Debug.Log (name + " received " + damage.ToString () + " damage. (" + totalStats ["CurrentHealth"].ToString () + " health left, blocked " + defense + " damage)"+System.DateTime.Now.Millisecond.ToString());
			if (equipment [1].filled) {
				equipment [1].Use ();
			}
			for (int i=2; i<equipment.Count; i++) {
				if (equipment [i].filled && i != blockItem && equipment [i].ItemType () != "Consumable") {
					if (Random.Range (0, 101) < 35) {
						equipment [i].Use ();
					}
				}
			}
			return true;
		} else {
			Debug.Log (name + " dodged the attack (" + RNG.ToString () + ")"+System.DateTime.Now.Millisecond.ToString());
			return false;
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
			while (this.exp>=100) {
				level += 1;
				LevelUp (Database.skills.SkillList ().Count);
				this.exp -= 100;
				if (level == 60) {
					this.exp = 0;
				}
			}
		}
	}

	public void UpdateStats ()
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
				while (rng[growth.Key]>=100) {
					baseStats [growth.Key] += 1;
					statgains += 1;
					if (levelUp) {
						levelUpStats [growth.Key] += 1;
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
			Debug.Log(name+" levels Up!");
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
			totalStats ["Current" + type] += Mathf.RoundToInt (totalStats ["Max" + type] * amount / 100);
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

	public bool HasHealingItems (string type="Health")
	{
		if (GetHealingItem (type) != 0) {
			return true;
		}
		return false;
	}

	public int GetHealingItem (string type)
	{
		for (int i=2; i<equipment.Count; i++) {
			if (equipment [i].filled) {
				Item item=Database.items.FindItem (equipment [i].itemId);
				if (item.subType == "Heal"&& item.stats.ContainsKey(type))
					return i;
			}
		}
		return 0;
	}

	public void UseHealingItem (string type="Health")
	{
		InventorySlot slot = equipment [GetHealingItem (type)];
		Item item = Database.items.FindItem (slot.itemId);
		foreach (KeyValuePair<string,int> stat in item.stats) {
			Heal (stat.Value, "Flat", stat.Key);
			Debug.Log (name + " used " + item.name + " and recovered " + stat.Value + " "+type+".");
		}

		slot.Use ();
	}

	public string ShortDescription ()
	{
		if (levelUp || skillUp) {
			return string.Format (Database.strings.GetString ("MemberUp"), name, Database.strings.GetString (gender + "Poss"));
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
			details = string.Format (Database.strings.GetString ("LevelUp"), name, stats) + "\n";
		}
		if (skillUp) {
			foreach (string skill in leveledSkill) {
				details += string.Format (Database.strings.GetString ("SkillUp"), name, skill) + "\n\n";
			}
		}
		return details;
	}
	public Ability GetHealingAbility(){
		for (int i=0;i<abilities.Count;i++){
			Ability ability=Database.skills.GetAbility(abilities[i]);
			if (ability.element=="Healing"&& totalStats["CurrentMana"]>ability.manaCost){
				return ability;
			}
		}
		return null;
	}
	public bool HasHealingAbility(){
		if (GetHealingAbility()!=null){
			return true;
		}
		return false;
	}

	public void Heal(Character character){
		character.Heal(Mathf.RoundToInt(totalStats["MAttack"]/2),"Flat","Health");
		totalStats["CurrentMana"]-=GetHealingAbility().manaCost;
		Debug.Log(name+ " healed " +character.name+ " for "+Mathf.RoundToInt(totalStats["MAttack"]/2).ToString() +" health "+System.DateTime.Now.Millisecond.ToString());
	}
}
