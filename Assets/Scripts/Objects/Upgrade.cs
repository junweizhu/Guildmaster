using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Upgrade
{
	public int id;
	public string name;
	public int maxLevel;
	public string description;
	public string sizeName;
	public int baseSize;
	public int sizePerLevel;
	public int baseUpgradeCost;
	public int upgradeCostPerLevel;
	public int baseDailyCost;
	public int dailyCostPerLevel;
	public Dictionary<int,int> baseMaterialCost;
	public Dictionary<int,int> materialCostPerLevel;

	//public Dictionary<int,Dictionary<int,int>> requiredMaterials= new Dictionary<int,Dictionary<int,int>>();

	public Upgrade (int id, string name, int maxlevel, int baseSize, int sizePerLevel, string sizeName, int baseUpgradeCost, int upgradeCostPerLevel, int baseDailyCost, int dailyCostPerLevel, string description="")
	{
		this.id = id;
		this.name = name;
		this.maxLevel = maxlevel;
		this.description = description;
		this.baseSize = baseSize;
		this.sizePerLevel = sizePerLevel;
		this.sizeName = sizeName;
		this.baseUpgradeCost = baseUpgradeCost;
		this.upgradeCostPerLevel = upgradeCostPerLevel;
		this.baseDailyCost = baseDailyCost;
		this.dailyCostPerLevel = dailyCostPerLevel;
	}

	public int MaxSize (int level)
	{
		return baseSize + sizePerLevel * level;
	}

	public int UpgradeCost (int level)
	{
		return baseUpgradeCost + upgradeCostPerLevel * level;
	}

	public int DailyCost (int level)
	{
		return baseDailyCost + dailyCostPerLevel * level;
	}

	public string MaterialCostString (int level)
	{
		string materials = "";
		Dictionary<int,int> materialCost = MaterialCost (level);
		if (materialCost.Count > 0) {
			foreach (KeyValuePair<int,int> material in materialCost) {
				materials += Database.items.FindItem (material.Key).name + ": " + material.Value.ToString () + "\n";
			}
		}
		if (materials==""){
			materials=Database.strings.GetString("NoMats");
		}
		return materials;
	}

	public Dictionary<int,int> MaterialCost (int level)
	{
		Dictionary<int,int> totalMatCost;
		if (baseMaterialCost!=null && baseMaterialCost.Count>0){
			totalMatCost= new Dictionary<int, int> (baseMaterialCost);
		} else{
			totalMatCost= new Dictionary<int, int> ();
		}
		if (materialCostPerLevel!=null && materialCostPerLevel.Count > 0) {
			foreach (KeyValuePair<int,int> material in materialCostPerLevel) {
				if (totalMatCost.ContainsKey (material.Key)) {
					totalMatCost [material.Key] = material.Value * level;
				} else {
					totalMatCost [material.Key] += material.Value * level;
				}
			}
		}
		return totalMatCost;
	}
}
