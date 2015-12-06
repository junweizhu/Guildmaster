using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Member {
	public int memberId;
	public int guildnr;
	public string memberName;
	public string memberGender;
	public int memberLevel;
	public int memberExp=0;
	public string memberStatus;
	public int memberHealth;
	public int memberMana;
	public int memberStrength;
	public int memberDexterity;
	public int memberAgility;
	public int memberIntelligence;
	public int memberDefense;
	public int memberMDefense;
	public int memberFame;
	public List<Skill> skillList = new List<Skill>();
	public Dictionary<Skill,int> memberSkillLevel= new Dictionary<Skill,int >();
	public Dictionary<Skill,int> memberSkillExp= new Dictionary<Skill,int >();
	public List<Item> memberInventory=new List<Item>();
	public List<int> memberItemQuality=new List<int>();
	public Dictionary <int,int> shopList=new Dictionary<int, int>();
	public int memberMoney;
	public bool recruited=false;

	public Member(int id,string name,bool male, int level, int money, List<Skill> skills)
	{
		memberId=id;
		memberName=name;
		if (male==true)
			memberGender="Male";
		else
			memberGender="Female";
		memberLevel=level;
		memberHealth=20;
		memberMana=20;
		memberStrength=5;
		memberDexterity=5;
		memberAgility=5;
		memberIntelligence=5;
		memberMoney=money;
		skillList=skills;
		memberStatus="Idle";
		foreach (Skill skill in skillList)
		{
			memberSkillLevel.Add(skill,1);
			memberSkillExp.Add (skill,0);
		}
	}
	public void giveMemberSkillExp(Skill skill, int exp)
	{
		memberSkillExp[skill]+=exp;
		if (memberSkillExp[skill]>100)
		{
			memberSkillExp[skill]-=100;
			memberSkillLevel[skill]+=1;
		}
	}
	public void AddItem(Item item, int quality)
	{
		memberInventory.Add(item);
		memberItemQuality.Add(quality);
	}
}
