using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuestBoard
{
	public List<Quest> questList = new List<Quest> ();
	public List<string> questStatus = new List<string> ();
	public List<float> questDays = new List<float> ();
	public List<List<Member>> questParticipants = new List<List<Member>> ();
	public List<int> questLevel=new List<int>();
	private QuestDatabase database;

	public QuestBoard (QuestDatabase db)
	{
		for (int i=0; i<10; i++) {
			questList.Add (new Quest ());
			questStatus.Add (" ");
			questDays.Add (0);
			questParticipants.Add (new List<Member> ());
			questLevel.Add(0);
		}
		database = db;
	}
		
	public void AddQuest (int id,int level)
	{
		Quest quest = database.FindQuest (id);
		if (quest != null) {
			for (int i=0; i< questList.Count; i++) {
				if (questList [i].name == null) {
					questList [i] = quest;
					questStatus [i] = "Open";
					questDays [i] = quest.duration;
					questParticipants [i].Clear ();
					questLevel[i]=level;
					break;
				}
			}
		}
	}
	public void AddQuest (int level)
	{
		Quest quest = database.RandomQuest(level);
		if (quest != null) {
			for (int i=0; i< questList.Count; i++) {
				if (questList [i].name == null) {
					questList [i] = quest;
					questStatus [i] = "Open";
					questDays [i] = quest.duration;
					questParticipants [i].Clear ();
					questLevel[i]=level;
					break;
				}
			}
		}
	}

	public void RemoveQuest (int slot)
	{
		questList.RemoveAt(slot);
		questStatus.RemoveAt(slot);
		questDays.RemoveAt(slot);
		questParticipants.RemoveAt(slot);
		questLevel.RemoveAt(slot);
		questList.Add (new Quest ());
		questStatus.Add ("Open");
		questDays.Add (0);
		questParticipants.Add (new List<Member>());
		questLevel.Add(0);

	}

	public Quest FindQuest(int number){
		return questList[number];
	}
}
