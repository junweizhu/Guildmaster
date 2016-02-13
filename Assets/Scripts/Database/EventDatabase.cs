using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventDatabase
{
	public List<EventTrigger> eventTriggers = new List<EventTrigger> ();
	public List<GameEvent> events = new List<GameEvent> ();
	public List<GameEvent> eventQueue = new List<GameEvent> ();

	public EventDatabase ()
	{
	}

	public void Generate ()
	{
		GenerateTrigger();
		GenerateEvent();
		SetEventTriggers();
		eventTriggers [0].Activate ();
	}

	public void GenerateTrigger(){
		eventTriggers.Add (new EventTrigger (0, "Start",true));
		eventTriggers.Add (new EventTrigger(1,"NextDay",false));
		eventTriggers.Add (new EventTrigger(2,"Guild",true));
		eventTriggers.Add (new EventTrigger(3,"Quest",true));
		eventTriggers.Add (new EventTrigger(4,"Tavern",true));
		eventTriggers.Add (new EventTrigger(5,"Market",true));
		eventTriggers.Add (new EventTrigger(6,"Shop",true));
		eventTriggers.Add (new EventTrigger(7,"Outside",true));
		eventTriggers.Add (new EventTrigger(8,"Prompt",false));
	}

	public void GenerateEvent(){
		events.Add (new GameEvent (0, "Tutorial", 1, "Tutorial"));
		events.Add (new GameEvent (1, "SecondTutorial", 2, "Tutorial2"));
	}

	public void SetEventTriggers(){
		TriggersToEvent(0,0);
		TriggersToEvent(1,1);
	}




	public void TriggersToEvent(int eventID,List<int> triggers)
	{
		for (int i=0;i<triggers.Count;i++){
			TriggersToEvent(eventID,triggers[i]);
		}
	}

	public void TriggersToEvent(int eventID,int triggerID){
		GameEvent gameEvent=GetEvent(eventID);
		gameEvent.eventId.Add(triggerID);
		GetTrigger(triggerID).gameEvent.Add(gameEvent.id);
	}
	
	public GameEvent GetEvent (int id)
	{
		foreach (GameEvent gameevent in events) {
			if (gameevent.id == id)
				return gameevent;
		}
		return null;
	}
	public EventTrigger GetTrigger (int id)
	{
		foreach (EventTrigger trigger in eventTriggers) {
			if (trigger.id == id)
				return trigger;
		}
		return null;
	}
	public void AddToQueue (int id)
	{
		foreach (GameEvent gameevent in events) {
			if (gameevent.id == id) {
				eventQueue.Add (gameevent);
				return;
			}
		}
	}

	public GameEvent GetActiveEvent ()
	{
		if (eventQueue.Count > 0) {
			GameEvent returnEvent = eventQueue [0];
			eventQueue.RemoveAt (0);
			return returnEvent;
		} else {
			return null;
		}
	}
}
