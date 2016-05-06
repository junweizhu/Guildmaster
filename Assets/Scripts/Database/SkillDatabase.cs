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
		abilityList.Add(new Ability(0,"Punch","Physical","Physical",		1,"Enemy",100,0,0,0,new List<string>(){"Fist"}));
		abilityList.Add(new Ability(1,"Swing","Physical","Physical",		1,"Enemy",100,0,0,0,new List<string>(){"Sword","Dagger","Mace","Axe","Staff","Spear","Rod"}));
		abilityList.Add(new Ability(2,"Thrust","Physical","Physical",		1,"Enemy",100,0,0,0,new List<string>(){"Sword","Spear"}));
		abilityList.Add(new Ability(3,"Shoot","Physical","Physical",		2,"Enemy",100,0,0,0,new List<string>(){"Bow"}));
		abilityList.Add(new Ability(4,"Fireball","Magic","Fire",			2,"Enemy",0,100,2,300,null, new Dictionary<string,int>(){{"Attack",1}}));
		abilityList.Add(new Ability(5,"Ice Arrow","Magic","Ice",			2,"Enemy",0,100,2,300,null, new Dictionary<string,int>(){{"Attack",1}}));
		abilityList.Add(new Ability(6,"Lightning Shock","Magic","Lightning",2,"Enemy",0,100,2,300,null, new Dictionary<string,int>(){{"Attack",1}}));
		abilityList.Add(new Ability(7,"Shadow Blast","Magic","Dark",		2,"Enemy",0,100,2,400,null, new Dictionary<string,int>(){{"Attack",1}}));
		abilityList.Add(new Ability(8,"Photon Ray","Magic","Light",			2,"Enemy",0,100,2,400,null, new Dictionary<string,int>(){{"Attack",1}}));
		abilityList.Add(new Ability(9,"Mana Shot","Magic","None",			2,"Enemy",0,100,2,400,null));
		abilityList.Add(new Ability(10,"Healing wind","Magic","Healing",	2,"Ally",0,100,2,700,null));
		abilityList.Add(new Ability(11,"Blazing Scimitar","Mixed","Fire",	1,"Enemy",75,50,2,0,new List<string>(){"Fist"}));
		abilityList.Add(new Ability(12,"Freezing Cutlass","Mixed","Ice",	1,"Enemy",75,50,2,0,new List<string>(){"Fist"}));
		abilityList.Add(new Ability(13,"Lightning Foil","Mixed","Lightning",1,"Enemy",75,50,2,0,new List<string>(){"Fist"}));
		abilityList.Add(new Ability(14,"Twilight Falcata","Mixed","Dark",	1,"Enemy",75,50,2,0,new List<string>(){"Fist"}));
		abilityList.Add(new Ability(15,"Shining Gladius","Mixed","Light",	1,"Enemy",75,50,2,0,new List<string>(){"Fist"}));
		abilityList.Add(new Ability(16,"Ethereal Sword","Mixed","None",		1,"Enemy",65,60,2,0,new List<string>(){"Fist"}));
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
