using UnityEngine;
using System.Collections;

public class Database{

	public static StringDatabase strings=new StringDatabase();
	public static SkillDatabase skills=new SkillDatabase();
	public static ItemDatabase items=new ItemDatabase();
	public static CharDatabase characters=new CharDatabase();
	public static MonsterDatabase monsters=new MonsterDatabase();
	public static AreaDatabase areas= new AreaDatabase();
	public static QuestDatabase quests=new QuestDatabase();
	public static GuildDatabase guilds=new GuildDatabase();
	public static EventDatabase events=new EventDatabase();

	public static void Initialize(){
		guilds.Generate();
		characters.Generate();
		events.Generate();
	}
}
