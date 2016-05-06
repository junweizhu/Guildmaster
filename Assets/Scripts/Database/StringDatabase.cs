using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StringDatabase{

	Dictionary<string,string>textList=new Dictionary<string, string>();
	List<Dialogue> dialogueList=new List<Dialogue>();
	Dictionary<int,string>objectiveText=new Dictionary<int, string>();
	List<int> standardDialogueStringId= new List<int>(){0};
	List<string> dialogueStringType= new List<string>(){"Clerk"};
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
		GenerateObjectiveText();
	}


	public string GetString(string id)
	{
		return textList[id];
	}

	public string GetObjectiveString(int id)
	{
		if (objectiveText.ContainsKey(id)){
			return objectiveText[id];
		} else{
			return objectiveText[0];
		}
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
		textList.Add ("SocializeTitle","Socialize");
		textList.Add ("Adventure","Select who should travel to this place");
		textList.Add ("QuestSelect","Select who should participate in this quest");
		textList.Add ("SelectItems","Select items to give to this member");
		textList.Add ("Idle","Idle");
		textList.Add ("Shopping","Shopping");
		textList.Add ("Questing", "Doing the quest {0}");
		textList.Add ("Socializeing","Going to the tavern");
		textList.Add ("Ongoing","Ongoing");
		textList.Add ("Exploring", "Exploring the {0}");
		textList.Add ("Gathering", "Gathering at the {0}");
		//textList.Add ("Hunting", "Hunting at the {0}");
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
		textList.Add ("Maintenance","({0} per day)");
		textList.Add ("GuildReport","Guild report");
		textList.Add ("MaintenancePaid","Paid {0} in maintenance cost.");
		textList.Add ("GuildLevelUp","Guild has gained a level.");
		textList.Add ("CurrencyCounter","Gold");
		textList.Add ("NoTask","The day has passed without any important events.");
		textList.Add ("ShopLog","{0} went to shop.");
		textList.Add ("SellLog","{0} set up a stall");
		textList.Add ("QuestLog","{0} finished a quest.");
		textList.Add ("SchoolLog","{0} went to the training hall");
		textList.Add ("SocializeLog","{0} visited the tavern.");
		textList.Add ("SocializeSuccess","{0} talked to various people in the tavern.");
		textList.Add ("QuestSuccess","Successfully finished {0}.");
		textList.Add ("QuestItemReward","Received Reward: \n{0}");
		textList.Add ("QuestMoneyReward","Received "+textList["Currency"]);
		textList.Add ("AdventureLog","{0} returned from an adventure");
		textList.Add ("ExploreLog","Explored {0} % of the area.");
		textList.Add ("ExploreAreaFound", "Found a way to the {0}");
		textList.Add ("MemberUp","{0} improved {1} skills!");
		textList.Add ("LevelUp","Level Up! {0} gained:\n{1}");
		textList.Add ("SkillUp","{0}'s {1}s are improved");
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
		textList.Add ("AreaFound", "One of the visitors showed the location of the {0}");
		textList.Add ("GainedExp","{0} gained {1} {2} experience.");
		textList.Add ("LogGainedExp","+ {0} {1} exp");
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
		textList.Add ("Recruit","Is \"{0}\" correct?");
		textList.Add ("Guild","Guild");
		textList.Add ("GainedItem", "Received {0}x {1}");
		textList.Add ("GuildGainedExp","Guild received {0} experience");
		textList.Add ("GuildGainedMoney","Received " +textList["Currency"]+".");
		textList.Add ("GuildGainedArea","You can now go to {0}.");
		textList.Add ("GuildGainedQuest","\"{0}\" has been added to the list of acceptable requests. Go to the tavern for more information.");
		textList.Add ("GuildGainedRecruit","{0} wants to join your guild. Find {1} in the tavern.");
	}

	void GenerateObjectiveText(){
		objectiveText.Add(0,"");
		objectiveText.Add(5,"Have a total of 5 members in your guild.");
		objectiveText.Add(7,"Visit the shop and buy supplies for your first adventure.");
		objectiveText.Add(9,"Explore the first area.");
		objectiveText.Add(11,"Sell an item.");
		objectiveText.Add(13,"Find and complete a request.");
		objectiveText.Add(15,"Upgrade your guildhouse.");
	}
	void GenerateDialogue(){
		dialogueList.Add (new Dialogue("StorageTooFull",0,"There's not enough space in the storage to hold all these items, we have to sell or throw away some of them.",new List<int>(){0},new List<string>(){"Character"}));
		dialogueList.Add (new Dialogue("NameToMale",0,"Welcome to the town hall, what can I do for you?",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("NameToMale",1,"So, you would like to register a new guild and make a living here?",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("NameToMale",2,"I see. I shall start the registration in that case.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("NameToMale",3,"I will start with the young man, what is your name?",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("NameToFemale",0,"{0} is it? Understood. And what is your name, young lady?",new List<int>(){0,0},new List<string>(){dialogueStringType[0],"Character"}));
		dialogueList.Add (new Dialogue("NameToGuild",0,"{0} is it? I've registered you two as temporary citizen in our fair town. You are limited in what you are capable of doing here until we recognize you as official citizens. This will happen once your guild has undergone proper procedures",new List<int>(){0,1},new List<string>(){dialogueStringType[0],"Character"}));
		dialogueList.Add (new Dialogue("NameToGuild",1,"I assume you have thought of a guild name, have you not? The name can not be changed once registered so once you have thought of it long enough, we will continue.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("NameToGuild",2,"Now {0} and {1}, may I please have the name of your new guild?",new List<int>(){0,0,1},new List<string>(){dialogueStringType[0],"Character","Character"}));

		dialogueList.Add (new Dialogue("RecruitMember",0,"{0}? Your registration is complete. You have finished the first step of becoming an official guild. Congratulations.",new List<int>(){0,0},new List<string>(){dialogueStringType[0],"Guild"}));
		dialogueList.Add (new Dialogue("RecruitMember",1,"For now, you will be given a small room for you to use as your guildhouse for the time being, free of charge.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("RecruitMember",2,"Before we can officially recognize your guild, you will be given several tasks which would help you to learn your roles and abilities as a guild.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("RecruitMember",3,"In order to be recognized as an official guild, you should have at least five members in the guild.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("RecruitMember",4,"This means you would need at least three more members before you are considered large enough to be a guild. So your first task is to find these three recruits.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("RecruitMember",5,"To find recruits, go to the tavern and choose to socialize.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("RecruitMember",6,"Then pick at most five of your members to do your recruiting. I suggest both of you should look for new recruits",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("RecruitMember",7,"Once you have decided on the members a task will be listed under the task list. You can review the tasks and even cancel them in your guild.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("RecruitMember",8,"Bear in mind that currently your member list can not exceed a number of five. You can recruit more once you have your very own guild house.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("RecruitMember",9,"When you are done deciding on the tasks of everyone, you are ready to start the day. Press the button on the far left corner to finish the day.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("RecruitMember",10,"You can always consult me if you forget how to proceed. You can find me in the town hall. Good luck, members of {0}!",new List<int>(){0,0},new List<string>(){dialogueStringType[0],"Guild"}));
		dialogueList.Add (new Dialogue("RecruitMemberTalk",0,"Members of {0}, you must recruit three more members before we proceed with the next procedure. Good luck.",new List<int>(){0,0},new List<string>(){dialogueStringType[0],"Guild"}));

		dialogueList.Add (new Dialogue("VisitShop",0,"Well done, you have finished your first task. You are one step closer to being an official guild!",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("VisitShop",1,"Without further ado, I will introduce you to your next task.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("VisitShop",2,"As you may have heard, one of your tasks as a guild is to provide materials to the town.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("VisitShop",3,"There are many places outside the town you can visit which has an abundance of valuable materials, suited for everyday use.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("VisitShop",4,"But of course, those areas are not without any danger so it is very important to prepare yourself for such travels",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("VisitShop",5,"Your task is to supply yourself with items necessary for to go to a location and gather materials. Here is some money for you to buy your supplies with.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("VisitShop",6,"Go to the plaza and buy yourself some basic items.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("VisitShop",7,"Choose at most five members who should go to the shop to buy the items. Then choose what to buy in what quantities. The maximum quantity will be shown below the item window.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("VisitShop",8,"You can currently carry just one item in your hand per person without any sort of help, so I suggest buying a {0} to carry more items.",new List<int>(){0,Database.items.FindItemId("Leather Pouch")},new List<string>(){dialogueStringType[0],"Item"}));
		dialogueList.Add (new Dialogue("VisitShop",9,"You are free to buy anything else you like, you are limited in how many items you can store in your storage however. You can find your items in the storage under your guild",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("VisitShop",10,"Good luck. I will provide you with a perfect location to gather materials from once you bought your first items.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("VisitShopTalk",0,"Visit the shops and buy supplies for your first adventure. I suggest buying a {0} at least.",new List<int>(){0,Database.items.FindItemId("Leather Pouch")},new List<string>(){dialogueStringType[0],"Item"}));

		dialogueList.Add (new Dialogue("FirstArea",0,"I assume you have bought the necessary supply for your travels, so let us continue with the next procedure.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstArea",1,"The place you will go to is called the {0}",new List<int>(){0,0},new List<string>(){dialogueStringType[0],"Area"}));
		dialogueList.Add (new Dialogue("FirstArea",2,"It's a relatively safe place, but dangerous nonetheless",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstArea",3,"Before you leave, I suggest you put on the items you have bought, including your consumable items.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstArea",4,"Consumable items will be equipped as an accessory. You will not take them with you if you do not equip them.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstArea",5,"Pay close attention to the durability of the items, once it reaches zero it will be gone and you will have to buy items again.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstArea",6,"Once you are ready, go outside and travel to the {0} with the five of you.",new List<int>(){0,0},new List<string>(){dialogueStringType[0],"Area"}));
		dialogueList.Add (new Dialogue("FirstArea",7,"Do not worry too much if you and your party gets injured and cannot go back. There are patrols everywhere and they will help you return to the town safely. You will lose all your gathered materials however.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstArea",8,"You will have to rest for the day if that happens though, so take good care of yourselves and your members.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstArea",9,"We will proceed once we confirm that you are able to return safely from your travels.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstArea",10,"Best of luck, members of {0}. I will await for your arrival.",new List<int>(){0,0},new List<string>(){dialogueStringType[0],"Guild"}));
		dialogueList.Add (new Dialogue("FirstAreaTalk",0,"Please travel to {0} and successfully return with a few items.",new List<int>(){0,0},new List<string>(){dialogueStringType[0],"Area"}));

		dialogueList.Add (new Dialogue("FirstSale",0,"I have watched your performances in the {0} and I must say, you have the potential to do great in this town. I am quite satisfied with your results so far.",new List<int>(){0,0},new List<string>(){dialogueStringType[0],"Area"}));
		dialogueList.Add (new Dialogue("FirstSale",1,"Before you start your travels, you can choose what to focus on. Exploring the area puts your focus more on learning about the area as well as the possibility to find more places to travel to.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstSale",2,"Gathering materials focuses more on gathering as many materials as it is possible, while training focuses more on finding and fighting monsters in the area.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstSale",3,"Choose wisely with your goals in mind.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstSale",4,"Now, as you have gathered a few materials, it is time to sell a few on the marketplace.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstSale",5,"This is one of the methods to make profit out of materials you have found during your travels.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstSale",6,"Go to the plaza and find the marketplace. In here you can select up to five members to do the selling and the items you wish to sell.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstSale",7,"Be aware that you can only take as many items to sell as your hands and pouches allow you to.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstSale",8,"Once you're done, a stall will be put for the day and all you can do is wait and hope for a customer to buy them. ",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstSale",9,"It may be hard to sell things at first, but once you have gained experience in selling and socializing, it will be much easier.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstSale",10,"If you need more materials, you should go to the {0} again and see if you can find any. Best of luck to you all.",new List<int>(){0,0},new List<string>(){dialogueStringType[0],"Area"}));
		dialogueList.Add (new Dialogue("FirstSaleTalk",0,"Visit the marketplace in the plaza and sell at least one item before we proceed to the next task.",standardDialogueStringId,dialogueStringType));

		dialogueList.Add (new Dialogue("FirstQuest",0,"I see you have made your first sale, members of {0}. Congratulations.",new List<int>(){0,0},new List<string>(){dialogueStringType[0],"Guild"}));
		dialogueList.Add (new Dialogue("FirstQuest",1,"The marketplace is an important place for guilds to make profit off of their findings, so be sure to make use of it often.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstQuest",2,"This is not the only way to gain money however.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstQuest",3,"You can also gain money by helping other people with their requests.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstQuest",4,"People from time to time will ask guilds for help and it will be up to you to decide to find these people and help them out.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstQuest",5,"Most of these people will gather in the tavern. So I suggest to send your members to strike up a conversations with other people to find problems they have",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstQuest",6,"Once you find some requests, you can choose to accept the requests in the tavern.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstQuest",7,"Some requests asks for people with certain skills, or certain items.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstQuest",8,"When you have the people or items for it, go to the questboard in your guild and view the request.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstQuest",9,"If they need the items and you have them, you can choose to complete right away. If they need people however, it will take however long the requests takes before you receive your rewards.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstQuest",10,"Find and finish one request at least before we proceed to the last procedure. Good luck.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstQuestTalk",0,"Find and accept a request in the tavern and then do what you can to complete it.",standardDialogueStringId,dialogueStringType));

		dialogueList.Add (new Dialogue("FirstUpgrade",0,"I have heard you have helped one of our citizens with a request. Thank you, members of {0}",new List<int>(){0,0},new List<string>(){dialogueStringType[0],"Guild"}));
		dialogueList.Add (new Dialogue("FirstUpgrade",1,"By gathering and selling materials and helping others with their problem, we can improve our humble town into something grand.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstUpgrade",2,"I am satisfied with the fact that you met all my expectations as a guild with potential.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstUpgrade",3,"Before we can officially recognize you as a guild, there is one more thing you have to do.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstUpgrade",4,"As you have seen, the guilds in our town has their own guildhouses and their membersize are greater than yours.This is because their guildhouses can accommodate that amount of members.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstUpgrade",5,"In order for your guild to grow, you have to move to your own guildhouse.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstUpgrade",6,"You can do so by requesting an upgrade on your guildhouse in our town hall. This will give you more room to get more members",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstUpgrade",7,"Upgrading buildings cost money however. There will be initial requirements and maintenance fees",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstUpgrade",8,"The initial requirements could be both a fee and specific materials for the upgrade.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstUpgrade",9,"The mainenance fee is necessary to keep the buildings in shape. It is shown as fee per day, but you pay the fees once a month, so keep an eye to your money.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstUpgrade",10,"I will award you a bit of extra money, so head to the upgrade department of the town hall and move yourselves to a guildhouse of your own. I will see you once this has happened",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("FirstUpgradeTalk",0,"Please head to the Upgrade department and request an upgrade to your guildhouse.",standardDialogueStringId,dialogueStringType));

		dialogueList.Add (new Dialogue("TutorialFinish",0,"Excellent, you have now your very own {0} guildhouse!",new List<int>(){0,0},new List<string>(){dialogueStringType[0],"Guild"}));
		dialogueList.Add (new Dialogue("TutorialFinish",1,"Now that you have your own guildhouse, all other upgrades are available to you. You can only upgrade your guild if you have shown to have done enough for our town.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("TutorialFinish",2,"Perhaps we can measure that in the form of guild level. Be sure to keep watch over it once you reach an appropriate level",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("TutorialFinish",3,"I have submitted the paperwork already and they have accepted you as an official guild.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("TutorialFinish",4,"Congratulations to you, members of {0}. All facilities will be open to you as of now and you can make your own plans to help the town and yourselves.",new List<int>(){0,0},new List<string>(){dialogueStringType[0],"Guild"}));
		dialogueList.Add (new Dialogue("TutorialFinish",5,"Now you will be on your own to find your place in town. I suggest you start socializing again for requests or more recruits.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("TutorialFinish",6,"You may even be able to learn of new places to gather materials from.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("TutorialFinish",7,"If you have new inexperienced recruits, you may wish to send them to our training hall in the plaza as well. It would help them learn the basics or even magic, should you have the money.",standardDialogueStringId,dialogueStringType));
		dialogueList.Add (new Dialogue("TutorialFinish",8,"My task here is done. May your names be heard by all of us, members of {0}.",new List<int>(){0,0},new List<string>(){dialogueStringType[0],"Guild"}));

		dialogueList.Add (new Dialogue("NormalTalk",0,"Hello, members of {0}, is everything going fine?",new List<int>(){0,0},new List<string>(){dialogueStringType[0],"Guild"}));

		dialogueList.Add (new Dialogue("Socialize1",0,"Testing the socializing event. {0}, what do you think?",new List<int>(){1,0},new List<string>(){"Character","Character"}));
		dialogueList.Add (new Dialogue("Socialize1",1,"So far, so good {0}. Let's do our best.",new List<int>(){0,1},new List<string>(){"Character","Character"}));
	}
}
