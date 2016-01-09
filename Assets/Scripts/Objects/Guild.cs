using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Guild
{
	public int id;
	public string name;
	public Image icon;
	public int level = 1;
	public int exp = 0;
	public int fame = 0;
	public int size = 0;
	public int money = 0;
	public Inventory inventory;
	public List<Member> memberlist = new List<Member> ();
	public QuestBoard questBoard;
	public List<Task> taskList = new List<Task> ();
	public Dictionary<int,List<Task>> taskLog = new Dictionary<int,List<Task>> ();
	public List<string> newmembers = new List<string> ();
	public List<Area> knownAreas=new List<Area>();
	public Dictionary<Area,int> foundGatheringPoints=new Dictionary<Area,int>();
	public Dictionary<Area,int> foundHuntingGrounds=new Dictionary<Area,int>();
	public Dictionary<Area,int> successfulVisits=new Dictionary<Area,int>();
	public StringDatabase sdb;

	public Guild ()
	{

	}

	public Guild (int id, string name, int level, int fame, int money, ItemDatabase idb, QuestDatabase qdb)
	{
		this.id = id;
		this.name = name;
		this.level = level;
		this.fame = fame;
		this.money = money;
		this.inventory = new Inventory (idb,this);
		this.questBoard = new QuestBoard (qdb);
		sdb=GameObject.FindObjectOfType<StringDatabase>();
	}

	public void RecruitMember (Member newmember)
	{
		memberlist.Add (newmember);
		newmember.recruited = true;
		newmember.guildnr = memberlist.Count;
		size = memberlist.Count;
	}

	public List<Member> GetAvailableMembers ()
	{
		List<Member> availablemembers = new List<Member> ();
		for (int i=0; i<memberlist.Count; i++) {
			if (memberlist [i].status == "Idle") {
				availablemembers.Add (memberlist [i]);
			}
		}
		return availablemembers;
	}

	public Member GetMember (int id)
	{
		foreach (Member member in memberlist) {
			if (member.guildnr == id) {
				return member;
			}
		}
		return null;
	}

	public void AddTask (string type, float duration, List<Member> members, int questid, int questnumber)
	{//for taking quests
		taskList.Add (new Task (this, type, duration, members, questid, questnumber));
	}

	public void AddTask (string type, float duration, List<Member> members, Dictionary<int,int> itemlist, int shopMoney)
	{//for shopping
		taskList.Add (new Task (this, type, duration, members, itemlist, shopMoney));
		money -= shopMoney;
	}

	public void AddTask (string type, float duration, List<Member> members, string typeRecruit)
	{//for searching quests and members
		taskList.Add (new Task (this, type, duration, members, typeRecruit));
	}

	public void AddTask (string type, Area area, List<Member> members, string typeRecruit)
	{//for Adventuring
		taskList.Add (new Task (this, type, area, members, typeRecruit));
	}

	public void UpdateTasks (int day)
	{
		if (taskList.Count > 0) {
			List<Task> finished = new List<Task> ();
			for (int i=0; i<taskList.Count; i++) {
				taskList [i].UpdateTask ();
				if (taskList [i].duration <= 0) {
					if (!taskLog.ContainsKey (day))
						taskLog [day] = new List<Task> ();
					taskLog [day].Add (taskList [i]);
					finished.Add (taskList [i]);
				}
			}
			if (finished.Count > 0) {
				for (int i=0; i<finished.Count; i++) {
					if (finished [i].type == "Quest") {
						questBoard.RemoveQuest (finished [i].questnumber);
					}
					taskList.Remove (finished [i]);
				}
			}
		}
	}
	public void UpdateMembers(){
		foreach(Member member in memberlist){
			if (member.stats["CurrentHealth"]>0||member.status=="Resting"){
				member.Heal(100,"Percent","Health");
				member.Heal(100,"Percent","Mana");
			} else if (member.status=="Idle"){
				member.stats["CurrentHealth"]=0;
				member.status="Resting";
			}
		}
	}
	public List<string> GetMemberActivity ()
	{
		List<string> guildinfo = new List<string> ();
		foreach (Member member in memberlist) {
			if (member.levelUp) {
				string info = member.name + " " + sdb.GetString ("LevelUp");
				int count = 0;
				foreach (KeyValuePair<string,int> stat in member.levelUpStats) {
					if (stat.Value != 0) {
						count++;
					}
				}
				foreach (KeyValuePair<string,int> stat in member.levelUpStats) {
					if (stat.Value != 0) {
						count--;
						info += " " + stat.Value.ToString () + " " + sdb.GetString (stat.Key);
						if (count == 1) {
							info += " " + sdb.GetString ("And");
						} else if (count != 0) {
							info += ",";
						} else {
							info += ".";
						}
					}
				}
				guildinfo.Add (info);
			}
			if (member.skillUp) {
				string info = member.name + " " + sdb.GetString ("SkillUp")+" "+ sdb.GetString (member.gender+"Poss");
				for (int i=0; i<member.leveledSkill.Count; i++) {
					info += " " + member.leveledSkill [i] + "s";
					if (i == member.leveledSkill.Count - 2) {
						info += " " + sdb.GetString ("And");
					} else if (i != member.leveledSkill.Count - 1) {
						info += ",";
					} else {
						info += ".";
					}
						
				}
				guildinfo.Add (info);
			}
			if (member.status=="Resting"){
				if (member.stats["CurrentHealth"]==0){
					guildinfo.Add (member.name +" "+ sdb.GetString ("Injured"));
				} else{
					guildinfo.Add (member.name +" "+ sdb.GetString ("Recovered1")+" "+sdb.GetString (member.gender+"Poss")+" "+sdb.GetString ("Recovered2"));
					member.status="Idle";
				}
			}
		}
		if (newmembers.Count > 0) {
			foreach (string name in newmembers) {
				guildinfo.Add (name + " " + sdb.GetString ("Join"));
			}
		}
		return guildinfo;
	}

	public void FinishShopping (Dictionary<int,int> items, int returnmoney)
	{
		money += returnmoney;
		GetItems(items);

	}
	public void GetItems(Dictionary<int,int> items){
		foreach (KeyValuePair<int,int> item in items) {
			inventory.AddItem (item.Key, 50, item.Value);
		}
	}

	public void FinishQuest (int questid, List<Member> members)
	{
		Quest quest = questBoard.FindQuest (questid);
		if (members != null) {
			foreach (Member member in members) {
				foreach (KeyValuePair<int,int> exp in quest.expReward) {
					member.GiveExp (GameObject.FindObjectOfType<SkillDatabase> ().GetSkill (exp.Key), exp.Value);
				}
			}
		} else {
			foreach (KeyValuePair<int,int> exp in quest.expReward) {
				memberlist [0].GiveExp (GameObject.FindObjectOfType<SkillDatabase> ().GetSkill (exp.Key), exp.Value);
			}
		}
		if (quest.itemRewards != null) {
			foreach (KeyValuePair<int,int> item in quest.itemRewards) {
				inventory.AddItem (item.Key, 100, item.Value);
			}
		}
		money += quest.moneyReward;
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

	public void FindNewQuest (int level)
	{
		questBoard.AddQuest (level);
	}

	public void FindNewRecruit (int level, string mainSkill)
	{
		if (level < 1)
			level = 1;
		Member newmember = GameObject.FindObjectOfType<CharDatabase> ().GenerateNewCharacter (level, mainSkill);
		RecruitMember (newmember);
		newmembers.Add (newmember.name);
	}
	public void FindNewArea(Area area){
		if (!knownAreas.Contains (area)){
			knownAreas.Add(area);
			successfulVisits[area]=0;
			foundHuntingGrounds[area]=0;
			foundGatheringPoints[area]=0;
		}
	}
	public void NextDayReset ()
	{
		newmembers.Clear ();
		foreach (Member member in memberlist) {
			member.NextDayResets ();
		}
	}
}
