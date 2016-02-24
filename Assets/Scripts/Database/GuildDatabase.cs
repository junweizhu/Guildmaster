using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuildDatabase{

	[SerializeField]
	private List<Guild> guildList= new List<Guild>();
	// Use this for initialization
	public GuildDatabase() {}
	
	public void Generate(){
		guildList.Add(new Guild(0,"test",0,0,500));
	}

	public Guild FindGuild(int id)
	{
		foreach(Guild guild in guildList)
		{
			if (guild.id==id)
				return guild;
		}
		return null;
	}
}
