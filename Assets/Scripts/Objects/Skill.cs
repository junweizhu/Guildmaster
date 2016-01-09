using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Skill {

	public int id;
	public string name;
	public Dictionary<string,int> statgrowth;

	public Skill(int id, string name, Dictionary<string,int> statgrowth)
	{
		this.id=id;
		this.name=name;
		this.statgrowth=statgrowth;
	}


}
