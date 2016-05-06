using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventDatabase
{
	public List<EventTrigger> eventTriggers = new List<EventTrigger> ();
	public List<GameEvent> events = new List<GameEvent> ();
	public List<GameEvent> eventQueue = new List<GameEvent> ();
	public List<GameEvent> activeSocializeEvents = new List<GameEvent> ();
	private int lastObjectiveId = 98;

	public EventDatabase ()
	{
		GenerateTrigger ();
		GenerateEvent ();
		SetEventTriggers ();
	}

	public void Generate ()
	{
		GetTrigger (101).Activate ();

	}

	public void GenerateTrigger ()
	{
		AddButton ("Null");//0
		AddButton ("StartGame");//1
		AddButton ("Continue");//2
		AddButton ("Exit");//3
		AddButton ("Guild");//4
		AddButton ("Tavern");//5
		AddButton ("Plaza");//6
		AddButton ("Outside");//7
		AddButton ("Hall");//8
		AddButton ("Memberlist");//9
		AddButton ("Storage");//10
		AddButton ("Questboard");//11
		AddButton ("Tasklist");//12
		AddButton ("Upgradelist");//13
		AddButton ("ShowCharacterStat");//14
		AddButton ("ShowItemStat");//15
		AddButton ("ShowQuestStat");//16
		AddButton ("ShowTaskStat");//17
		AddButton ("ShowUpgradeStat");//18
		AddButton ("Socialize");//19
		AddButton ("Recruitlist");//20
		AddButton ("Requestlist");//21
		AddButton ("ShowRecruitStat");//22
		AddButton ("ShowRequestStat");//23
		AddButton ("AlchemyShop");//24
		AddButton ("WeaponSmith");//25
		AddButton ("ArmorSmith");//26
		AddButton ("ArcaneSmith");//27
		AddButton ("TrainingHall");//28
		AddButton ("MarketPlace");//29
		AddButton ("ShowAreaStat");//30
		AddButton ("Inventory");//31
		AddButton ("Skill");//32
		AddButton ("Ability");//33
		AddButton ("Dump");//34
		AddButton ("Detail");//35
		AddButton ("Reward");//36
		AddButton ("Member");//37
		AddButton ("Upgrade");//38
		AddButton ("MemberSelect");//39
		AddButton ("Recruit");//40
		AddButton ("AcceptRequest");//41
		AddButton ("ShopMemberSelect");//42
		AddButton ("Buy/Sell");//43
		AddButton ("Adventure");//44
		AddButton ("NextDay");//45

		//eventButtons.Add (new EventTrigger(0, "Null",true));//0 is the null value


		eventTriggers.Add (new EventTrigger (100, "StorageTooFull", false, true));
		eventTriggers.Add (new EventTrigger (101, "Start", true)); //100-200 should be for storymode/Objectives for townhall;
		eventTriggers.Add (new EventTrigger (102, "GaveNameToMale", true));
		eventTriggers.Add (new EventTrigger (103, "GaveNameToFemale", true));
		eventTriggers.Add (new EventTrigger (104, "GaveNameToGuild", true));
		eventTriggers.Add (new EventTrigger (105, "OfficialGuild", 5));
		eventTriggers.Add (new EventTrigger (106, "ShopVisited", 0, 0, 0, 0, 0, 1));
		eventTriggers.Add (new EventTrigger (107, "FirstAdventure", null, null, 0, null, new Dictionary<int, int> (){{0,1}}));
		eventTriggers.Add (new EventTrigger (108, "FirstItemSold", 0, 0, 0, 1, 0, 0));
		eventTriggers.Add (new EventTrigger (109, "FirstQuest", 0, 1));
		eventTriggers.Add (new EventTrigger (110, "FirstUpgrade", null, null, 0, null, null, null, new Dictionary<int,int> (){{0,1}}));

	}

	public void GenerateEvent ()
	{//event trigger here is what the event would trigger once done, 0 for no trigger

		events.Add (new GameEvent (0,"Null",0,"",0));
		events.Add (new GameEvent (1, "GiveNameToMale", 102, "NameToMale", "Character", 0));
		events.Add (new GameEvent (2, "GiveNameToFemale", 103, "NameToFemale", "Character", 1));
		events.Add (new GameEvent (3, "GiveNameToGuild", 104, "NameToGuild", "Guild", 0));
		events.Add (new GameEvent (4, "RecruitMembers", 0, "RecruitMember"));
		events.Add (new GameEvent (5, "RecruitMembersTalk", 0, "RecruitMemberTalk"));
		events.Add (new GameEvent (6, "VisitShop", 0, "VisitShop", 500));
		events.Add (new GameEvent (7, "VisitShopTalk", 0, "VisitShopTalk"));
		events.Add (new GameEvent (8, "GoToFirstArea", 0, "FirstArea", null, null, null, new List<int> (){0}));
		events.Add (new GameEvent (9, "GoToFirstAreaTalk", 0, "FirstAreaTalk"));
		events.Add (new GameEvent (10, "SellItem", 0, "FirstSale"));
		events.Add (new GameEvent (11, "SellItemTalk", 0, "FirstSaleTalk"));
		events.Add (new GameEvent (12, "CompleteFirstQuest", 0, "FirstQuest"));
		events.Add (new GameEvent (13, "CompleteFirstQuestTalk", 0, "FirstQuestTalk"));
		events.Add (new GameEvent (14, "UpgradeGuild", 0, "FirstUpgrade", 500));
		events.Add (new GameEvent (15, "UpgradeGuildTalk", 0, "FirstUpgradeTalk"));
		events.Add (new GameEvent (16, "TutorialFinish", 0, "TutorialFinish"));
		events.Add (new GameEvent (99, "NormalTalk", 0, "NormalTalk"));
		events.Add (new GameEvent (100, "StorageTooFull", 0, "StorageTooFull"));
		events.Add (new GameEvent (101, "TestSocialization", 0, "Socialize1",true));
	}

	public void SetEventTriggers ()
	{//Which event should require which trigger(s)?
		TriggersToEvent (1, 101);
		TriggersToEvent (2, 102);
		TriggersToEvent (3, 103);
		TriggersToEvent (4, 104);
		TriggersToEvent (6, 105);
		TriggersToEvent (8, 106);
		TriggersToEvent (10, 107);
		TriggersToEvent (12, 108);
		TriggersToEvent (14, 109);
		TriggersToEvent (16, 110);
		TriggersToEvent (100, 100);
		TriggersToEvent (101, 106);
	}

	public void SetEventEndTriggers(){//Until which trigger(s) activation is the event available?
		EndTriggersToEvent(101,107);


	}
	public void AddButton (string name)
	{
		eventTriggers.Add (new EventTrigger (eventTriggers.Count, name, false, true));
	}
	
	public void TriggersToEvent (int eventID, List<int> triggers)
	{
		for (int i=0; i<triggers.Count; i++) {
			TriggersToEvent (eventID, triggers [i]);
		}
	}
	public void TriggersToEvent (int eventID, int triggerID)
	{
		GameEvent gameEvent = GetEvent (eventID);
		gameEvent.eventId.Add (triggerID);
		GetTrigger (triggerID).gameEvent.Add (gameEvent.id);
	}
	public void EndTriggersToEvent (int eventID, List<int> triggers)
	{
		for (int i=0; i<triggers.Count; i++) {
			EndTriggersToEvent (eventID, triggers [i]);
		}
	}
	public void EndTriggersToEvent (int eventID, int triggerID)
	{
		GameEvent gameEvent = GetEvent (eventID);
		gameEvent.negEventId.Add (triggerID);
		GetTrigger (triggerID).gameEvent.Add (gameEvent.id);
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
		for (int i=0; i<eventTriggers.Count; i++) {
			if (eventTriggers [i].id == id)
				return eventTriggers [i];
		}
		return null;
	}

	public void AddToQueue (int id)
	{
		eventQueue.Add (GetEvent(id));
	}

	public void AddToSocializeEvent(int id){
		activeSocializeEvents.Add (GetEvent (id));
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
	public void UpdateTrigger ()
	{
		for (int i=0; i<eventTriggers.Count; i++) {
			if (!eventTriggers [i].isButton) {
				eventTriggers [i].CheckIfTriggered ();
			}
		}
	}

	public int CurrentObjectiveId ()
	{
		for (int i=0; i<events.Count; i++) {
			if (events [i].id <= lastObjectiveId) {
				if (events [i].eventId.Count > 0) {
					for (int j=0; j<events[i].eventId.Count; j++) {
						if (!GetTrigger (events [i].eventId [j]).activated)
							return events[i].id-1;
					}
				}
			}
		}
		return 99;
	}

	public void ActivateObjectiveEvent ()
	{
		AddToQueue (CurrentObjectiveId ());
	}

	public List<int>GetActivatedTriggers ()
	{
		List<int> activatedTriggers = new List<int> ();
		for (int i=0; i<eventTriggers.Count; i++) {
			if (eventTriggers [i].activated) {
				activatedTriggers.Add (eventTriggers [i].id);
			}
		}
		return activatedTriggers;
	}

	public void LoadActivatedTriggers (List<int>activatedTriggers)
	{
		if (activatedTriggers.Count > 0) {
			for (int i=0; i<activatedTriggers.Count; i++) {
				GetTrigger (activatedTriggers [i]).activated = true;
			}
		}
	}

	public List<int>GetFinishedEvents(){
		List<int> finishedEvents = new List<int> ();
		for (int i=0; i<events.Count; i++) {
			if (events [i].finished) {
				finishedEvents.Add (events [i].id);
			}
		}
		return finishedEvents;
	}
	public void LoadFinishedEvents (List<int>finishedEvents)
	{
		if (finishedEvents.Count > 0) {
			for (int i=0; i<finishedEvents.Count; i++) {
				GetEvent (finishedEvents [i]).finished = true;
			}
		}
	}
}
