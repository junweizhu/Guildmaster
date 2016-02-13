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
	public List<Task> tasklog=new List<Task>();
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
		task.guild=this;
		if (task.shoppingMoney > 0) {
			money -= task.shoppingMoney;
		}
	}

	public void UpdateTasks (int day)
	{
		tasklog.Clear();
		if (taskList.Count > 0) {
			for (int i=0; i<taskList.Count; i++) {
				taskList [i].UpdateTask ();
				if (taskList [i].duration <= 0) {
					//if (!taskLog.ContainsKey (day))
						//taskLog [day] = new List<Task> ();
					//taskLog [day].Add (taskList [i]);
					tasklog.Add (taskList [i]);
				}
			}
			if (tasklog.Count > 0) {
				for (int i=0; i<tasklog.Count; i++) {
					if (tasklog [i].type == "Quest") {
						questBoard.RemoveQuest (tasklog [i].questnumber);
					}
					taskList.Remove (tasklog [i]);
				}
			}
		}
	}

	public void UpdateCharacters ()
	{
		foreach (Character character in characterlist) {
			if (character.totalStats ["CurrentHealth"] > 0 || character.status == "Resting") {
				character.Heal (100, "Percent", "Health");
				character.Heal (100, "Percent", "Mana");
				character.status="Idle";
			} else if (character.status == "Idle") {
				character.totalStats ["CurrentHealth"] = 0;
				character.status = "Resting";
			}
		}
	}
	/*public List<string> GetCharacterActivity ()
	{
		List<string> guildinfo = new List<string> ();
		foreach (Character character in characterlist) {
			if (character.levelUp) {
				string info = character.name + " " + Database.strings.GetString ("LevelUp");
				int count = 0;
				foreach (KeyValuePair<string,int> stat in character.levelUpStats) {
					if (stat.Value != 0) {
						count++;
					}
				}
				foreach (KeyValuePair<string,int> stat in character.levelUpStats) {
					if (stat.Value != 0) {
						count--;
						info += " " + stat.Value.ToString () + " " + Database.strings.GetString (stat.Key);
						if (count == 1) {
							info += " " + Database.strings.GetString ("And");
						} else if (count != 0) {
							info += ",";
						} else {
							info += ".";
						}
					}
				}
				guildinfo.Add (info);
			}
			if (character.skillUp) {
				string info = character.name + " " + Database.strings.GetString ("SkillUp")+" "+ Database.strings.GetString (character.gender+"Poss");
				for (int i=0; i<character.leveledSkill.Count; i++) {
					info += " " + character.leveledSkill [i] + "s";
					if (i == character.leveledSkill.Count - 2) {
						info += " " + Database.strings.GetString ("And");
					} else if (i != character.leveledSkill.Count - 1) {
						info += ",";
					} else {
						info += ".";
					}
						
				}
				guildinfo.Add (info);
			}
			if (character.status=="Resting"){
				if (character.baseStats["CurrentHealth"]==0){
					guildinfo.Add (character.name +" "+ Database.strings.GetString ("Injured"));
				} else{
					guildinfo.Add (character.name +" "+ Database.strings.GetString ("Recovered1")+" "+Database.strings.GetString (character.gender+"Poss")+" "+Database.strings.GetString ("Recovered2"));
					character.status="Idle";
				}
			}
		}
		if (newcharacters.Count > 0) {
			foreach (string name in newcharacters) {
				guildinfo.Add (name + " " + Database.strings.GetString ("Join"));
			}
		}
		return guildinfo;
	}*/
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

	public void FinishQuest (int questid, List<Character> characters)
	{
		Quest quest = questBoard.FindQuest (questid);
		if (quest.requiredItems.Count > 0) {
			inventory.RemoveItem (quest.requiredItems);
		}
		if (characters != null) {
			foreach (Character character in characters) {
				foreach (KeyValuePair<int,int> exp in quest.expReward) {
					character.GiveExp (exp.Key, exp.Value);
				}
			}
		} else {
			foreach (KeyValuePair<int,int> exp in quest.expReward) {
				characterlist [0].GiveExp (exp.Key, exp.Value);
			}
		}
		if (quest.itemRewards != null) {
			foreach (KeyValuePair<int,int> item in quest.itemRewards) {
				inventory.AddItem (item.Key, item.Value);
			}
		}
		money += quest.moneyReward;
		quest.finished=true;
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

	/*public void FindNewQuest (int level)
	{
		questBoard.AddQuest (level);
	}*/

	public Character FindNewRecruit (int level, string mainSkill)
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
	}

	/*public void GiveItemsToCharacter (Dictionary<int,int> itemlist, int characterid)
	{
		foreach (KeyValuePair<InventorySlot,int> slot in inventory.GetAllItems(itemlist)) {
			GetCharacter (characterid).AddItem (slot.Key, slot.Value);
		}
		inventory.RemoveItem (itemlist);
	}*/

	public void GiveItemToCharacter(int inventoryId, int equipslotId, int characterId){
		Character character=GetCharacter (characterId);
		if (character.equipment[equipslotId].filled){
			inventory.AddItem(character.equipment[equipslotId].itemId,1,character.equipment[equipslotId].durability);
		}
		if (inventoryId!=999){
			InventorySlot slot =inventory.GetInventorySlot(inventoryId);
			character.Equip(equipslotId,slot.itemId,slot.durability);
			inventory.RemoveItemFromSlot(inventoryId,1);
		} else{
			character.UnEquip(equipslotId);
		}
	}
}
