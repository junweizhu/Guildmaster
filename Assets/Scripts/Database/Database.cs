using UnityEngine;
using System.Collections;

public class Database
{


	public static ItemDatabase items = new ItemDatabase ();
	public static SkillDatabase skills = new SkillDatabase ();
	public static CharDatabase characters = new CharDatabase ();
	public static MonsterDatabase monsters = new MonsterDatabase ();
	public static UpgradeDatabase upgrades = new UpgradeDatabase ();
	public static AreaDatabase areas = new AreaDatabase ();
	public static QuestDatabase quests = new QuestDatabase ();
	public static GuildDatabase guilds = new GuildDatabase ();
	public static EventDatabase events = new EventDatabase ();
	public static StringDatabase strings = new StringDatabase ();
	public static Guild myGuild;
	public static GameManager game;
	public static int day = 1;
	public static int month = 1;
	public static int year = 1;

	public static void Initialize ()
	{
		guilds.Generate ();
		characters.Generate ();
		events.Generate ();
	}

	public static void SaveData ()
	{
		SaveLoad.data.day = day;
		SaveLoad.data.month = month;
		SaveLoad.data.year = year;
		SaveLoad.data.guilds = guilds.GetGuilds ();
		SaveLoad.data.characters = characters.GetCharacter ();
		SaveLoad.data.quests = quests.GetQuest ();
		SaveLoad.data.activatedtriggers = events.GetActivatedTriggers ();
		SaveLoad.data.finishedevents = events.GetFinishedEvents ();
		SaveLoad.Save ();
	}
	
	public static void LoadData ()
	{
		SaveLoad.Load ();
		day = SaveLoad.data.day;
		month = SaveLoad.data.month;
		year = SaveLoad.data.year;
		guilds.LoadGuilds (SaveLoad.data.guilds);
		characters.LoadCharacter (SaveLoad.data.characters);
		quests.LoadQuest (SaveLoad.data.quests);
		events.LoadActivatedTriggers (SaveLoad.data.activatedtriggers);
		events.LoadFinishedEvents (SaveLoad.data.finishedevents);
	}
}
