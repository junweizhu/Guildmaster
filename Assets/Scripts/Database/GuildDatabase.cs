using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuildDatabase{

	[SerializeField]
	private List<Guild> guildList= new List<Guild>();
	private float expCalcA=10;
	private float expCalcB=5;
	private float expCalcC=4;
	private float expCalcD=3;
	// Use this for initialization
	public GuildDatabase() {}
	
	public void Generate(){
		guildList.Add(new Guild(0,"test",0,0,0));
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

	public int RequiredExpTNL(int level){
		float exp=expCalcA+(expCalcB*level)+(2*expCalcC*level)+(expCalcD*Mathf.Pow(level,2));
		return Mathf.RoundToInt(exp);
	}

	public void GuildMaintenancePay(){
		for (int i=0; i<guildList.Count;i++){
			guildList[i].PayMaintenance();
		}
	}
	public List<Guild> GetGuilds(){
		return guildList;
	}

	public void LoadGuilds(List<Guild> guildlist){
		guildList=guildlist;

	}
}
