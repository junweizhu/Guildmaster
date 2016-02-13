using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameEvent{
	public int id;
	public string name;

	public List<int> eventId=new List<int>();
	//public List<int> triggeredId=new List<int>();
	public bool active=false;
	public bool finished=false;
	//Actual event content
	public string dialogue;
	public Dictionary<int,int> itemToGive;
	public Dictionary<int,int> memberToRecruit;
	public Dictionary<int,int> questToTake;
	public Dictionary<int,int> areaToVisit;
	public int eventTrigger;

	public GameEvent(){
	}

	public GameEvent(int id, string name,int eventTrigger,string dialogue,Dictionary<int,int>items=default(Dictionary<int,int>),Dictionary<int,int>members=default(Dictionary<int,int>),Dictionary<int,int>quests=default(Dictionary<int,int>), Dictionary<int,int>areas=default(Dictionary<int,int>)){
		this.id=id;
		this.name=name;
		this.dialogue=dialogue;
		itemToGive=items;
		memberToRecruit=members;
		questToTake=quests;
		areaToVisit=areas;
		this.eventTrigger=eventTrigger;
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

	public void CheckTrigger(){
		for (int i=0;i<eventId.Count;i++){
			if (!Database.events.GetTrigger(eventId[i]).activated){
				return;
			}
		}
		Database.events.AddToQueue(id);
	}
}
