using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Quest
{
	public int baseId;
	public int id;
	public string name;
	public string type;
	public string status;
	public int duration;
	public string shortDescription;
	public string longDescription;
	public int maxParticipants;
	public int moneyReward;
	public Dictionary<int,int> expReward;
	public Dictionary<int,int> itemRewards;
	public Dictionary<int,int> requiredSkills;
	public Dictionary<int,int> requiredItems;
	public float baseMoneyReward;
	public Dictionary<int,float> baseItemRewards;
	public Dictionary<int,float> baseRequiredSkills;
	public Dictionary<int,float> baseRequiredItems;
	public bool finished = false;
	public bool accepted=false;
	public List<Character> participants = new List<Character> ();

	public Quest ()
	{

	}

	public Quest (int id, string name, string type, int maxparticipants, int duration, float money=0f, Dictionary<int,int> exp=default(Dictionary<int,int>), string shortdescription="", string longdescription="", Dictionary<int,float> reward=default(Dictionary<int,float>), Dictionary<int,float> items=default(Dictionary<int,float>), Dictionary<int,float> skills=default(Dictionary<int,float>))
	{
		baseId = id;
		this.name = name;
		this.type = type;
		this.maxParticipants = maxparticipants;
		this.duration = duration;
		baseMoneyReward = money;
		this.shortDescription = shortdescription;
		expReward = exp;
		baseRequiredSkills = skills;
		baseItemRewards = reward;
		baseRequiredItems = items;
		this.longDescription = longdescription;

		/*		if (longDescription!="")
		{
			longDescription+="\n";
		}

		if (requiredItems!=null) {
			longDescription += "Required Items:\n\n";
			foreach (KeyValuePair<int,int> i in items) {
				longDescription += "-"+i.Value.ToString () + " " + Database.items.FindItem (i.Key).name;
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
			foreach (KeyValuePair<int,int> i in requiredSkills) {
				longDescription += "-"+ Database.skills.GetSkill (i.Key).name+ " Level "+i.Value.ToString ()+"\n";			
			}

		}
		if (longDescription=="")
			longDescription="No Description available";
			*/
	}

	public Quest (int id, Quest quest, int level)
	{
		this.id = id;

		name = quest.name;
		type = quest.type;
		status = "Open";
		maxParticipants = quest.maxParticipants;
		duration = quest.duration;
		moneyReward = Mathf.CeilToInt (baseMoneyReward * level);
		shortDescription = quest.shortDescription;
		expReward = quest.expReward;
		if (baseRequiredSkills!=null) {
			requiredSkills = new Dictionary<int, int> ();
			foreach (KeyValuePair<int,float> skill in baseRequiredSkills) {
				requiredSkills [skill.Key] = Mathf.CeilToInt (skill.Value * level);
			}
		}
		if (baseRequiredItems!=null) {
			requiredItems = new Dictionary<int, int> ();
			foreach (KeyValuePair<int,float> item in baseRequiredItems) {
				requiredItems [item.Key] = Mathf.CeilToInt (item.Value * level);
			}
		}
		if (baseItemRewards!=null) {
			itemRewards = new Dictionary<int, int> ();
			foreach (KeyValuePair<int,float> item in baseItemRewards) {
				itemRewards [item.Key] = Mathf.CeilToInt (item.Value * level);
			}
		}
		if (quest.longDescription != "") {
			longDescription = quest.longDescription + "\n";
		}else{
			longDescription="";
		}
		
		if (requiredItems != null) {
			longDescription += Database.strings.GetString ("RequiredItems") + "\n\n";
			foreach (KeyValuePair<int,int> i in requiredItems) {
				longDescription += "-" + i.Value.ToString () + " " + Database.items.FindItem (i.Key).name;
				if (i.Value > 1) {
					longDescription += "s\n";
				} else {
					longDescription += "\n";
				}
			}
			longDescription += "\n";
		}
		if (requiredSkills != null) {
			longDescription += Database.strings.GetString ("RequiredSkills") + "\n\n";
			foreach (KeyValuePair<int,int> i in requiredSkills) {
				longDescription += "-" + Database.skills.GetSkill (i.Key).name + " Level " + i.Value.ToString () + "\n";			
			}
			
		}
		if (longDescription == "")
			longDescription = Database.strings.GetString ("NoDescription");
	}
}
