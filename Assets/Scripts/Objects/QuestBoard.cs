using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class QuestBoard
{
	public List<Quest> questList = new List<Quest> ();
	public int size;
	//public List<string> questStatus = new List<string> ();
	//public List<float> questDays = new List<float> ();
	//public List<List<Member>> questParticipants = new List<List<Member>> ();
	//public List<int> questLevel=new List<int>();

	public QuestBoard ()
	{
		for (int i=0; i<21; i++) {
			questList.Add (new Quest ());
			//questStatus.Add (" ");
			//questDays.Add (0);
			//questParticipants.Add (new List<Member> ());
			//questLevel.Add(0);
		}
		size=21;
	}
		
	public void AddQuest (int id)
	{
		Quest quest = Database.quests.FindQuest (id);
		if (quest != null) {
			for (int i=0; i< questList.Count; i++) {
				if (questList [i].name == null&&i<size) {
					questList [i] = quest;
					quest.accepted=true;
					break;
				}
			}
		}
	}
	/*public void AddQuest (int level)
	{
		Quest quest = Database.quests.RandomQuest(level);
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
	}*/

	public void RemoveQuest (int slot)
	{
		questList.RemoveAt(slot);

		questList.Add (new Quest ());


	}

	public Quest FindQuest(int number){
		return questList[number];
	}
}
