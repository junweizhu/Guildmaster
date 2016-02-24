﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Task
{

	public string type;
	public float duration;
	public int questnumber;
	[System.NonSerialized]
	public List<Character>
		characters;
	public Dictionary<int,int>itemList;
	public Inventory itemsSold;
	public Inventory stock;
	public string typeSearch;
	public int shoppingMoney;
	private int returnMoney;
	public bool success;
	private Area area;
	private int gatheringPointsFound = 0;
	private int huntingGroundsFound = 0;
	private Area newArea;
	private Dictionary<string,Dictionary<string,int>> playerExpGain = new Dictionary<string,Dictionary<string,int>> ();
	private int monstercount = 0;
	private List<string> newRecruits = new List<string> ();
	private int questCount = 0;
	private Quest quest;
	private List<string> casualties=new List<string>();
	private Skill skill;
	private Ability ability;

	public Task ()
	{

	}

	public Task (string type, Quest quest, List<Character> characters, int questnumber)//quest
	{
		SetMainData (type, characters);
		duration = quest.duration;
		this.quest = quest;
		this.questnumber = questnumber;
	}

	public Task (string type, float duration, List<Character> characters, Dictionary<int,int>items, int money)//buying or selling items
	{

		SetMainData (type, characters);
		this.duration = duration;
		if (type=="Shop"){
			itemList = items;
		} else{
			Inventory inventory = Database.myGuild.inventory;
			stock=new Inventory(items.Count);
			foreach (KeyValuePair<int,int> itemslot in items){
				InventorySlot slot=inventory.GetInventorySlot(itemslot.Key);
				stock.AddItem(slot.itemId,itemslot.Value,slot.durability);
				slot.AddQuantity(0-itemslot.Value);
			}
			itemsSold=new Inventory(items.Count);
		}
		shoppingMoney = money;
	}

	public Task(string type, float duration, List<Character> characters, Skill skill,Ability ability, int money){ //training
		SetMainData(type, characters);
		this.duration=duration;
		this.skill=skill;
		this.ability=ability;
		shoppingMoney=money;
	}

	public Task (string type, float duration, List<Character> characters, string typesearch)//searching at tavern
	{
		SetMainData (type, characters);
		this.duration = duration;
		this.typeSearch = typesearch;
	}

	public Task (string type, Area area, List<Character> characters, string typesearch)//exploring
	{
		SetMainData (type, characters);
		this.area = area;
		this.duration = area.travelTime + 1;
		this.typeSearch = typesearch;
		itemList = new Dictionary<int, int> ();
		success=true;
	}

	public Task (string type, float duration, List<Character> characters)//Tavern
	{
		SetMainData (type, characters);
		this.duration = duration;
		itemList = new Dictionary<int, int> ();
		
	}

	void SetMainData (string type, List<Character> characters)
	{
		this.type = type;
		this.characters = characters;
	}

	public void UpdateTask ()
	{
		duration -= 1;
		if (type == "Quest") {
			quest.duration -= 1;
		}
		if (duration <= 0) {
			if (type == "Quest") {
				Database.myGuild.FinishQuest (questnumber, characters);
			}
			if (type == "Shop") {
				float payment = 1.00f;
				for (int i=0; i< characters.Count; i++) {
					GiveExp (characters [i], "Social Skill", 10);
					if (Random.Range (0, 100) > (100 - (characters [i].skillLevel [Database.skills.GetSkill ("Social Skill")] / (i + 1)))) {
						payment *= 1.0f - (0.5f / (2 + i));
					}
				}
				returnMoney = Mathf.CeilToInt (shoppingMoney * (1 - payment));
				Database.myGuild.FinishShopping (itemList, returnMoney);
				return;
			}
			if (type == "Sell") {
				SellTime();
				if (stock.Count()>0){
					Inventory inventory=Database.myGuild.inventory;
					foreach (int slot in stock.GetAllFilledSlotId()){
						InventorySlot itemslot=stock.GetInventorySlot(slot);
						inventory.AddItem(itemslot.itemId,itemslot.quantity,itemslot.durability);
						itemslot.EmptyItem();
					}
				}
				Database.myGuild.money+=TotalProfit();
			}
			if (type=="School"){
				foreach(Character character in characters){
					if (ability!=null){
						character.abilities.Add(ability.id);
					}
					if (skill!=null){
						GiveExp(character,skill.name,100);
					}
				}
			}
			if (type == "Socialize") {
				Dictionary<string,string> typeSkill = new Dictionary<string, string> (){{"Fighter","Weapon Skill"},{"Mage","Magic Skill"},{"Adventurer","Field Skill"},{"Social","Social Skill"}};
				
				for (int i=0; i< characters.Count; i++) {
					int fame = (Random.Range (-2, 2) + Random.Range (-2, 2)) / 2;
					List<string> skills = new List<string> (){"Fighter","Mage","Adventurer","Social"};
					typeSearch = skills [Random.Range (0, skills.Count)];
					float chance = characters [i].skillLevel [Database.skills.GetSkill ("Social Skill")] + characters [i].skillLevel [Database.skills.GetSkill (typeSkill [typeSearch])] / 2;
					if (Random.Range (0, 100) > 90 - chance) {
						newRecruits.Add (Database.myGuild.FindNewRecruit (Random.Range (0, characters [i].level + Random.Range (0, 10) - 5), typeSkill [typeSearch]).name);
						GiveExp (characters [i], "Social Skill", 10);
						fame = 2;
					} else if (Random.Range (0, 100) > 90 - chance) {
						Debug.Log ("They found a quest");
						questCount += 1;
						int rng = Random.Range (0, 10) - 5;
						if (rng < 1) {
							rng = 1;
						}
						Database.quests.GenerateQuest ((Random.Range (0, characters [i].level + rng)));
						GiveExp (characters [i], "Social Skill", 10);
						fame = 2;
					} else if (Random.Range (0, 100) > 90 - chance){
						Debug.Log ("They talked about a new area");
						int rng=Database.areas.RandomArea(characters[i].level);
						if (Database.myGuild.knownAreas.Contains(rng)){
							Debug.Log ("We already know this area");
						} else{
							Debug.Log ("We never knew this area");
							Database.myGuild.FindNewArea(rng);
						}
					}
					GiveExp (characters [i], "Social Skill", Random.Range (0, 25));
					characters [i].baseStats ["Fame"] += fame;
					Database.myGuild.fame += fame;
				}
			}
			if (type == "Adventure") {
				AdventureTime (false);
				if (success && itemList.Count > 0) {
					Database.myGuild.GetItems (itemList);
				} 
				if (characters.IsInjured()){
					foreach (Character character in characters){
						if (character.totalStats["CurrentHealth"]<=0){
							casualties.Add(character.name);
						}
					}
				}
			}
			for (int i=0; i< characters.Count; i++) {
				characters [i].status = "Idle";
				characters [i].statusAdd = "";
			}
		}
	}

	public string ShortDescription ()
	{
		string names = "";
		for (int i=0; i<characters.Count; i++) {
			if (i != 0) {
				if (i + 1 == characters.Count) {
					names += " " + Database.strings.GetString ("And") + " ";
				} else {
					names += ", ";
				}
			}
			names += characters [i].name;
		}
		return string.Format (Database.strings.GetString (type + "Log"), names);
	}

	public string Details ()
	{
		string detail = "";
		if (type == "Shop") {
			detail = string.Format (Database.strings.GetString ("Bought"), Itemlist ());
			if (returnMoney > 0) {
				detail += string.Format (Database.strings.GetString ("MoneyBack"), returnMoney);
			}
		}
		if (type == "Adventure") {
			if (success) {
				detail = Database.strings.GetString (type + "Success") + "\n\n";
				if (monstercount > 0) {
					detail += string.Format (Database.strings.GetString ("MonstersFought"), monstercount) + "\n\n";
				}
				if (itemList.Count > 0) {
					detail += string.Format (Database.strings.GetString ("Obtained"), Itemlist () + "\n");
				}
			} else if (characters.Count > 1) {
				detail = string.Format (Database.strings.GetString (type + "Failed"), Database.strings.GetString ("Plural3rd")) + "\n\n";
			} else {
				detail = string.Format (Database.strings.GetString (type + "Failed"), Database.strings.GetString (characters [0].gender + "3rd")) + "\n\n";
			}
			if (casualties.Count>0){
				for (int i=0;i<casualties.Count;i++){
					detail+=string.Format (Database.strings.GetString ("Injured"), casualties[i])+"\n";
				}
				detail+="\n";
			}
		}
		if (type=="School"){
			string stringtype=type;
			if (ability!=null){
				stringtype+="Ability";
			} else{
				stringtype+="Skill";
			}
			if (characters.Count > 1) {
				detail = string.Format (Database.strings.GetString (stringtype), Database.strings.GetString ("Plural3rd")) + "\n\n";
			}else {
				detail = string.Format (Database.strings.GetString (stringtype), Database.strings.GetString (characters [0].gender + "3rd")) + "\n\n";
			}
			if (ability!=null){
				detail += string.Format (Database.strings.GetString ("LearnAbility"),ability.name)+"\n\n";
			}
}
		if (type == "Socialize") {
			if (characters.Count > 1) {
				detail = string.Format (Database.strings.GetString (type + "Success"), Database.strings.GetString ("Plural3rd")) + "\n\n";
			} else {
				detail = string.Format (Database.strings.GetString (type + "Success"), Database.strings.GetString (characters [0].gender + "3rd")) + "\n\n";
			}
			if (newRecruits.Count > 0) {
				string recruit = "";
				for (int i=0; i<newRecruits.Count; i++) {
					if (i != 0) {
						if (i + 1 == newRecruits.Count) {
							recruit += " " + Database.strings.GetString ("And") + " ";
						} else {
							recruit += ", ";
						}
					}
					recruit += newRecruits [i];
				}
				detail += string.Format (Database.strings.GetString ("RecruitFound"), recruit) + "\n\n";
			}
			if (questCount > 0) {
				detail += string.Format (Database.strings.GetString ("QuestFound"), questCount) + "\n\n";
			}
		}
		if (type=="Sell"){
			string textSuccess="Fail";
			if (success){
				textSuccess="Success";
			}
			if (characters.Count > 1) {
				detail = string.Format (Database.strings.GetString (type + textSuccess), Database.strings.GetString ("Plural3rd")) + "\n\n";
			} else {
				detail = string.Format (Database.strings.GetString (type + textSuccess), Database.strings.GetString (characters [0].gender + "3rd")) + "\n\n";
			}
			if (itemsSold.Count()>0){
				detail += string.Format (Database.strings.GetString ("Sold"), ItemsSold ())+"\n\n";
				detail += string.Format (Database.strings.GetString ("Profit"), returnMoney.ToString())+"\n\n";
			}
		}
		detail += CharacterExp ();
		return detail;
	}

	public void GiveExp (Character character, string skillname, int exp)
	{
		character.GiveExp (Database.skills.GetSkill (skillname), exp);
		if (skillname == null) {
			skillname = "General";
		}
		if (!playerExpGain.ContainsKey (character.name)) {
			playerExpGain [character.name] = new Dictionary<string, int> ();
		}
		if (!playerExpGain [character.name].ContainsKey (skillname)) {
			playerExpGain [character.name] [skillname] = exp;
		} else {
			playerExpGain [character.name] [skillname] += exp;
		}

	}

	private string CharacterExp ()
	{
		string text = "";
		if (playerExpGain.Count > 0) {
			foreach (KeyValuePair<string,Dictionary<string,int>> character in playerExpGain) {
				if (character.Value.ContainsKey ("General")) {
					text += string.Format (Database.strings.GetString ("GainedExp"), character.Key, character.Value ["General"],"")+"\n\n";
				}
				foreach (KeyValuePair<string,int> gainedexp in character.Value) {
					if (gainedexp.Key != "General") {
						text += string.Format (Database.strings.GetString ("GainedExp"), character.Key, gainedexp.Value, gainedexp.Key) + "\n\n";
					}
				}
			}
		}
		return text;
	}

	private string Itemlist ()
	{
		string list = "";
		Dictionary<int,int> usedItemList;
			usedItemList=itemList;
		foreach (KeyValuePair<int,int> item in usedItemList) {
			list += item.Value.ToString () + " " + Database.items.FindItem (item.Key).name+"\n";
		}
		return list;
	}

	private string ItemsSold(){
		string list="";
		foreach(InventorySlot slot in itemsSold.GetAllItems()){
			Item item=Database.items.FindItem(slot.itemId);
			list+=slot.quantity.ToString()+" "+item.name;
			if (slot.durability>0){
				list+="("+slot.durability.ToString()+")";
			}
			list+="\n\n";
		}
		return list;
	}

	private int TotalProfit(){
		returnMoney=0;
		foreach (InventorySlot slot in itemsSold.GetAllItems()){
			Item item=Database.items.FindItem(slot.itemId);
			int value=item.sellValue*slot.durability/item.durability;
			returnMoney+=value*slot.quantity;
		}
		return returnMoney;
	}

	private void SellTime(){
		
		int charactercount=characters.Count;
		int socialskillId=Database.skills.GetSkill ("Social Skill");
		for (int time=0; time<=12; time++) {
			int RNG = Random.Range(0,100);
			for (int i=0; i<charactercount;i++){
				if (RNG<35+ExtensionMethods.Calculate(characters[i].skillLevel [socialskillId],0.5f)){
					success=true;
					GiveExp(characters[i],"Social",5);
					List<int> list=stock.GetAllFilledSlotId();
					InventorySlot itemToSell=stock.GetInventorySlot(list[Random.Range(0,list.Count)]);
					itemsSold.AddItem(itemToSell.itemId,1,itemToSell.durability);
					itemToSell.AddQuantity(-1);
					if (stock.Count()==0){
						return;
					}
				}
				GiveExp(characters[i],"Social",1);
			}
		}

	}

	private void AdventureTime (bool travelling)
	{//Start your adventure!
		string action = "Idle";
		GatheringPoint gatheringPoint = null;
		int inventorySpace = characters.Count * 5;
		int fieldSkillLevel = 0;
		int combatLevel = 0;
		int gatheringCount = 0;
		Dictionary<Monster,int> monsters = Database.monsters.GetAreaMonsters (area.name);
		List<Character> battleMonster = new List<Character> ();
		int averageLevel = 0;
		List<Character> livingMonsters;
		List<Character> livingCharacters;
		foreach (Character character in characters) {
			averageLevel += character.level;
			if (character.skillLevel [Database.skills.GetSkill ("Field Skill")] > fieldSkillLevel) {
				fieldSkillLevel = character.skillLevel [Database.skills.GetSkill ("Field Skill")];
			}
			if (character.skillLevel [Database.skills.GetSkill ("Combat Skill")] < combatLevel) {
				combatLevel = character.skillLevel [Database.skills.GetSkill ("Combat Skill")];
			}
		}
		averageLevel /= characters.Count;
		if (averageLevel < area.level) {
			averageLevel = area.level;

		}
		if (!travelling) {
			for (int time=0; time<=48; time++) {
				livingCharacters = characters.Alive ();
				livingMonsters = battleMonster.Alive ();
				if (action == "Idle") {
					if (time >= 36 || characters.IsInjured () || (action == "Gathering" && gatheringPointsFound == area.maxGatheringPoints)) {
						Debug.Log("Returning to town"+time);
						return;
					}

					if (characters.IsHurt ()) {
						characters.Rest ();
						Debug.Log ("Taking a rest " + time);
						//Move
					} else if (Random.Range (0, 101) < 6 + Mathf.RoundToInt (fieldSkillLevel / 2) + Database.myGuild.foundGatheringPoints [area.id]) { //Found a gathering point!
						Debug.Log ("Gathering Point Found " + time.ToString ());
						if (typeSearch == "Gathering" || Random.Range (0, 100) < 15) {
							gatheringPoint = area.FindRandomGatheringPoint ();
							if (gatheringPoint != null) {
								gatheringCount = Random.Range (1, gatheringPoint.maxQuantity);
								action = "Gathering";
							}
						}
						if (Random.Range (0, 100) < 100 - ((Database.myGuild.foundGatheringPoints [area.id] + gatheringPointsFound) / area.maxGatheringPoints * 100)) {
							gatheringPointsFound += 1;
							Debug.Log ("It's a new gathering point " + time);
						} else if (Random.Range (0, 100) < (gatheringPointsFound / area.maxGatheringPoints * 100)) {
							action = "Idle";
							Debug.Log ("This gathering point was already discovered today " + time);
						}
					} else if (Random.Range (0, 101) < 6) { //monster(s) encounter
						action = "Battle";
						bool generateMonster = true;
						battleMonster.Clear ();
						while (generateMonster) {
							int RNG = Random.Range (0, 101);
							battleMonster.Add (monsters.GenerateCharacter (RNG, averageLevel));
							if (Random.Range (0, 100) > battleMonster.Count * 30) {
								generateMonster = true;
							} else {
								generateMonster = false;
							}
						}
						monstercount += battleMonster.Count;
						livingMonsters = battleMonster;
						Debug.Log ("Monster Encounter! " + battleMonster.Count.ToString () + " monsters " + time);
						if (Random.Range (0, 101) < 15 + Mathf.FloorToInt (combatLevel / 2)) {//ambushed or not?
							Debug.Log ("Pre-emptive battle" + time);
							livingCharacters.TurnStart (this, livingMonsters, time);
						} else if (Random.Range (0, 101) < 15 - Mathf.FloorToInt (combatLevel / 2)) {
							Debug.Log ("Ambushed!" + time);
							livingMonsters.TurnStart (this, livingCharacters, time);
						}
						if (livingMonsters.Alive ().Count == 0) {
							action = "Idle";
						}
					} else {
						Debug.Log ("Looking " + time);
					}
				} else if (action == "Gathering") {
					foreach (Character character in livingCharacters) {
						if (Random.Range (0, 101) < 50 + character.skillLevel[Database.skills.GetSkill ("Field Skill")]) {
							int maxAmount = 1 + Mathf.FloorToInt (character.skillLevel[Database.skills.GetSkill ("Field Skill")] / 10);
							int gathered = 1;
							if (maxAmount > 1) {
								gathered = Random.Range (1, maxAmount);
							}

							if (gathered > inventorySpace) {
								gathered = inventorySpace;
							} 
							if (gathered > gatheringCount) {
								gathered = gatheringCount;
							}
							inventorySpace -= gathered;
							int type = gatheringPoint.FindRandomItem ().id;
							if (itemList.ContainsKey (type)) {
								itemList [type] += gathered;
							} else {
								itemList [type] = gathered;
							}
							gatheringCount -= gathered;
							if (inventorySpace == 0) {
								return;
							}
							if (gatheringCount == 0) {
								Debug.Log ("Finished Gathering " + time);
								break;
							}
							Debug.Log (character.name + " gathered " + gathered + " " + Database.items.FindItem (type).name + " " + time);
						} else {
							Debug.Log (character.name + " failed to gather anything " + time);
						}
					}
					if (time >= 36) {
						return;
					}
					if ((typeSearch == "Gathering" || Random.Range (0, 100) < 20) && gatheringCount != 0) {
						action = "Gathering";
					} else {
						action = "Idle";
					}
				} else if (action == "Battle") {
					livingCharacters.TurnStart (this, livingMonsters, time);
					if (livingMonsters.Alive ().Count > 0 && livingCharacters.Alive ().Count > 0) {
						livingMonsters.TurnStart (this, livingCharacters, time);
					}
					if (livingMonsters.Alive ().Count == 0) {
						Debug.Log ("Battle is won! " + time.ToString ());
						action = "Idle";
					}
					if (livingCharacters.Alive ().Count == 0) {
						Debug.Log ("Party is wiped out");
						success = false;
						return;
					}
				}
				if (typeSearch == "Gathering" && inventorySpace == 0) {
					Debug.Log ("Inventory is full, returning to town" + time);
					return;
				}
			}
		}
	}
}
