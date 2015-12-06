using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Skill {

	public int skillId;
	public string skillName;
	public Dictionary<string,int> skillStatgrowth;

	public Skill(int id, string name, Dictionary<string,int> statgrowth)
	{
		skillId=id;
		skillName=name;
		skillStatgrowth=statgrowth;
	}


}
