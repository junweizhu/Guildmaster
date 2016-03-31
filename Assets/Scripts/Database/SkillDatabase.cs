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
		SetTrainingHall();
	}

	public void GenerateSkills(){
		skillList.Add(new Skill(0,"Weapon Skill",new Dictionary<string, int>()));
		skillList.Add(new Skill(1,"Magic Skill",new Dictionary<string, int>()));
		skillList.Add(new Skill(2,"Combat Skill",new Dictionary<string, int>(){{"Health",1},{"Mana",1}}));
		skillList.Add(new Skill(3,"Field Skill",new Dictionary<string, int>()));
		skillList.Add(new Skill(4,"Social Skill",new Dictionary<string, int>()));
	}
	
	public void GenerateAbilities(){
		abilityList.Add(new Ability(0,"Punch","Physical",1,0,0,new List<string>(){"Fist"}));
		abilityList.Add(new Ability(1,"Swing","Physical",1,0,0,new List<string>(){"Sword","Dagger","Mace","Axe","Staff","Spear"}));
		abilityList.Add(new Ability(2,"Thrust","Physical",1,0,0,new List<string>(){"Sword","Spear"}));
		abilityList.Add(new Ability(3,"Shoot","Physical",2,0,0,new List<string>(){"Bow"}));
		abilityList.Add(new Ability(4,"Fireball","Elemental",2,2,300,null, new Dictionary<string,int>(){{"MAttack",1}}));
		abilityList.Add(new Ability(5,"Ice Arrow","Elemental",2,2,300,null, new Dictionary<string,int>(){{"MAttack",1}}));
		abilityList.Add(new Ability(6,"Lightning Shock","Elemental",2,2,300,null, new Dictionary<string,int>(){{"MAttack",1}}));
		abilityList.Add(new Ability(7,"Shadow Blast","Black",2,2,400,null, new Dictionary<string,int>(){{"MAttack",1}}));
		abilityList.Add(new Ability(8,"Photon Ray","White",2,2,400,null, new Dictionary<string,int>(){{"MAttack",1}}));
		abilityList.Add(new Ability(9,"Healing wind","Healing",2,2,700,null));
		abilityList.Add(new Ability(10,"Mana Shot","Omni",2,3,400,null));
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
	public void SetTrainingHall(){
		Shop shop=Database.items.GetShop(4);
		//shop.AddSkill(-1,10,60);
		shop.AddSkill(0,17,100,30);
		shop.AddSkill(1,17,100,50);
		shop.AddSkill(2,17,100,75);
		shop.AddSkill(3,17,100,35);
		shop.AddSkill(4,17,100,25);
		shop.AddAbility(0,4);
		shop.AddAbility(0,5);
		shop.AddAbility(0,6);
		shop.AddAbility(10,7);
		shop.AddAbility(10,8);
		shop.AddAbility(10,9);
		shop.AddAbility(10,10);
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
