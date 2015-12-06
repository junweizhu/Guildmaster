using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Quest{
	public int questId;
	public string questName;
	public string questType;
	public int questDuration;
	public string questShortDescription;
	public string questLongDescription;
	public int questMaxParticipants;
	public int questMoneyReward;
	public int questExpReward;
	public int questItemIdReward1;
	public int questAmountReward1;
	public int questItemIdReward2;
	public int questAmountReward2;
	public int questItemIdReward3;
	public int questAmountReward3;
	public int questRequiredSkill1;
	public int questRequiredSkill1Level;
	public int questRequiredSkill2;
	public int questRequiredSkill2Level;
	public int questRequiredSkill3;
	public int questRequiredSkill3Level;

	public Quest()
	{

	}

	public Quest(int id, string name,string type, int maxparticipants, int duration)
	{
		questId=id;
		questName=name;
		questType=type;
		questMaxParticipants=maxparticipants;
		questDuration=duration;
	}
}
