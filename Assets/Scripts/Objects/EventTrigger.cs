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
	public Dictionary<int,int> memberSkillRequirements;
	public bool areaCondition = false;
	public Dictionary<int,int> areaVisited;
	public bool itemCondition = false;
	public Dictionary<int,int> itemRequirements;
	public bool activatesOnce = true;

	// Use this for initialization

	public EventTrigger ()
	{
	}

	public EventTrigger (int id, string name, List<int> time=default(List<int>), List<int>quests=default(List<int>), int membercount=0, Dictionary<int,int>skillRequirements=default(Dictionary<int,int>), Dictionary<int,int>areas=default(Dictionary<int,int>), Dictionary<int,int>items=default(Dictionary<int,int>))
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
	}

	public EventTrigger (int id, string name, bool activatesOnce)
	{
		this.id = id;
		this.name = name;
		this.activatesOnce = activatesOnce;
	}

	public void AddEvent (int id, bool activatesbyevent)
	{
		gameEvent.Add (id);
	}

	public void Activate ()
	{
		if ((activatesOnce && !activated) || !activatesOnce) {
			activated = true;
			//Debug.Log (name+" triggered");
			if (gameEvent.Count > 0) {
				for (int i=0; i<gameEvent.Count; i++) {
					Database.events.GetEvent (gameEvent [i]).CheckTrigger();
				}
			}
			if (activatesOnce){
				activated=false;
			}
		}
	}
}
