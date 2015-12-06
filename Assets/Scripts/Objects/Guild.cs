using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Guild {
	public int guildId;
	public string guildName;
	public Image guildIcon;
	public int guildLevel=1;
	public int guildExp=0;
	public int guildFame=0;
	public int guildSize;
	public int guildMoney=0;
	//public Dictionary<int,Item> guildStorage= new Dictionary<int,Item>();
	public Inventory guildInventory;
	public List<Member> guildMemberlist= new List<Member>();

	public Guild()
	{

	}

	public Guild(int id,string name, int level, int fame, int size, int money,ItemDatabase db)
	{
		guildId=id;
		guildName=name;
		guildLevel=level;
		guildFame=fame;
		guildSize=size;
		guildMoney=money;
		guildInventory=new Inventory(db);
	}

	public void RecruitMember(Member newmember)
	{
		guildMemberlist.Add(newmember);
		newmember.recruited=true;
		newmember.guildnr=guildMemberlist.Count+1;
		guildSize+=1;
	}
	public List<Member> GetAvailableMembers()
	{
		List<Member> availablemembers= new List<Member>();
		for (int i=0; i<guildMemberlist.Count; i++) {
			if (guildMemberlist [i].memberStatus == "Idle") {
				availablemembers.Add(guildMemberlist [i]);
			}
		}
		return availablemembers;
	}
}
