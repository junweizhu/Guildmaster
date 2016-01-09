using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Quest
{
	public int id;
	public string name;
	public string type;
	public int duration;
	public string shortDescription;
	public string longDescription;
	public int maxParticipants;
	public int moneyReward;
	public Dictionary<int,int> expReward;
	public Dictionary<int,int> itemRewards;
	public Dictionary<int,int> requiredSkills;
	public Dictionary<int,int> requiredItems;

	public Quest()
	{

	}
	public Quest (int id, string name, string type, int maxparticipants, int duration, int money=0, Dictionary<int,int> exp=default(Dictionary<int,int>), string shortdescription="", string longdescription="", Dictionary<int,int> reward=default(Dictionary<int,int>), Dictionary<int,int> items=default(Dictionary<int,int>), Dictionary<int,int> skills=default(Dictionary<int,int>))
	{
		this.id = id;
		this.name = name;
		this.type = type;
		this.maxParticipants = maxparticipants;
		this.duration = duration;
		this.moneyReward = money;
		this.shortDescription = shortdescription;
		expReward = exp;
		itemRewards = reward;
		requiredSkills = skills;
		requiredItems = items;
		this.longDescription = longdescription;
		if (longDescription!="")
		{
			longDescription+="\n";
		}
		if (requiredItems!=null) {
			longDescription += "Required Items:\n\n";
			foreach (KeyValuePair<int,int> i in items) {
				longDescription += "-"+i.Value.ToString () + " " + GameObject.FindObjectOfType<ItemDatabase> ().FindItem (i.Key).name;
				if (i.Value > 1) 
				{
					longDescription += "s\n";
				} 
				else
				{
					longDescription += "\n";
				}

			}
		}
		if (requiredSkills!=null)
		{
			longDescription += "Requires people with the following skills:\n\n";
			foreach (KeyValuePair<int,int> i in skills) {
				longDescription += "-"+ GameObject.FindObjectOfType<SkillDatabase> ().GetSkill (i.Key).name+ " Level "+i.Value.ToString ()+"\n";			
			}

		}
		if (longDescription=="")
			longDescription="No Description available";
	}
}
