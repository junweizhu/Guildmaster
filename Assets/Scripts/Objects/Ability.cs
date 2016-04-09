using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Ability {

	public int id;
	public string name;
	public List<string> weaponType;
	public string element;
	public int range;
	public int manaCost;
	public int teachingCost;
	public int percentagePhysDamage;
	public int percentageMagDamage;
	public string skill;
	public Dictionary<string,int> statBonus;

	public Ability(int id,string name,string skill, string element,int range,int percentPhys, int percentMag,int manaCost, int teachingCost, List<string> weaponType,Dictionary<string,int> statBonus=null){
		this.id=id;
		this.name=name;
		this.element=element;
		this.range=range;
		this.manaCost=manaCost;
		this.teachingCost=teachingCost;
		this.weaponType=weaponType;
		this.statBonus=statBonus;
		percentagePhysDamage = percentPhys;
		percentageMagDamage = percentMag;
		this.skill = skill;
	}

	public int CalculateDamage(int pDamage,int mDamage, Character target=null,string weaponType=""){
		float damage = (float)pDamage*percentagePhysDamage/100;
		damage += (float)mDamage * percentageMagDamage/100;
		if (statBonus != null) {
			if (statBonus.ContainsKey ("Attack")) {
				damage += statBonus ["Attack"];
			}
		}
		if (target != null) {
			float defenseMod=1;
			if (skill == "Physical") {
				if (weaponType =="Mace") {
					defenseMod = 0.75f;
				}
				damage -= target.totalStats ["PDefense"]*defenseMod;
			} else {
				if (element == "None") {
					defenseMod = 0.75f;
				}
				damage -= target.totalStats ["MDefense"]*defenseMod;
			}
		}
		return Mathf.RoundToInt (damage);
	}
}
