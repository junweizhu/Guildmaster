using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestDatabase : MonoBehaviour {

	[SerializeField]
	private List<Quest> randomQuestList=new List<Quest>();
	[SerializeField]
	private List<Quest> mainQuestList= new List<Quest>();
	private List<Quest> allQuestList=new List<Quest>();
	public Dictionary<string,int> rankLevel=new Dictionary<string,int>(){{"F",1},{"E",10},{"D",20},{"C",30},{"B",40},{"A",50},{"S",60}};
	// Use this for initialization
	void Start () {
		NewQuest("main","Training","Training",1,1,100,new Dictionary<int, int>(){{99,100},{2,100}},"Train someone to learn the basics of important skills","",null,null);
		NewQuest("main","Buy 1 Potion","Item",0,0,100,new Dictionary<int, int>(){{99,20}},"Send someone to the shop to buy a potion","",null,new Dictionary<int,int>(){{0,1}});
		NewQuest("random","Test1","Training",2,1,0,new Dictionary<int, int>(){{99,100}},"Test","");
	}

	public void NewQuest(string questtype,string name, string type, int maxparticipants, int duration, int money=0, Dictionary<int,int> exp=null, string shortdescription="", string longdescription="", Dictionary<int,int> reward=default(Dictionary<int,int>), Dictionary<int,int> items=default(Dictionary<int,int>), Dictionary<int,int> skills=default(Dictionary<int,int>))
	{
		if (questtype=="main")
		{
			mainQuestList.Add(new Quest((allQuestList.Count),name,type,maxparticipants,duration,money,exp,shortdescription,longdescription,reward,items,skills));
			allQuestList.Add(mainQuestList[mainQuestList.Count-1]);
		}
		else if (questtype=="random")
		{
			randomQuestList.Add (new Quest((allQuestList.Count),name,type,maxparticipants,duration,money,exp,shortdescription,longdescription,reward,items,skills));
			allQuestList.Add(randomQuestList[randomQuestList.Count-1]);
		}

	}
	public Quest FindQuest(int id,string type)
	{
		if (type=="main"){
		foreach(Quest quest in mainQuestList)
		{
			if (quest.id==id)
				return quest;
		}
		
		}
		else if (type=="random")
		{
			foreach(Quest quest in randomQuestList)
			{
				if (quest.id==id)
					return quest;
			}
		}
		return null;
	}
	public Quest FindQuest(int id)
	{
		foreach(Quest quest in mainQuestList)
		{
			if (quest.id==id)
				return quest;
		}
		return null;
	}
	public Quest RandomQuest(int level)
	{

		return randomQuestList[Random.Range(0,randomQuestList.Count)];
	}
}
