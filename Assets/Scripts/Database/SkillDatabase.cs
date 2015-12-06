using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillDatabase : MonoBehaviour {
	[SerializeField]
	private List<Skill> skillList= new List<Skill>();
	// Use this for initialization
	void Start () {
		skillList.Add(new Skill(0,"Weapon Skill",new Dictionary<string, int>(){{"Strength",1}}));
		skillList.Add(new Skill(1,"Magic Skill",new Dictionary<string, int>(){{"Intelligence",1}}));
		skillList.Add(new Skill(2,"Combat Skill",new Dictionary<string, int>(){{"Health",1},{"Mana",1}}));
		skillList.Add(new Skill(3,"Field Skill",new Dictionary<string, int>()));
		skillList.Add(new Skill(4,"Social Skill",new Dictionary<string, int>()));
	}

	public Skill GetSkill(int id)
	{
		foreach(Skill skill in skillList)
		{
			if (skill.skillId==id)
			{
				return skill;
			}
		}
		return null;
	}
	public List<Skill> SkillList()
	{
		return skillList;
	}
}
