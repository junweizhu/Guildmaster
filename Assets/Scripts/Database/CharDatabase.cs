using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharDatabase {
	[SerializeField]
	private List<Character> memberList= new List<Character>();
	private List<string> maleNames=new List<string>(){"Test3","Test4","Test5","Test6","Test7","Test8"};
	private List<string> femaleNames=new List<string>(){"test3","test4","test5","test6","test7","test8"};
	int magicSkillId=1;

	// Use this for initialization
	public CharDatabase () {
	}

	public void Generate(){
		GenerateNewCharacter (1, 2, true);
		GenerateNewCharacter (1, 3, true,false,true);
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
		
	public Character GenerateNewCharacter(int level,int mainSkill,bool fixedGender=false, bool genderIsMale=true,bool learnMagic=false)
	{
		string charName;
		bool male = (Random.Range (0, 2) == 0);
		if (fixedGender) {
			male = genderIsMale;
		} 
		if (male){
			charName=maleNames[Random.Range(0,maleNames.Count)];
			maleNames.Remove(charName);
		}
		else{
			charName=femaleNames[Random.Range(0,femaleNames.Count)];
			femaleNames.Remove(charName);
		}
		memberList.Add (new Character(memberList.Count,charName,male,level));
		foreach (Skill skill in Database.skills.SkillList())
		{
			float modifier=1.0f;
			int minimum=0;
			if (skill.id==mainSkill){
				modifier=1.25f;
				minimum=50;
			}

			int exp=Mathf.RoundToInt(Random.Range(50*modifier,((level)*modifier)*100))+minimum;
			memberList[memberList.Count-1].GiveExp(skill.id,exp);
		}
		if (mainSkill == magicSkillId ||Random.Range(0,100)<=15||learnMagic) {
			int magic = Random.Range(4,7);
			memberList [memberList.Count - 1].abilities.Add (magic);
		}
		memberList[memberList.Count-1].NextDayResets();
		memberList[memberList.Count-1].recruitable=true;
		return memberList[memberList.Count-1];
	}

	public List<Character> GetRecruitables()
	{
		List<Character> unrecruitedList= new List<Character>();
		for (int i=0;i<memberList.Count;i++){
			if (memberList[i].recruited==false&&memberList[i].recruitable)
				unrecruitedList.Add (memberList[i]);
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
