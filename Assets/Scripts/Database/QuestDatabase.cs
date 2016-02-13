using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestDatabase
{

	[SerializeField]
	private List<Quest>
		randomQuestList = new List<Quest> ();
	[SerializeField]
	private List<Quest>
		mainQuestList = new List<Quest> ();
	private List<Quest> allQuestList = new List<Quest> ();
	public Dictionary<string,int> rankLevel = new Dictionary<string,int> (){{"F",1},{"E",10},{"D",20},{"C",30},{"B",40},{"A",50},{"S",60}};
	public List<Quest> generatedQuestList = new List<Quest> ();
	// Use this for initialization
	public QuestDatabase ()
	{
		NewQuest ("main", "Training", "Training", 1, 1, 100f, new Dictionary<int, int> (){{99,100},{2,100}}, "Train someone to learn the basics of important skills");
		NewQuest ("main", "Buy 1 Potion", "Item", 0, 0, 100f, new Dictionary<int, int> (){{99,20}}, "Send someone to the shop to buy a potion", "", null, new Dictionary<int,float> (){{0,1f}});
		NewQuest ("random", "Test1", "Training", 2, 1, 0f, new Dictionary<int, int> (){{99,100}}, "Test", "");
		NewQuest ("random", "Test2", "Training", 2, 1, 0f, new Dictionary<int, int> (){{1,100}}, "Test", "");
	}
	public void NewQuest (string questtype, string name, string type, int maxparticipants, int duration, float money=0f, Dictionary<int,int> exp=null, string shortdescription="", string longdescription="", Dictionary<int,float> reward=default(Dictionary<int,float>), Dictionary<int,float> items=default(Dictionary<int,float>), Dictionary<int,float> skills=default(Dictionary<int,float>))
	{
		if (questtype == "main") {
			mainQuestList.Add (new Quest (allQuestList.Count, name, type, maxparticipants, duration, money, exp, shortdescription, longdescription, reward, items, skills));
			allQuestList.Add (mainQuestList [mainQuestList.Count - 1]);
		} else if (questtype == "random") {
			randomQuestList.Add (new Quest ((allQuestList.Count), name, type, maxparticipants, duration, money, exp, shortdescription, longdescription, reward, items, skills));
			allQuestList.Add (randomQuestList [randomQuestList.Count - 1]);
		}

	}
	public List<Quest> GetQuest(){
		return generatedQuestList;
	}

	public void LoadQuest(List<Quest> quests){
		generatedQuestList=quests;
	}
	public Quest FindBaseQuest (int id, string type)
	{
		if (type == "main") {
			foreach (Quest quest in mainQuestList) {
				if (quest.baseId == id)
					return quest;
			}
		
		} else if (type == "random") {
			foreach (Quest quest in randomQuestList) {
				if (quest.baseId == id)
					return quest;
			}
		}
		return null;
	}

	public Quest FindBaseQuest (int id)
	{
		foreach (Quest quest in mainQuestList) {
			if (quest.baseId == id)
				return quest;
		}
		return null;
	}

	public Quest FindQuest (int id)
	{
		foreach (Quest quest in generatedQuestList) {
			if (quest.id == id)
				return quest;
		}
		return null;
	}

	public void GenerateQuest (int id, int level, string type)
	{
		generatedQuestList.Add (new Quest (generatedQuestList.Count, FindBaseQuest (id, type), level));
	}

	public void GenerateQuest (int level=1)
	{
		generatedQuestList.Add (new Quest (generatedQuestList.Count, RandomQuest (), level));
	}

	public Quest RandomQuest ()
	{
		return randomQuestList [Random.Range (0, randomQuestList.Count)];
	}

	public List<Quest> AvailableQuests ()
	{
		List<Quest> questlist = new List<Quest> ();
		if (generatedQuestList.Count > 0) {
			foreach (Quest quest in generatedQuestList) {
				if (!quest.accepted) {
					questlist.Add (quest);
				}
			}
		}
		return questlist;
	}
}
