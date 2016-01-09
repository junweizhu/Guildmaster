using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Task
{

	public string type;
	public float duration;
	public int questid;
	public int questnumber;
	[System.NonSerialized]
	public List<Member>
		members;
	public Dictionary<int,int>itemList;
	public string typeSearch;
	public Guild guild;
	[System.NonSerialized]
	public int
		shoppingMoney;
	private int returnMoney;
	private StringDatabase sdb;
	public bool success;
	private Area area;
	private int gatheringPointsFound = 0;
	private int huntingGroundsFound = 0;
	private Area newArea;

	public Task ()
	{

	}

	public Task (Guild guild, string type, float duration, List<Member> members, int questid, int questnumber)
	{
		this.guild = guild;
		this.type = type;
		this.duration = duration;
		this.members = members;
		this.questid = questid;
		this.questnumber = questnumber;
	}

	public Task (Guild guild, string type, float duration, List<Member> members, Dictionary<int,int>items, int money)
	{
		this.guild = guild;
		this.type = type;
		this.duration = duration;
		this.members = members;
		itemList = items;
		shoppingMoney = money;
	}

	public Task (Guild guild, string type, float duration, List<Member> members, string typesearch)
	{
		this.guild = guild;
		this.type = type;
		this.duration = duration;
		this.members = members;
		this.typeSearch = typesearch;
	}

	public Task (Guild guild, string type, Area area, List<Member> members, string typesearch)
	{
		this.guild = guild;
		this.type = type;
		this.area = area;
		this.duration = area.travelTime + 1;
		this.members = members;
		this.typeSearch = typesearch;
		itemList = new Dictionary<int, int> ();
	}

	public void UpdateTask ()
	{
		duration -= 1;

		if (duration <= 0) {
			if (type == "Shop") {
				float payment = 1.25f;
				for (int i=0; i< members.Count; i++) {
					members [i].status = "Idle";
					members [i].GiveExp (members [i].FindSkill ("Social Skill"), 10);
					if (Random.Range (0, 100) > (100 - (members [i].FindSkillLevel ("Social Skill") / (i + 1)))) {
						payment *= 1.0f - (0.5f / (2 + i));
					}
				}
				returnMoney = Mathf.CeilToInt (shoppingMoney * (1 - payment));
				guild.FinishShopping (itemList, returnMoney);
				return;
			}
			if (type == "Quest") {
				guild.FinishQuest (questnumber, members);
				for (int i=0; i< members.Count; i++) {
					members [i].status = "Idle";
				}
			}
			if (type == "SearchQuest") {
				int rank = GameObject.FindObjectOfType<QuestDatabase> ().rankLevel [typeSearch];
				for (int i=0; i< members.Count; i++) {
					if (Random.Range (0, 5 + members [i].level * 2) > rank && success == false) {
						guild.FindNewQuest (Random.Range (0, members [i].level + 2));
						success = true;
					}
					members [i].status = "Idle";
					members [i].GiveExp (members [i].FindSkill ("Social Skill"), 10);
				}
			}
			if (type == "SearchRecruit") {
				Dictionary<string,string> typeSkill = new Dictionary<string, string> (){{"Fighter","Weapon Skill"},{"Mage","Magic Skill"},{"Adventurer","Field Skill"},{"Social","Social Skill"}};

				for (int i=0; i< members.Count; i++) {
					if (!typeSkill.ContainsKey (typeSearch)) {
						List<string> skills = new List<string> (){"Fighter","Mage","Adventurer","Social"};
						typeSearch = skills [Random.Range (0, skills.Count)];
					}
					float chance = members [i].FindSkillLevel ("Social Skill") + members [i].FindSkillLevel (typeSkill [typeSearch]) / 2;
					if (Random.Range (0, 100) > 85 - chance && success == false) {
						guild.FindNewRecruit (Random.Range (0, members [i].level + Random.Range (0, 10) - 5), typeSkill [typeSearch]);
						success = true;
					}
					members [i].status = "Idle";
					members [i].GiveExp (members [i].FindSkill ("Social Skill"), 10);
				}
			}
			if (type == "Adventure") {
				AdventureTime (false);
				if (success){
					guild.GetItems (itemList);
				}
				for (int i=0; i< members.Count; i++) {
					members [i].status = "Idle";
				}
			}
		}
	}

	public string Details ()
	{
		sdb = GameObject.FindObjectOfType<StringDatabase> ();
		string detail = sdb.GetString (type + "Log1");
		for (int i=0; i<members.Count; i++) {
			if (i != 0) {
				if (i + 1 == members.Count) {
					detail += " " + sdb.GetString ("And") + " ";
				} else {
					detail += ", ";
				}
			}
			detail += members [i].name;
		}
		detail += " " + sdb.GetString (type + "Log2");

		if (type == "Shop") {
			detail += " " + Itemlist ();
		} else if (type == "Quest") {
			detail += " \"" + GameObject.FindObjectOfType<QuestDatabase> ().FindQuest (questid).name + "\"";
		} else if (type == "SearchQuest" || type == "SearchRecruit") {
			if (success)
				detail += " " + sdb.GetString (type + "Success");
			else
				detail += " " + sdb.GetString (type + "Fail");
		} else if (type == "Adventure") {
			detail += " " + sdb.GetString (typeSearch + "Log");
		}

		detail += " " + sdb.GetString (type + "Log3");
		if (type == "Adventure") {
			if (members.Count > 1) {
				detail += sdb.GetString ("Plural3rd");
			} else {
				detail += sdb.GetString (members [0].gender + "3rd");
			}
			if (success) {
				detail += " " + sdb.GetString (typeSearch + "LogSuccess");
				if (typeSearch == "Gathering") {
					detail += " " + Itemlist () + ".";
				}
			} else{
				detail += " " + sdb.GetString (typeSearch + "LogFail");
			}
		}

		return detail;
	}

	private string Itemlist ()
	{
		string list = "";
		int itemcount = 0;
		foreach (KeyValuePair<int,int> item in itemList) {
			
			if (itemcount != 0) {
				if (itemcount + 1 == itemList.Count) {
					list += sdb.GetString ("And");
				} else {
					list += ", ";
				}
			}
			list += item.Value.ToString () + " " + GameObject.FindObjectOfType<ItemDatabase> ().FindItem (item.Key).name;
			if (item.Value > 1) {
				list += sdb.GetString ("pluralletter");
			}
			
		}
		return list;
	}

	private void AdventureTime (bool travelling)
	{//Start your adventure!
		string action = "Idle";
		GatheringPoint gatheringPoint = null;
		int inventorySpace = 0;
		int fieldSkillLevel = 0;
		int combatLevel = 0;
		int gatheringCount = 0;
		Dictionary<Monster,int> monsters = GameObject.FindObjectOfType<MonsterDatabase> ().GetAreaMonsters (area.name);
		List<Monster> battleMonster = new List<Monster> ();
		int averageLevel = 0;
		List<Monster> livingMonsters;
		List<Member> livingMembers;
		foreach (Member member in members) {
			inventorySpace += 10 - member.GetInventory().Count;
			averageLevel += member.level;
			if (member.FindSkillLevel ("Field Skill") > fieldSkillLevel) {
				fieldSkillLevel = member.FindSkillLevel ("Field Skill");
			}
			if (member.FindSkillLevel ("Combat Skill") < combatLevel) {
				combatLevel = member.FindSkillLevel ("Combat Skill");
			}
		}
		averageLevel /= members.Count;
		if (averageLevel < area.level) {
			averageLevel = area.level;

		}
		if (!travelling) {
			for (int time=0; time<=48; time++) {
				livingMembers=members.Alive();
				livingMonsters=battleMonster.Alive();
				if (action == "Idle") {
					if (time>=36){
						return;
					}
					//Move
					if (Random.Range (0, 101) < 6 + Mathf.RoundToInt (fieldSkillLevel / 2) + guild.foundGatheringPoints [area]) { //Found a gathering point!
						Debug.Log ("Gathering Point Found "+time.ToString());
						if (typeSearch == "Gathering" || Random.Range (0, 100) < 20) {
							gatheringPoint = area.FindRandomGatheringPoint ();
							if (gatheringPoint != null) {
								gatheringCount = Random.Range (1, gatheringPoint.maxQuantity);
								action = "Gathering";
							}
						}
						if (Random.Range (0, 100) < 100 - ((guild.foundGatheringPoints [area] + gatheringPointsFound) / area.maxGatheringPoints * 100)) {
							gatheringPointsFound += 1;
							Debug.Log ("It's a new gathering point "+ time);
						} else if (Random.Range (0, 100) <(gatheringPointsFound / area.maxGatheringPoints * 100)){
							action="Idle";
							Debug.Log ("This gathering point was already discovered today "+ time);
						}
					} else if (Random.Range (0, 101) < 10) { //monster(s) encounter

						action = "Battle";
						bool generateMonster = true;
						battleMonster.Clear();
						while (generateMonster) {
							int RNG = Random.Range (0, 101);
							battleMonster.Add (monsters.GenerateMonster (RNG, averageLevel));
							if (Random.Range (0, 100) > battleMonster.Count * 30) {
								generateMonster = true;
							} else {
								generateMonster = false;
							}
						}
						livingMonsters=battleMonster;
						Debug.Log ("Monster Encounter! " + battleMonster.Count.ToString () + " monsters " + time);
						if (Random.Range (0, 101) > 20 - Mathf.FloorToInt (combatLevel / 2)) {//ambushed or not?
							livingMembers.TurnStart (livingMonsters,time);
						} else{
							Debug.Log ("Ambushed!"+ time);
						}
						if (livingMonsters.Alive().Count > 0&&livingMembers.Alive ().Count>0) {
							livingMonsters.TurnStart (livingMembers,time);
						} 
						if(livingMonsters.Alive().Count==0){
							action = "Idle";
						}
					} else{
						Debug.Log("Looking "+time);
					}
				} else if (action == "Gathering") {
					foreach (Member member in livingMembers) {
						if (Random.Range (0, 101) < 50 + member.FindSkillLevel ("Field Skill")) {
							int maxAmount = 1 + Mathf.FloorToInt (member.FindSkillLevel ("Field Skill") / 10);
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
							success = true;
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
								Debug.Log ("Finished Gathering "+ time);
								break;
							}
							Debug.Log(member.name+ " gathered " +gathered+" "+GameObject.FindObjectOfType<ItemDatabase>().FindItem(type).name +" "+ time);
						}else{
							Debug.Log(member.name+ " failed to gather anything "+ time);
						}
					}
					if (time>=36){
						return;
					}
					if ((typeSearch == "Gathering" || Random.Range (0, 100) < 20) && gatheringCount != 0) {
						action = "Gathering";
					} else {
						action = "Idle";
					}
				} else if (action == "Battle") {
					livingMembers.TurnStart (livingMonsters,time);
					if (livingMonsters.Alive().Count > 0&&livingMembers.Alive ().Count>0) {
						livingMonsters.TurnStart (livingMembers,time);
					}
					if(livingMonsters.Alive().Count==0){
						Debug.Log("Battle is won! "+time.ToString());
						action = "Idle";
					}
					if (livingMembers.Alive ().Count==0){
						Debug.Log("Party is wiped out");
						success=false;
						return;
					}
				}
				if (typeSearch=="Gathering" && inventorySpace == 0) {
					Debug.Log("Inventory is full, returning to town"+ time);
					return;
				}
			}
		}
	}
}
