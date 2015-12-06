using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestDatabase : MonoBehaviour {

	[SerializeField]
	private List<Quest> questList= new List<Quest>();
	// Use this for initialization
	void Start () {
		questList.Add(new Quest(0,"Buy Potion","Item",0,0));
	}

	public Quest FindQuest(int id)
	{
		foreach(Quest quest in questList)
		{
			if (quest.questId==id)
				return quest;
		}
		return null;
	}
}
