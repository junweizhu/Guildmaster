using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillDatabase{
	[SerializeField]
	private List<Skill> skillList= new List<Skill>();
	private List<Ability> abilityList= new List<Ability>();
	// Use this for initialization
	public SkillDatabase() {
		GenerateSkills();
		GenerateAbilities();
	}

	public void GenerateSkills(){
		skillList.Add(new Skill(0,"Weapon Skill",new Dictionary<string, int>()));
		skillList.Add(new Skill(1,"Magic Skill",new Dictionary<string, int>()));
		skillList.Add(new Skill(2,"Combat Skill",new Dictionary<string, int>(){{"MaxHealth",1},{"MaxMana",1}}));
		skillList.Add(new Skill(3,"Field Skill",new Dictionary<string, int>()));
		skillList.Add(new Skill(4,"Social Skill",new Dictionary<string, int>()));
	}
	
	public void GenerateAbilities(){
		abilityList.Add(new Ability(0,"Punch","Physical",1,0,new List<string>(){"Fist"}));
		abilityList.Add(new Ability(1,"Swing","Physical",1,0,new List<string>(){"Sword","Dagger","Mace","Axe","Staff","Spear"}));
		abilityList.Add(new Ability(2,"Thrust","Physical",1,0,new List<string>(){"Sword","Spear"}));
		abilityList.Add(new Ability(3,"Shoot","Physical",2,0,new List<string>(){"Bow"}));
	}
	public Skill GetSkill(int id)
	{
		foreach(Skill skill in skillList)
		{
			if (skill.id==id)
			{
				return skill;
			}
		}
		return null;
	}

	public Ability GetAbility(int id)
	{
		foreach(Ability ability in abilityList)
		{
			if (ability.id==id)
			{
				return ability;
			}
		}
		return null;
	}
	public int GetSkill(string name)
	{
		foreach(Skill skill in skillList)
		{
			if (skill.name==name)
			{
				return skill.id;
			}
		}
		return 99;
	}
	public List<Skill> SkillList()
	{
		return skillList;
	}
}
