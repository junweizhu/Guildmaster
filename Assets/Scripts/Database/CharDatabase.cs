using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharDatabase : MonoBehaviour {
	[SerializeField]
	private List<Member> memberList= new List<Member>();
	private SkillDatabase skillDatabase;
	private int nextId;
	// Use this for initialization
	void Start () {
		skillDatabase=GetComponent<SkillDatabase>();
		memberList.Add (new Member(0, "Test",true,1,100,skillDatabase.SkillList()));
		memberList.Add (new Member(1, "Test2",true,1,1000,skillDatabase.SkillList()));
		memberList.Add (new Member(2, "Test3",true,1,100,skillDatabase.SkillList()));
		memberList.Add (new Member(3, "Test4",true,1,100,skillDatabase.SkillList()));

		nextId=memberList[memberList.Count-1].memberId+1;
	}

	public Member GetMember(int id)
	{
		foreach(Member member in memberList)
		{
			if (member.memberId==id)
			{
				return member;
			}
		}
		return null;
	}

	public Member GenerateNewCharacter()
	{
		return null;

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
