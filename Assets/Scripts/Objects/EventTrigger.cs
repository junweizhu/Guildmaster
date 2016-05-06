using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventTrigger
{
	public int id;
	public string name;
	//Conditions to activate;
	public bool activated = false;
	public List<int> gameEvent = new List<int> ();
	public bool timeCondition = false;
	public List<int>time;
	public bool questCondition = false;
	public List<int> questId;
	public bool memberCondition = false;
	public int memberCount;
	public int questCount;
	public int itemsGathered;
	public int itemsSold;
	public int monstersSlain;
	public int visitedShop;
	public int visitedSchool;
	public int membersInjured;
	public int treasury;
	public Dictionary<int,int> memberSkillRequirements;
	public bool areaCondition = false;
	public Dictionary<int,int> areaVisited;
	public bool itemCondition = false;
	public Dictionary<int,int> itemRequirements;
	public bool upgradeCondition = false;
	public Dictionary<int,int> upgradeRequirements;
	public bool activatesOnce = true;
	public bool isButton = false;

	// Use this for initialization

	public EventTrigger ()
	{
	}

	public EventTrigger (int id, string name, List<int> time=default(List<int>), List<int>quests=default(List<int>), int membercount=0, Dictionary<int,int>skillRequirements=default(Dictionary<int,int>), Dictionary<int,int>areas=default(Dictionary<int,int>), Dictionary<int,int>items=default(Dictionary<int,int>),Dictionary<int,int>upgrades=default(Dictionary<int,int>))
	{
		this.id = id;
		this.name = name;
		if (time != null) {
			timeCondition = true;
			this.time = time;
		}
		if (quests != null) {
			questId = quests;
			questCondition = true;
		}
		if (membercount > 0 || membercount == -1) {
			memberCondition = true;
			memberCount = membercount;
			memberSkillRequirements = skillRequirements;
		}
		if (areas != null) {
			areaCondition = true;
			areaVisited = areas;
		}
		if (items != null) {
			itemCondition = true;
			itemRequirements = items;
		}
		if (upgrades!=null){
			upgradeCondition=true;
			upgradeRequirements=upgrades;
		}

	}

	public EventTrigger (int id, string name, int membercount, int questcount=0, int itemsgathered=0, int itemsold=0, int monsterslain=0, int shopvisit=0, int schoolvisit=0)
	{
		this.id = id;
		this.name = name;
		memberCount = membercount;
		questCount = questcount;
		itemsGathered = itemsgathered;
		itemsSold = itemsold;
		monstersSlain = monsterslain;
		visitedShop = shopvisit;
		visitedSchool = schoolvisit;
	}

	public EventTrigger (int id, string name, bool activatesOnce, bool isButton=false)
	{
		this.id = id;
		this.name = name;
		this.activatesOnce = activatesOnce;
		this.isButton=isButton;
	}

	public void AddEvent (int id, bool activatesbyevent)
	{
		gameEvent.Add (id);
	}

	public void Activate ()
	{
		if ((activatesOnce && !activated) || !activatesOnce) {
			Debug.Log(name+" is activated");
			activated = true;
			if (gameEvent.Count > 0) {
				for (int i=0; i<gameEvent.Count; i++) {
					Database.events.GetEvent (gameEvent [i]).CheckIfActivated ();
				}
			}
			if (!activatesOnce) {
				activated = false;
			}
		}
	}

	public void CheckIfTriggered ()
	{
		if (activatesOnce&&activated||isButton){
			return;
		}
		if (treasury>Database.myGuild.money|| memberCount > Database.myGuild.characterlist.Count || questCount > Database.myGuild.questFinished || itemsGathered > Database.myGuild.itemsGathered || itemsSold > Database.myGuild.itemsSold || monstersSlain > Database.myGuild.monstersSlain || visitedShop > Database.myGuild.visitedShop || visitedSchool > Database.myGuild.visitedSchool) {
			return;
		}
		if (timeCondition) {
			if (time [2] > Database.year) {
				return;
			} else if (time [2] == Database.year) {
				if (time [1] > Database.month) {
					return;
				} else if (time [1] == Database.month && time [0] > Database.day) {
					return;
				}
			}
		}

		if (questCondition){
			for (int i=0;i<questId.Count;i++){
				if (!Database.quests.FindQuest(questId[i]).finished){
					return;
				}
			}
		}
		if (memberCondition){
			int suitedCharacters=0;
			for (int i=0;i<Database.myGuild.characterlist.Count;i++){
				bool suited=true;
				foreach (KeyValuePair<int,int> skill in memberSkillRequirements){
					if (suited&&Database.myGuild.characterlist[i].skillLevel[skill.Key]<skill.Value){
						suited=false;
					}
				}
				if (suited){
					suitedCharacters+=1;
				}
			}
			if (suitedCharacters<memberCount){
				return;
			}
		}
		if(itemCondition){
			foreach(KeyValuePair<int,int> item in itemRequirements){
				if (!Database.myGuild.inventory.Contains(item.Key,item.Value)){
					return;
				}
			}
		}
		if(areaCondition){
			foreach(KeyValuePair<int,int> area in areaVisited){
				if (!Database.myGuild.successfulVisits.ContainsKey(area.Key)||Database.myGuild.successfulVisits[area.Key]<area.Value){
					return;
				}
			}
		}
		if (upgradeCondition){
			foreach(KeyValuePair<int,int> upgrade in upgradeRequirements){
				if (Database.myGuild.upgradelist[upgrade.Key]<upgrade.Value){
					return;
				}
			}
		}

		Activate ();
	}
}
