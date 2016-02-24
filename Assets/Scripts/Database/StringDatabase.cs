using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StringDatabase{

	Dictionary<string,string>textList=new Dictionary<string, string>();
	List<Dialogue> dialogueList=new List<Dialogue>();
	public Dictionary<int,string> monthNames=new Dictionary<int, string>(){
		{1,"January"},
		{2,"February"},
		{3,"March"},
		{4,"April"},
		{5,"May"},
		{6,"June"},
		{7,"July"},
		{8,"August"},
		{9,"September"},
		{10,"Oktober"},
		{11,"November"},
		{12,"December"}};
	// Use this for initialization
	public StringDatabase() {
		GenerateText();
		GenerateDialogue();
	}


	public string GetString(string id)
	{
		return textList[id];
	}

	public List<Dialogue> GetDialogue(string dialoguename){
		List<Dialogue> dialogues=new List<Dialogue>();
		int count=0;

		for(int i=0;i<dialogueList.Count;i++){

			if (dialogueList[i].name==dialoguename){
				dialogues.Add(dialogueList[i]);
			} else if (count>0){
				break;
			}
		}
		if (dialogues.Count>1)
			dialogues=dialogues.OrderBy(dialogue=>dialogue.order).ToList();
		return dialogues;
	}
	void GenerateText(){
		textList.Add ("ShopSelect","Select who should go to the shop");
		textList.Add ("SearchRecruit","Choose your options for looking for recruits");
		textList.Add ("SearchQuest","Select who should look for new quests");
		textList.Add ("SocializeSelect","Who should go to the tavern?");
		textList.Add ("AdventureTitle","Adventure");
		textList.Add ("SocializeTitle","Quest Search");
		textList.Add ("Adventure","Select who should travel to this place");
		textList.Add ("QuestSelect","Select who should participate in this quest");
		textList.Add ("SelectItems","Select items to give to this member");
		textList.Add ("Idle","Idle");
		textList.Add ("Shopping","Shopping");
		//textList.Add ("SearchRecruiting","Looking for new recruits");
		//textList.Add ("SearchQuesting","Looking for new quests");
		textList.Add ("Questing", "Doing the quest {0}");
		textList.Add ("Socializeing","Going to the tavern");
		textList.Add ("Ongoing","Ongoing");
		textList.Add ("Exploring", "Exploring the {0}");
		textList.Add ("Gathering", "Gathering at the {0}");
		textList.Add ("Hunting", "Hunting at the {0}");
		textList.Add ("Training", "Training at the {0}");
		textList.Add ("Studying", "Studying");
		textList.Add ("Open","Open");
		textList.Add ("Resting","Resting");
		textList.Add ("Selling","Selling");
		textList.Add ("Date","Day {0} of {1} of Year {2}");
		textList.Add ("Male","Male");
		textList.Add ("Female","Female");
		textList.Add ("MalePoss","his");
		textList.Add ("FemalePoss","her");
		textList.Add ("Strength","strength");
		textList.Add ("Intelligence","intelligence");
		textList.Add ("Dexterity","dexterity");
		textList.Add ("Health","health");
		textList.Add ("Mana","mana");
		textList.Add ("Agility","agility");
		textList.Add ("pluralletter","s");
		textList.Add ("Male3rd","He");
		textList.Add ("Female3rd","She");
		textList.Add ("Plural3rd","They");
		textList.Add ("And","and");
		textList.Add ("Duration","{0} days");
		textList.Add ("Currency","{0} Gold");
		textList.Add ("CurrencyCounter","Gold");
		textList.Add ("NoTask","The day has passed without any important events.");
		textList.Add ("ShopLog","{0} went to shop.");
		textList.Add ("SellLog","{0} set up a stall");
		textList.Add ("QuestLog","{0} finished a quest.");
		textList.Add ("SchoolLog","{0} went to the training hall");
		textList.Add ("SocializeLog","{0} visited the tavern.");
		textList.Add ("SocializeSuccess","{0} talked to various people in the tavern.");
		textList.Add ("AdventureLog","{0} returned from an adventure");
		textList.Add ("MemberUp","{0} improved {1} skills!");
		textList.Add ("LevelUp","Level Up! {0} gained:\n{1}");
		textList.Add ("SkillUp","{0}'s {1} skills are improved");
		textList.Add ("Injured","{0} is injured and resting");
		textList.Add ("Recovered","{0} recovered from {1} injuries");
		textList.Add ("SchoolSkill","{0} studied hard.");
		textList.Add ("SchoolAbility","{0} studied a new ability");
		textList.Add ("LearnAbility","Learned the ability {0}");
		textList.Add ("Obtained","Obtained: \n{0}");
		textList.Add ("Bought","Bought: \n{0}");
		textList.Add ("SellSuccess","{0} successfully sold items.");
		textList.Add ("SellFail","{0} failed to sell any items.");
		textList.Add ("Sold","Sold: \n{0}");
		textList.Add ("Profit","Made "+textList["Currency"]);
		textList.Add ("MoneyBack","Due to negotiations, {0} less gold was paid.");
		textList.Add ("QuestFound","{0} people requested for your guild's help");
		textList.Add ("RecruitFound", "{0} wants to join your guild.");
		textList.Add ("GainedExp","{0} gained {1} {2} experience.");
		textList.Add ("AdventureSuccess","The adventure ended safely.");
		textList.Add ("AdventureFailed","{0} returned with injuries and failed to take the items they gathered.");
		textList.Add ("MonstersFought","{0} monsters fought.");
		textList.Add ("RequiresSkills","Requires people with the following skills:");
		textList.Add ("RequiresItems","Required Items:");
		textList.Add ("NoDescription","No Description available");
		textList.Add ("Experience","-{0} {1} Experience");
		textList.Add ("NoRewards","There are no rewards given in this quest.");
		textList.Add ("Heal","Recovers {0}.");
		textList.Add ("Ability","Teaches the ability {0}");
		textList.Add ("Prompt","Do you wish to continue?");
		textList.Add ("CloseRange","Close Range");
		textList.Add ("LongRange","Long Range");
		textList.Add ("NoMats","No materials required");

	}
	void GenerateDialogue(){
		dialogueList.Add (new Dialogue(0,"Tutorial",0,"This is the very first test of the current dialogue system, {0}. I hope it works to your satisfactory",new List<int>(){0,0},new List<string>(){"","Character"}));
		dialogueList.Add (new Dialogue(1,"Tutorial",1,"This test 2 of the current dialogue system, {0}. I hope it still works to your satisfactory",new List<int>(){0,0},new List<string>(){"","Character"}));
		dialogueList.Add (new Dialogue(2,"Tutorial2",0,"I hope this works",new List<int>(){0},new List<string>(){"Character"}));
	}
}
