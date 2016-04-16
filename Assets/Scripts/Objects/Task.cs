using UnityEngine;
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
	public Dictionary<int,int> itemList;
	public Inventory itemsSold;
	public Inventory stock;
	public string typeSearch;
	public int shoppingMoney;
	private int returnMoney;
	public bool success = false;
	public int guildExp;
	public Area area;
	public int gatheringPointsFound = 0;
	private Area newArea;
	private Dictionary<Character,Dictionary<int,int>> characterExpGain = new Dictionary<Character,Dictionary<int,int>> ();
	public int monstercount = 0;
	private List<string> newRecruits = new List<string> ();
	private int questCount = 0;
	private Quest quest;
	public List<string> casualties = new List<string> ();
	private Skill skill;
	private Ability ability;
	public bool canBeCanceled = true;
	public int weaponSkillId = 0;
	public int magicSkillId = 1;
	public int combatSkillId = 2;
	public int fieldSkillId = 3;
	public int socialSkillId = 4;
	public int newStepsCount = 0;
	public List<int> foundNewArea = new List<int> ();
	public bool manual = false;
	public bool finished;
	public Adventure adventure;


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
		if (type == "Shop") {
			itemList = items;
		} else {
			Inventory inventory = Database.myGuild.inventory;
			stock = new Inventory (items.Count);
			foreach (KeyValuePair<int,int> itemslot in items) {
				InventorySlot slot = inventory.GetInventorySlot (itemslot.Key);
				stock.AddItem (slot.itemId, itemslot.Value, slot.durability);
				slot.AddQuantity (0 - itemslot.Value);
			}
			itemsSold = new Inventory (items.Count);
		}
		shoppingMoney = money;
	}

	public Task (string type, float duration, List<Character> characters, Skill skill, Ability ability, int money)//training
	{ 
		SetMainData (type, characters);
		this.duration = duration;
		this.skill = skill;
		this.ability = ability;
		shoppingMoney = money;
	}

	public Task (string type, float duration, List<Character> characters, string typesearch)//searching at tavern
	{
		SetMainData (type, characters);
		this.duration = duration;
		this.typeSearch = typesearch;
	}

	public Task (string type, Area area, List<Character> characters, string typesearch)//adventuring
	{
		SetMainData (type, characters);
		this.area = area;
		this.duration = area.travelTime + 1;
		this.typeSearch = typesearch;
		itemList = new Dictionary<int, int> ();
		success = true;
		for (int i = 0; i < characters.Count; i++) {
			characters [i].CreateTempEquipment ();
			if (characters [i].id == 0 || characters [i].id == 1) {
				manual = true;
			}
		}
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
		canBeCanceled = false;
		if (type == "Quest") {
			quest.duration -= 1;
		}
		if (duration <= 0) {
			if (manual && finished || !manual) {
				guildExp = 0;
				if (type == "Quest") {
					//Database.myGuild.FinishQuest (questnumber, characters,false);
					if (quest.expReward != null) {
						foreach (KeyValuePair<int,int> expGain in quest.expReward) {
							for (int i = 0; i < characters.Count; i++) {
								GiveExp (characters [i], expGain.Key, expGain.Value);
							}
						}
					}
					guildExp = quest.guildExpReward;
					if (quest.itemRewards != null) {
						itemList = quest.itemRewards;
						Database.myGuild.GetItems (itemList);
					}
					quest.finished = true;
				}
				if (type == "Shop") {
					float payment = 1.00f;
					for (int i = 0; i < characters.Count; i++) {
						GiveExp (characters [i], socialSkillId, 10);
						if (Random.Range (0, 100) > (100 - (characters [i].skillLevel [socialSkillId] / (i + 1)))) {
							payment *= 1.0f - (0.5f / (2 + i));
						}
					}
					returnMoney = Mathf.CeilToInt (shoppingMoney * (1 - payment));
					Database.myGuild.FinishShopping (itemList, returnMoney);
					return;
				}
				if (type == "Sell") {
					if (stock.Count () > 0) {
						Inventory inventory = Database.myGuild.inventory;
						foreach (int slot in stock.GetAllFilledSlotId()) {
							InventorySlot itemslot = stock.GetInventorySlot (slot);
							inventory.AddItem (itemslot.itemId, itemslot.quantity, itemslot.durability);
							itemslot.EmptyItem ();
						}
					}
					Database.myGuild.money += TotalProfit ();
				}
				if (type == "School") {
					foreach (Character character in characters) {
						if (ability != null) {
							character.abilities.Add (ability.id);
						}
						if (skill != null) {
							GiveExp (character, skill.id, 100);
						}
					}
				}
				if (type == "Socialize") {
					Dictionary<string,int> typeSkill = new Dictionary<string,int> () {
						{ "Fighter",weaponSkillId },
						{ "Mage",magicSkillId }, {
							"Adventurer",
							fieldSkillId
						}, {
							"Social",
							socialSkillId
						}
					};
				
					for (int i = 0; i < characters.Count; i++) {
						int fame = (Random.Range (-2, 2) + Random.Range (-2, 2)) / 2;
						List<string> skills = new List<string> (){ "Fighter", "Mage", "Adventurer", "Social" };
						typeSearch = skills [Random.Range (0, skills.Count)];
						float chance = characters [i].skillLevel [socialSkillId] + characters [i].skillLevel [typeSkill [typeSearch]] / 2;
						if (Random.Range (0, 100) > 95 - chance || (!Database.events.GetTrigger (105).activated && Random.Range (0, 100) > 50 - chance && Database.characters.GetRecruitables ().Count + Database.myGuild.characterlist.Count < 5)) {
							newRecruits.Add (Database.myGuild.FindNewRecruit (Random.Range (0, characters [i].level + Random.Range (0, 10) - 5), typeSkill [typeSearch]).name);
							GiveExp (characters [i], socialSkillId, 10);
							fame = 2;
						} else if (Random.Range (0, 100) > 90 - chance && Database.events.GetTrigger (110).activated || Database.events.GetTrigger (109).activated && !Database.events.GetTrigger (110).activated && Database.quests.AvailableQuests ().Count < 1 && Database.myGuild.questBoard.questList.Count < 1) {
							Debug.Log ("They found a quest");
							questCount += 1;
							int rng = Random.Range (0, 10) - 5;
							if (rng < 1) {
								rng = 1;
							}
							Database.quests.GenerateQuest ((Random.Range (0, characters [i].level + rng)));
							GiveExp (characters [i], socialSkillId, 10);
							fame = 2;
						} else if (Random.Range (0, 100) > 95 - chance && Database.events.GetTrigger (107).activated) {
							Debug.Log ("They talked about a new area");
							int rng = Database.areas.RandomArea (characters [i].level);
							if (Database.myGuild.knownAreas.Contains (rng)) {
								Debug.Log ("We already know this area");
							} else {
								Debug.Log ("We never knew this area");
								Database.myGuild.FindNewArea (rng);
								foundNewArea.Add (rng);
								GiveExp (characters [i], socialSkillId, 10);
							}
						}
						GiveExp (characters [i], socialSkillId, Random.Range (0, 15));
						characters [i].baseStats ["Fame"] += fame;
						Database.myGuild.fame += fame;
					}
				}
				if (type == "Adventure") {
				
					if (success && itemList.Count > 0) {
						Database.myGuild.GetItems (itemList);
					} 
					for (int i = 0; i < characters.Count; i++) {
						characters [i].EquipmentAfterAdventure ();
						if (characters [i].totalStats ["CurrentHealth"] <= 0) {
							casualties.Add (characters [i].name);
						}
					}
				}
				for (int i = 0; i < characters.Count; i++) {
					characters [i].status = "Idle";
					characters [i].statusAdd = "";
				}
				DistributeExp ();
				Database.myGuild.GiveExp (guildExp);
				if (foundNewArea.Count > 0) {
					for (int i = 0; i < foundNewArea.Count; i++) {
						Database.myGuild.FindNewArea (foundNewArea [i]);
					}
				}
			} else {
				Database.game.manualTasks.Add (this);
			}
		}
	}

	public void Cancel ()
	{
		if (type == "Quest") {
			quest.Reset ();
		}
		if (shoppingMoney > 0) {
			Database.myGuild.money += shoppingMoney;
		}
		if (stock != null) {
			ReturnItems (stock);
		}
		if (itemsSold != null) {
			ReturnItems (itemsSold);
		}
		foreach (Character character in characters) {
			character.status = "Idle";
			character.statusAdd = "";
		}

		if (Database.game.tasksWithCoroutine.Contains (this)) {
			Database.game.tasksWithCoroutine.Remove (this);
		}
	}

	private void ReturnItems (Inventory items)
	{
		List<int> slots = items.GetAllFilledSlotId ();
		for (int i = 0; i < slots.Count; i++) {
			InventorySlot itemslot = items.GetInventorySlot (slots [i]);
			Database.myGuild.inventory.AddItem (itemslot.itemId, itemslot.quantity, itemslot.durability);
			itemslot.EmptyItem ();
		}

	}

	public string ShortDescription ()
	{
		string names = "";
		for (int i = 0; i < characters.Count; i++) {
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
		if (type == "Quest") {
			detail = string.Format (Database.strings.GetString (type + "Success"), quest.name) + "\n\n";
			if (itemList != null) {
				detail += string.Format (Database.strings.GetString ("QuestItemReward"), Itemlist () + "\n");
			}
			if (quest.moneyReward > 0) {
				detail += string.Format (Database.strings.GetString ("QuestMoneyReward"), quest.moneyReward);
			}
		}
		if (type == "Adventure") {
			if (success) {
				detail = Database.strings.GetString (type + "Success") + "\n\n";
				detail += string.Format (Database.strings.GetString ("ExploreLog"), Mathf.RoundToInt ((float)100 * newStepsCount / area.size).ToString ()) + "\n\n";
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
			if (foundNewArea.Count > 0) {
				for (int i = 0; i < foundNewArea.Count; i++) {
					detail += string.Format (Database.strings.GetString ("ExploreAreaFound"), Database.areas.FindArea (foundNewArea [i]).name) + "\n\n";
				}
			}
			if (casualties.Count > 0) {
				for (int i = 0; i < casualties.Count; i++) {
					detail += string.Format (Database.strings.GetString ("Injured"), casualties [i]) + "\n";
				}
				detail += "\n";
			}
		}
		if (type == "School") {
			string stringtype = type;
			if (ability != null) {
				stringtype += "Ability";
			} else {
				stringtype += "Skill";
			}
			if (characters.Count > 1) {
				detail = string.Format (Database.strings.GetString (stringtype), Database.strings.GetString ("Plural3rd")) + "\n\n";
			} else {
				detail = string.Format (Database.strings.GetString (stringtype), Database.strings.GetString (characters [0].gender + "3rd")) + "\n\n";
			}
			if (ability != null) {
				detail += string.Format (Database.strings.GetString ("LearnAbility"), ability.name) + "\n\n";
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
				for (int i = 0; i < newRecruits.Count; i++) {
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
			if (foundNewArea.Count > 0) {
				for (int i = 0; i < foundNewArea.Count; i++) {
					detail += string.Format (Database.strings.GetString ("AreaFound"), Database.areas.FindArea (foundNewArea [i]).name) + "\n\n";
				}
			}
		}
		if (type == "Sell") {
			string textSuccess = "Fail";
			if (success) {
				textSuccess = "Success";
			}
			if (characters.Count > 1) {
				detail = string.Format (Database.strings.GetString (type + textSuccess), Database.strings.GetString ("Plural3rd")) + "\n\n";
			} else {
				detail = string.Format (Database.strings.GetString (type + textSuccess), Database.strings.GetString (characters [0].gender + "3rd")) + "\n\n";
			}
			if (itemsSold.Count () > 0) {
				detail += string.Format (Database.strings.GetString ("Sold"), ItemsSold ()) + "\n\n";
				detail += string.Format (Database.strings.GetString ("Profit"), returnMoney.ToString ()) + "\n\n";
			}
		}
		detail += CharacterExp ();
		if (guildExp > 0) {
			detail += string.Format (Database.strings.GetString ("GainedExp"), Database.strings.GetString ("Guild"), guildExp, "") + "\n\n";
		}
		return detail;
	}

	public void GiveExp (Character character, int skillId, int exp)
	{
		if (!characterExpGain.ContainsKey (character)) {
			characterExpGain [character] = new Dictionary<int, int> ();
		}
		if (!characterExpGain [character].ContainsKey (skillId)) {
			characterExpGain [character] [skillId] = exp;
		} else {
			characterExpGain [character] [skillId] += exp;
		}
		if (skillId == 99) {
			adventure.actionList.Add(character.name +" gained "+ exp+" exp.");
		}
	}

	public void DistributeExp ()
	{
		for (int i = 0; i < characters.Count; i++) {
			if (characterExpGain.ContainsKey (characters [i])) {
				foreach (KeyValuePair<int,int> expGain in characterExpGain[characters[i]]) {
					characters [i].GiveExp (expGain.Key, expGain.Value);
				}
			}
		}
	}

	private string CharacterExp ()
	{
		string text = "";
		if (characterExpGain.Count > 0) {
			foreach (KeyValuePair<Character,Dictionary<int,int>> character in characterExpGain) {
				text += character.Key.name + "\n" + Database.strings.GetString ("Health") + ": " + character.Key.totalStats ["CurrentHealth"] + "/" + character.Key.totalStats ["MaxHealth"] + "\n" + Database.strings.GetString ("Mana") + ": " + character.Key.totalStats ["CurrentMana"] + "/" + character.Key.totalStats ["MaxMana"] + "\n";
				if (character.Value.ContainsKey (99)) {
					text += string.Format (Database.strings.GetString ("LogGainedExp"), character.Value [99], "") + "\n";
				}
				foreach (KeyValuePair<int,int> gainedexp in character.Value) {
					if (gainedexp.Key != 99) {
						text += string.Format (Database.strings.GetString ("LogGainedExp"), gainedexp.Value.ToString (), Database.skills.GetSkill (gainedexp.Key).name) + "\n";
					}
				}
				text += "\n";
			}
		}
		return text;
	}

	private string Itemlist ()
	{
		string list = "";
		Dictionary<int,int> usedItemList;
		usedItemList = itemList;
		foreach (KeyValuePair<int,int> item in usedItemList) {
			list += item.Value.ToString () + " " + Database.items.FindItem (item.Key).name + "\n";
		}
		return list;
	}

	private string ItemsSold ()
	{
		string list = "";
		List<InventorySlot> items = itemsSold.GetAllItems ();
		for (int i = 0; i < items.Count; i++) {
			InventorySlot slot = items [i];
			Item item = Database.items.FindItem (slot.itemId);
			list += slot.quantity.ToString () + " " + item.name;
			if (slot.durability > 0) {
				list += "(" + slot.durability.ToString () + ")";
			}
			list += "\n\n";
		}
		return list;
	}

	private int TotalProfit ()
	{
		returnMoney = 0;
		List<InventorySlot> items = itemsSold.GetAllItems ();
		for (int i = 0; i < items.Count; i++) {
			Item item = Database.items.FindItem (items [i].itemId);
			int value = item.sellValue * items [i].durability / item.durability;
			returnMoney += value * items [i].quantity;
		}
		return returnMoney;
	}

	private void SetTaskToCoroutineList ()
	{
		Database.game.tasksWithCoroutine.Add (this);
	}

	private void RemoveTaskFromCoroutineList ()
	{
		Database.game.tasksWithCoroutine.Remove (this);
	}

	public IEnumerator SellTime (WaitForSeconds waitTime)
	{
		SetTaskToCoroutineList ();
		int charactercount = characters.Count;
		for (int time = 0; time <= 12; time++) {
			if (Database.game.tasksWithCoroutine.Contains (this)) {
				yield return (waitTime);
			} else {
				yield break;
			}
			int RNG = Random.Range (0, 100);
			for (int i = 0; i < charactercount; i++) {
				if (RNG < 15 + ExtensionMethods.Calculate (characters [i].skillLevel [socialSkillId], 0.5f) || (!Database.events.GetTrigger (108).activated && !success)) {
					success = true;
					List<int> list = stock.GetAllFilledSlotId ();
					InventorySlot itemToSell = stock.GetInventorySlot (list [Random.Range (0, list.Count)]);
					int count = Mathf.RoundToInt (Random.Range (0, 500) / 500) * itemToSell.quantity + 1;
					itemsSold.AddItem (itemToSell.itemId, count, itemToSell.durability);
					itemToSell.AddQuantity (-count);
					GiveExp (characters [i], socialSkillId, count * 5);
					if (stock.Count () == 0) {
						RemoveTaskFromCoroutineList ();
						yield break;
					}
				}
				GiveExp (characters [i], socialSkillId, 1);
			}
		}
		RemoveTaskFromCoroutineList ();
	}

	public IEnumerator AdventureTime (WaitForSeconds waitTime)
	{//Start your adventure!
		SetTaskToCoroutineList ();
		adventure = new Adventure (characters, area, this);

		for (int time = 0; time <= 48; time++) {
			if (Database.game.tasksWithCoroutine.Contains (this)) {
				yield return (waitTime);
			} else {
				yield break;
			}
			if (adventure.action == "Idle") {//start doing something
				
				if (time >= 36 || characters.IsInjured () || (adventure.action == "Gathering" && gatheringPointsFound == area.maxGatheringPoints)) {
					adventure.actionList.Add ("Returning to town");
					ShowDebugLog (time);
					break;
				}
				if (characters.IsHurt ()) {//resting to heal
					adventure.Rest();
					//Move
				} else {
					adventure.Explore ();
					if (adventure.action == "Battle") {
						if (adventure.status == "Pre-emptive") {
							if (typeSearch == "Training" || Random.Range (0, 100) < 20) {
								TurnStart (adventure.livingCharacters, adventure.livingMonsters);
							} else {
								adventure.action = "Idle";
								adventure.actionList.Add ("Decided to ignore them and walk away");
							}
						} else if (adventure.status == "Ambush") {
							TurnStart (adventure.livingMonsters, adventure.livingCharacters);
						}
						adventure.CheckBattleIsFinished ();
					} else if (adventure.status == "Found GatheringSpot") {
						if (typeSearch == "Gathering" || Random.Range (0, 100) < 20) {
							adventure.actionList.Add ("Decided to gather");
							adventure.action = "Gathering";
						}
					}

				}
				adventure.actionList.Add ((100 * newStepsCount / area.size).ToString () + " completion");
			} else if (adventure.action == "Gathering") {
				adventure.Gather ();
				if ((typeSearch == "Gathering" || Random.Range (0, 100) < 20) && adventure.gatheringCount != 0) {
					adventure.action = "Gathering";
				} else {
					adventure.actionList.Add ("Spent enough time gathering here.");
					adventure.action = "Idle";
				}
				if (time >= 36) {
					ShowDebugLog (time);
					break;
				}
			} else if (adventure.action == "Battle") {
				adventure.RefreshBattleCharacterList ();
				TurnStart (adventure.livingCharacters, adventure.livingMonsters);
				if (adventure.livingMonsters.Alive ().Count > 0 && adventure.livingCharacters.Alive ().Count > 0) {
					TurnStart (adventure.livingMonsters, adventure.livingCharacters);
				}
				if (adventure.livingMonsters.Alive ().Count == 0) {
					adventure.actionList.Add ("Battle is won!");
					adventure.action = "Idle";
					guildExp += 2 + 3 * area.level;
					for (int i = 0; i < adventure.livingCharacters.Count; i++) {
						GiveExp (adventure.livingCharacters [i], combatSkillId, 5 * area.level);
					}
				}
				if (adventure.livingCharacters.Alive ().Count == 0) {
					adventure.actionList.Add ("Party is wiped out");
					success = false;
					ShowDebugLog (time);
					break;
				}
			}
			if (typeSearch == "Gathering" && (adventure.inventorySpace == 0 || adventure.action == "idle" && gatheringPointsFound == area.maxGatheringPoints)) {
				if (adventure.inventorySpace == 0) {
					adventure.actionList.Add ("Inventory is full, returning to town");
				} else if(adventure.action == "idle" && gatheringPointsFound == area.maxGatheringPoints){
					adventure.actionList.Add ("all gatheringpoints found, returning to town");
				}
				ShowDebugLog (time);
				break;
			}
			ShowDebugLog (time);
		}
		RemoveTaskFromCoroutineList ();
	}
	public void ShowDebugLog(int time){
		for (int i = 0; i < adventure.actionList.Count; i++) {
			Debug.Log (adventure.actionList [i] + " " + time+"-"+i);
		}
		adventure.actionList.Clear ();
	}
	public void TurnStart (List<Character> attackers, List<Character> defenders)
	{
		if (defenders != null) {
			if (attackers [0].isEnemy) {
				adventure.actionList.Add ("Enemy turn.");
			} else {
				adventure.actionList.Add ("Player turn");
			}
			string action="";
			Character defender=null;
			Ability attack=null;
			for (int i = 0; i < attackers.Count; i++) {
				adventure.PlayerTurn (attackers [i],attackers, defenders, ref action, ref defender, ref attack);
				if (action == "Fight") {
					Ability counter = defender.ChooseCounterAttack (attack.range, attackers [i]);
					adventure.Fight (attackers [i], attack, defender, counter);
					if (defenders.Alive ().Count == 0 || attackers.Alive ().Count == 0) {
						return;
					}
				}
				defenders = defenders.Alive ();
			}
		}
	}
		
}
