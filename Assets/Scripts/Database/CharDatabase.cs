using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharDatabase : MonoBehaviour {
	[SerializeField]
	private List<Member> memberList= new List<Member>();
	private List<string> maleNames=new List<string>(){"Test3","test4"};
	private List<string> femaleNames=new List<string>(){"Test5","test6"};
	private SkillDatabase skillDatabase;
	// Use this for initialization
	void Start () {
		skillDatabase=GetComponent<SkillDatabase>();
		memberList.Add (new Member(0, "Test",true,1,100,skillDatabase.SkillList()));
		memberList.Add (new Member(1, "Test2",true,1,100,skillDatabase.SkillList()));
	}

	public Member GetMember(int id)
	{
		foreach(Member member in memberList)
		{
			if (member.id==id)
			{
				return member;
			}
		}
		return null;
	}

	public Member GenerateNewCharacter(int level,string mainSkill)
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
		memberList.Add (new Member(memberList.Count,charName,male,level,100,skillDatabase.SkillList()));
		foreach (Skill skill in skillDatabase.SkillList())
		{
			float modifier=1.0f;
			int minimum=0;
			if (skill.name==mainSkill){
				modifier=1.25f;
				minimum=50;
			}

			int exp=Mathf.RoundToInt(Random.Range(50*modifier,((level)*modifier)*100))+minimum;
			memberList[memberList.Count-1].GiveExp(skill,exp);
		}
		return memberList[memberList.Count-1];

	}
	public List<Member> GetRecruitables()
	{
		List<Member> unrecruitedList= new List<Member>();
		foreach (Member member in memberList)
		{
			if (member.recruited==false)
				unrecruitedList.Add (member);
		}
		return unrecruitedList;
	}
}
