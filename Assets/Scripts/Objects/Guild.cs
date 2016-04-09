using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Guild
{
	public int id;
	public string name;
	public int icon;
	public int level = 1;
	public int exp = 0;
	public int requiredExp;
	public int fame = 0;
	public int size = 0;
	public int money = 0;
	public Inventory inventory;
	public List<Character> characterlist = new List<Character> ();
	public QuestBoard questBoard;
	public List<Task> taskList = new List<Task> ();
	//public Dictionary<int,List<Task>> taskLog = new Dictionary<int,List<Task>> ();
	public List<int> knownAreas = new List<int> ();
	public Dictionary<int,int> foundGatheringPoints = new Dictionary<int,int> ();
	public Dictionary<int,int> foundHuntingGrounds = new Dictionary<int,int> ();
	public Dictionary<int,int> successfulVisits = new Dictionary<int,int> ();
	public List<Task> tasklog = new List<Task> ();
	public Dictionary<int,int> upgradelist = new Dictionary<int,int> ();
	public int maintenanceCost;
	public int questFinished = 0;
	public int itemsGathered = 0;
	public int itemsSold = 0;
	public int monstersSlain = 0;
	public int tasksGiven = 0;
	public int membersInjured = 0;
	public int visitedShop = 0;
	public int visitedSchool = 0;
	public int visitedTavern = 0;
	public bool paidMaintenance = false;
	public bool levelUp=false;

	public Guild ()
	{
	}

	public Guild (int id, string name, int level, int fame, int money)
	{
		this.id = id;
		this.name = name;
		this.level = level;
		this.fame = fame;
		this.money = money;
		this.inventory = new Inventory ();

		this.questBoard = new QuestBoard ();
		for (int i=0; i<Database.upgrades.GetUpgradeListSize(); i++) {
			upgradelist [i] = 0;
		}
		this.inventory.size = Database.upgrades.GetUpgrade (1).MaxSize (upgradelist [1]);
		this.questBoard.size = Database.upgrades.GetUpgrade (2).MaxSize (upgradelist [2]);
		requiredExp = Database.guilds.RequiredExpTNL (level);
	}

	public void RecruitCharacter (Character newcharacter)
	{
		characterlist.Add (newcharacter);
		newcharacter.recruited = true;
		newcharacter.guildnr = characterlist.Count;
		size = characterlist.Count;
	}

	public List<Character> GetAvailableCharacters ()
	{
		List<Character> availablecharacters = new List<Character> ();
		for (int i=0; i<characterlist.Count; i++) {
			if (characterlist [i].status == "Idle") {
				availablecharacters.Add (characterlist [i]);
			}
		}
		return availablecharacters;
	}

	public Character GetCharacter (int id)
	{
		foreach (Character character in characterlist) {
			if (character.guildnr == id) {
				return character;
			}
		}
		return null;
	}

	public void AddTask (Task task)
	{
		taskList.Add (task);
		if (task.shoppingMoney > 0) {
			money -= task.shoppingMoney;
		}
	}

	public void UpdateTasks (int day)
	{
		if (taskList.Count > 0) {
			for (int i=0; i<taskList.Count; i++) {
				taskList [i].UpdateTask ();
				if (taskList [i].duration <= 0) {
					tasklog.Add (taskList [i]);
				}
			}
			if (tasklog.Count > 0) {
				for (int i=0; i<tasklog.Count; i++) {
					if (tasklog [i].type == "Quest") {
						questBoard.RemoveQuest (tasklog [i].questnumber);
						questFinished++;
					} else if (tasklog [i].type == "Sell") {
						itemsSold += tasklog [i].itemsSold.GetAllItems ().Count;
					} else if (tasklog [i].type == "Adventure") {
						monstersSlain += tasklog [i].monstercount;
						membersInjured += tasklog [i].casualties.Count;
						if (tasklog [i].success) {
							itemsGathered += tasklog [i].itemList.Count;
							if (successfulVisits.ContainsKey (tasklog [i].area.id)) {
								successfulVisits [tasklog [i].area.id]++;
							} else {
								successfulVisits [tasklog [i].area.id] = 1;
							}
						}
					} else if (tasklog [i].type == "Socialize") {
						visitedTavern++;
					} else if (tasklog [i].type == "Shop") {
						visitedShop++;
					} else if (tasklog [i].type == "School") {
						visitedSchool++;
					}
					taskList.Remove (tasklog [i]);
				}
				tasksGiven += tasklog.Count;
			}
		}
		maintenanceCost += DailyMaintenance();
	}
	
	public int DailyMaintenance(){
		int cost=0;
		foreach (KeyValuePair<int,int> upgradelevel in upgradelist) {
			cost+= Database.upgrades.GetUpgrade (upgradelevel.Key).DailyCost (upgradelevel.Value);
		}
		return cost;
	}
	public void PayMaintenance ()
	{
		money -= maintenanceCost;
		paidMaintenance = true;
	}

	public void UpdateCharacters ()
	{
		foreach (Character character in characterlist) {
			if (character.totalStats ["CurrentHealth"] > 0|| character.status == "Resting") {
				if (character.totalStats ["CurrentHealth"]<character.totalStats ["MaxHealth"]){
					character.Heal (100, "Percent", "Health");
					character.Heal (100, "Percent", "Mana");
				}
				character.status = "Idle";
			} else if (character.status == "Idle") {
				character.totalStats ["CurrentHealth"] = 0;
				character.status = "Resting";
			}
		}
	}

	public List<Character> GetCharacterActivity ()
	{
		List<Character> characters = new List<Character> ();
		foreach (Character character in characterlist) {
			if (character.levelUp || character.skillUp) {
				characters.Add (character);
			}
		}
		return characters;
	}

	public void FinishShopping (Dictionary<int,int> items, int returnmoney)
	{
		money += returnmoney;
		GetItems (items);
	}

	public void GetItems (Dictionary<int,int> items)
	{
		foreach (KeyValuePair<int,int> item in items) {
			inventory.AddItem (item.Key, item.Value);
		}
	}

	public void CancelTask (int index)
	{
		taskList [index].Cancel ();
		taskList.RemoveAt (index);
	}

	public void FinishQuest (int questid, List<Character> characters, bool showDialog=true)
	{
		Quest quest = questBoard.FindQuest (questid);
		if (quest.requiredItems != null && quest.requiredItems.Count > 0) {
			inventory.RemoveItem (quest.requiredItems);
		}
		if (quest.expReward != null && characters != null) {
			foreach (Character character in characters) {
				foreach (KeyValuePair<int,int> exp in quest.expReward) {
					character.GiveExp (exp.Key, exp.Value);
				}
			}
		}
		if (quest.itemRewards != null) {
			foreach (KeyValuePair<int,int> item in quest.itemRewards) {
				inventory.AddItem (item.Key, item.Value);
			}
		}
		GiveExp (quest.guildExpReward);
		money += quest.moneyReward;
		if (showDialog){
			GameObject.FindObjectOfType<DialogueScreenDisplay> ().ShowDialogue (quest.guildExpReward,quest.moneyReward, quest.itemRewards,quest.expReward,null);
		}
		quest.finished = true;
	}

	public bool CanFinishQuest (int questid)
	{
		Quest quest = questBoard.FindQuest (questid);
		if (quest.requiredItems != null) {
			foreach (KeyValuePair<int,int> item in quest.requiredItems) {
				if (!inventory.Contains (item.Key, item.Value)) {
					return false;
				}
			}
		}
		return true;
	}

	public Character FindNewRecruit (int level, int mainSkill)
	{
		if (level < 1)
			level = 1;
		return Database.characters.GenerateNewCharacter (level, mainSkill);
	}

	public void FindNewArea (int id)
	{
		if (!knownAreas.Contains (id)) {
			knownAreas.Add (id);
			successfulVisits [id] = 0;
			foundHuntingGrounds [id] = 0;
			foundGatheringPoints [id] = 0;
		}
	}

	public void NextDayReset ()
	{
		foreach (Character character in characterlist) {
			character.NextDayResets ();
		}
		if (paidMaintenance){
			paidMaintenance = false;
			maintenanceCost=0;
		}
		levelUp=false;
		tasklog.Clear ();
	}

	public void GiveItemToCharacter (int inventoryId, int equipslotId, int characterId)
	{
		Character character = GetCharacter (characterId);
		if (character.equipment [equipslotId].filled) {
			inventory.AddItem (character.equipment [equipslotId].itemId, 1, character.equipment [equipslotId].durability);
		}
		if (inventoryId != 999) {
			InventorySlot slot = inventory.GetInventorySlot (inventoryId);
			character.Equip (equipslotId, slot.itemId, slot.durability);
			inventory.RemoveItemFromSlot (inventoryId, 1);
		} else {
			character.UnEquip (equipslotId);
		}
	}

	public bool CanUpgrade (int id)
	{
		if (id!=0&&upgradelist[0]==0){
			return false;
		}
		Upgrade upgrade = Database.upgrades.GetUpgrade (id);
		if (money < upgrade.UpgradeCost (upgradelist [id]) || Mathf.RoundToInt (upgradelist [id] * 50 / upgrade.maxLevel) >= level) {
			return false;
		}
		Dictionary<int,int> materialCost = upgrade.MaterialCost (upgradelist [id]);
		foreach (KeyValuePair<int,int> material in materialCost) {
			if (!inventory.Contains (material.Key, material.Value)) {
				return false;
			}
		}
		return true;
	}

	public void Upgrade (int id)
	{
		Upgrade upgrade = Database.upgrades.GetUpgrade (id);
		money -= upgrade.UpgradeCost (upgradelist [id]);
		Dictionary<int,int> materialCost = upgrade.MaterialCost (upgradelist [id]);
		foreach (KeyValuePair<int,int> material in materialCost) {
			inventory.RemoveItem (material.Key, material.Value);
		}
		upgradelist [id] += 1;
		inventory.size = Database.upgrades.GetUpgrade (1).MaxSize (upgradelist [1]);
		questBoard.size = Database.upgrades.GetUpgrade (2).MaxSize (upgradelist [2]);
	}

	public void GiveExp (int exp)
	{
		this.exp += exp;
		if (requiredExp <= this.exp) {
			this.exp -= requiredExp;
			level++;
			requiredExp = Database.guilds.RequiredExpTNL (level);
			levelUp=true;
		}
	}
}
