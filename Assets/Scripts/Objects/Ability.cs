using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ability {

	public int id;
	public string name;
	public List<string> weaponType;
	public string element;
	public int range;
	public int manaCost;
	public Dictionary<string,int> statBonus;

	public Ability(int id,string name,string element,int range,int manaCost, List<string> weaponType,Dictionary<string,int> statBonus=null){
		this.id=id;
		this.name=name;
		this.element=element;
		this.range=range;
		this.manaCost=manaCost;
		this.weaponType=weaponType;
		this.statBonus=statBonus;
	}
}
