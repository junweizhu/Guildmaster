using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharDatabase {
	[SerializeField]
	private List<Character> memberList= new List<Character>();
	private List<string> maleNames=new List<string>(){"Test3","test4"};
	private List<string> femaleNames=new List<string>(){"Test5","test6"};
	private List<Skill> skills;

	// Use this for initialization
	public CharDatabase () {
	}

	public void Generate(){
		skills=Database.skills.SkillList();
		memberList.Add (new Character(0, "Test",true,1,"Warrior"));
		memberList.Add (new Character(1, "Test2",true,1,"Mage"));
	}
	public List<Character> GetCharacter(){
		return memberList;
	}

	public void LoadCharacter(List<Character> members){
		memberList=members;
	}
	public Character GetCharacter(int id)
	{
		foreach(Character member in memberList)
		{
			if (member.id==id)
			{
				return member;
			}
		}
		return null;
	}
	public List<string> GetCharacterNames(List<int> id){
		List<string> name=new List<string>();
		for(int i=0;i<id.Count;i++){
			name.Add(GetCharacter(id[i]).name);
		}
		return name;
	}
	public Character GenerateNewCharacter(int level,string mainSkill)
	{
		string charName;
		bool male=(Random.Range(0,2)==0);
		if (male){
			charName=maleNames[Random.Range(0,maleNames.Count)];
			maleNames.Remove(charName);
		}
		else{
			charName=femaleNames[Random.Range(0,femaleNames.Count)];
			femaleNames.Remove(charName);
		}
		memberList.Add (new Character(memberList.Count,charName,male,level));
		foreach (Skill skill in skills)
		{
			float modifier=1.0f;
			int minimum=0;
			if (skill.name==mainSkill){
				modifier=1.25f;
				minimum=50;
			}

			int exp=Mathf.RoundToInt(Random.Range(50*modifier,((level)*modifier)*100))+minimum;
			memberList[memberList.Count-1].GiveExp(skill.id,exp);
		}
		return memberList[memberList.Count-1];
	}

	public List<Character> GetRecruitables()
	{
		List<Character> unrecruitedList= new List<Character>();
		foreach (Character member in memberList)
		{
			if (member.recruited==false)
				unrecruitedList.Add (member);
		}
		return unrecruitedList;
	}
	public int RecruitableId(Character member){
		List<Character> unrecruitedList= GetRecruitables();
		for(int i=0;i<unrecruitedList.Count;i++){
			if (unrecruitedList[i].id==member.id){
				return i;
			}
		}
		return 999;
	}
}
