using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StringDatabase : MonoBehaviour {

	Dictionary<string,string> dialogueList=new Dictionary<string, string>();
	// Use this for initialization
	void Start () {
		dialogueList.Add ("ShopSelect","Select who should go to the shop");
		dialogueList.Add ("SearchRecruit","Choose your options for looking for recruits");
		dialogueList.Add ("SearchQuest","Select who should look for new quests");
		dialogueList.Add ("AdventureTitle","Adventure");
		dialogueList.Add ("SearchRecruitTitle","Recruit");
		dialogueList.Add ("SearchQuestTitle","Quest Search");
		dialogueList.Add ("Adventure","Select who should travel to this place");
		dialogueList.Add ("QuestSelect","Select who should participate in this quest");
		dialogueList.Add ("Idle","Idle");
		dialogueList.Add ("Shopping","Shopping");
		dialogueList.Add ("SearchRecruiting","Looking for recruits");
		dialogueList.Add ("SearchQuesting","Looking for new quests");
		dialogueList.Add ("Questing", "Doing the quest");
		dialogueList.Add ("Ongoing","Ongoing");
		dialogueList.Add ("Exploring", "Exploring the");
		dialogueList.Add ("Gathering", "Gathering at the");
		dialogueList.Add ("Hunting", "Hunting at the");
		dialogueList.Add ("Training", "Training at the");
		dialogueList.Add ("Open","Open");
		dialogueList.Add ("Resting","Resting");
		dialogueList.Add ("ShopLog1","");
		dialogueList.Add ("ShopLog2","bought");
		dialogueList.Add ("ShopLog3","today.");
		dialogueList.Add ("QuestLog1","");
		dialogueList.Add ("QuestLog2","finished");
		dialogueList.Add ("QuestLog3","today.");
		dialogueList.Add ("SearchQuestLog1","");
		dialogueList.Add ("SearchQuestLog2","searched and");
		dialogueList.Add ("SearchQuestFail","failed to find");
		dialogueList.Add ("SearchQuestSuccess","found");
		dialogueList.Add ("SearchQuestLog3","a quest today.");
		dialogueList.Add ("SearchRecruitLog1","");
		dialogueList.Add ("SearchRecruitLog2","searched and ");
		dialogueList.Add ("SearchRecruitFail","failed to find anyone");
		dialogueList.Add ("SearchRecruitSuccess","found someone");
		dialogueList.Add ("SearchRecruitLog3","willing to join our guild.");
		dialogueList.Add ("AdventureLog1","");
		dialogueList.Add ("AdventureLog2","traveled to");
		dialogueList.Add ("AdventureLog3","today.");
		dialogueList.Add ("ExploringLog","and explored the area");
		dialogueList.Add ("ExploringLogSuccess","They found");
		dialogueList.Add ("ExploringLogFail","found nothing of interest");
		dialogueList.Add ("GatheringLog","and looked for materials");
		dialogueList.Add ("GatheringLogSuccess","gathered");
		dialogueList.Add ("GatheringLogFail","were unable to find anything");
		dialogueList.Add ("HuntingLog","and went hunting for animals");
		dialogueList.Add ("HuntingLogSuccess","gathered");
		dialogueList.Add ("HuntingLogFail","were unable to hunt anything");
		dialogueList.Add ("TrainingLog","and started training against monsters");
		dialogueList.Add ("TrainingLogSuccess","fought");
		dialogueList.Add ("TrainingLogFail","were unable to find anything worth fighting");
		dialogueList.Add ("Join","joined the guild.");
		dialogueList.Add ("And","and");
		dialogueList.Add ("NoTask","The day has passed without any important events");
		dialogueList.Add ("LevelUp","leveled up and gained");
		dialogueList.Add ("SkillUp","improved");
		dialogueList.Add ("MalePoss","his");
		dialogueList.Add ("FemalePoss","her");
		dialogueList.Add ("Strength","str");
		dialogueList.Add ("Intelligence","int");
		dialogueList.Add ("Dexterity","dex");
		dialogueList.Add ("MaxHealth","hp");
		dialogueList.Add ("MaxMana","mp");
		dialogueList.Add ("Agility","agi");
		dialogueList.Add ("pluralletter","s");
		dialogueList.Add ("Male3rd","He");
		dialogueList.Add ("Female3rd","She");
		dialogueList.Add ("Plural3rd","They");
		dialogueList.Add ("Injured","is injured and is currently resting for a day.");
		dialogueList.Add ("Recovered1","recovered from");
		dialogueList.Add ("Recovered2","injuries.");
	}

	public string GetString(string id)
	{
		return dialogueList[id];
	}
}
