using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradeDatabase{
	List<Upgrade> upgradeList=new List<Upgrade>();

	public UpgradeDatabase(){
		upgradeList.Add(new Upgrade(0,"Guild Building",10,5,2,"Members",500,500,0,5,"Increases member size"));
		upgradeList.Add(new Upgrade(1,"Storage",10,5,5,"Item Slots",300,300,0,2,"Increases storage size"));
		upgradeList.Add(new Upgrade(2,"Questboard",10,1,2,"Quest Slots",250,250,0,0,"Increases the questboard size"));
		//upgradeList.Add(new Upgrade(3,"Garden",5,0,2,"Increases the amount of materials you can grow at the guild"));
	}

	public int GetUpgradeListSize(){
		return upgradeList.Count;
	}


	public Upgrade GetUpgrade (int id)
	{
		foreach (Upgrade upgrade in upgradeList) {
			if (upgrade.id == id)
				return upgrade;
		}
		return null;
	}
}
