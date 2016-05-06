using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameEvent
{
	public int id;
	public string name;

	public List<int> eventId = new List<int> ();
	public List<int> negEventId = new List<int> ();
	//public List<int> triggeredId=new List<int>();
	public bool active = false;
	public bool finished = false;
	//Actual event content
	public string dialogue;
	public Dictionary<int,int> itemToGive;
	public List<int> memberToRecruit;
	public Dictionary<int,int> questToTake;
	public List<int> areaToVisit;
	public bool enterName;
	public string changeNameType;
	public int changeNameId;
	public int eventTrigger;
	public int moneyToGive;
	public bool socializeEvent = false;

	public GameEvent ()
	{
	}

	public GameEvent (int id, string name, int eventTrigger, string dialogue, string changeNameType, int changeNameId)
	{
		BasicInfo (id, name, eventTrigger, dialogue);
		if (changeNameType != "") {
			enterName = true;
			this.changeNameType = changeNameType;
			this.changeNameId = changeNameId;
		}
	}

	public GameEvent (int id, string name, int eventTrigger, string dialogue, int money)
	{
		BasicInfo (id, name, eventTrigger, dialogue);
		moneyToGive = money;
	}

	public GameEvent (int id, string name, int eventTrigger, string dialogue, bool socializeEvent)
	{
		BasicInfo (id, name, eventTrigger, dialogue);
		this.socializeEvent = socializeEvent;
	}

	public GameEvent (int id, string name, int eventTrigger, string dialogue, Dictionary<int,int>items = default(Dictionary<int,int>), List<int>members = default(List<int>), Dictionary<int,int>quests = default(Dictionary<int,int>), List<int>areas = default(List<int>))
	{
		BasicInfo (id, name, eventTrigger, dialogue);
		itemToGive = items;
		memberToRecruit = members;
		questToTake = quests;
		areaToVisit = areas;
	}

	public void BasicInfo (int id, string name, int eventTrigger, string dialogue)
	{
		this.id = id;
		this.name = name;
		this.dialogue = dialogue;
		this.eventTrigger = eventTrigger;
	}
	/*	public void Trigger(int id){
		if (!triggeredId.Contains(id)){
			triggeredId.Add(id);
		}
		for (int i=0;i<eventId.Count;i++){
			Debug.Log ("Checking trigger"+eventId[i].ToString());
			if (!triggeredId.Contains(eventId[i])){
				Debug.Log("Failed");
				active=false;
				break;
			} else{
				Debug.Log("Success");
				active=true;
			}
		}
		if (active){
			Database.events.AddToQueue(id);
		}
	}*/

	public void CheckIfActivated ()
	{
		for (int i = 0; i < negEventId.Count; i++) {
			if (Database.events.GetTrigger (negEventId [i]).activated) {
				if (Database.events.activeSocializeEvents.Contains (this)) {
					Database.events.activeSocializeEvents.Remove (this);
				}
				return;
			}
		}
		
		if (!Database.events.activeSocializeEvents.Contains (this) && !Database.events.eventQueue.Contains (this)) {
			for (int i = 0; i < eventId.Count; i++) {
				if (!Database.events.GetTrigger (eventId [i]).activated) {
					return;
				}
			}
			if (socializeEvent) {
				Database.events.AddToSocializeEvent (id);
			} else {
				Database.events.AddToQueue (id);
			}
		}
	}

	public void FinishEvent ()
	{
		finished = true;
		bool newDialogue = false;
		if (itemToGive != null) {
			foreach (KeyValuePair<int,int> item in itemToGive) {
				Database.myGuild.inventory.AddItem (item.Key, item.Value);
			}
			newDialogue = true;
		}
		if (moneyToGive > 0) {
			Database.myGuild.money += moneyToGive;
			newDialogue = true;
		}
		if (areaToVisit != null) {
			for (int i = 0; i < areaToVisit.Count; i++) {
				Database.myGuild.FindNewArea (areaToVisit [i]);
			}
			newDialogue = true;
		}
		if (questToTake != null) {
			foreach (KeyValuePair<int,int> quest in questToTake) {
				Database.quests.GenerateQuest (quest.Key, quest.Value);
			}
			newDialogue = true;
		}
		if (memberToRecruit != null) {
			for (int i = 0; i < memberToRecruit.Count; i++) {
				Database.characters.GetCharacter (memberToRecruit [i]).recruitable = true;
			}
			newDialogue = true;
		}
		if (newDialogue) {
			Database.game.dialogueScreen.ShowDialogue (0, moneyToGive, itemToGive, null, questToTake, areaToVisit, memberToRecruit);
		}
		if (socializeEvent) {
			Database.events.activeSocializeEvents.Remove (this);
			Database.game.talkScreen.Refresh ();
		}
	}
}
