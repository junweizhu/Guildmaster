using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuildDatabase : MonoBehaviour {

	[SerializeField]
	private List<Guild> guildList= new List<Guild>();
	// Use this for initialization
	void Start () {
		guildList.Add(new Guild(0,"test",1,0,10000, GetComponent<ItemDatabase>(),GetComponent<QuestDatabase>()));
	}
	
	// Update is called once per frame
	void Update () {
	
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
