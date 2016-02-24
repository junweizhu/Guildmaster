using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shop
{
	public int id;
	public string name;
	public string description;
	public bool useSkill;
	public Dictionary<int,List<int>> itemList = new Dictionary<int,List<int>> ();
	public Dictionary<int,List<int>> skillList = new Dictionary<int,List<int>> ();
	public Dictionary<int,List<int>> abilityList = new Dictionary<int,List<int>> ();
	public Dictionary<int,int> costModifier = new Dictionary<int,int> ();

	public Shop (int id, string name, string description)
	{
		this.id = id;
		this.name = name;
		this.description = description;
	}

	public void AddItem (int level, int itemId)
	{
		if (!itemList.ContainsKey (level))
			itemList [level] = new List<int> ();
		itemList [level].Add (itemId);
	}

	public void AddAbility (int level, int abilityId)
	{
		if (!abilityList.ContainsKey (level))
			abilityList [level] = new List<int> ();
		abilityList [level].Add (abilityId);
		useSkill = true;
	}

	public void AddSkill (int skillId, int maxLevelMod, int maxLevel, int costModifier)
	{
		if (!skillList.ContainsKey (skillId))
			skillList [skillId] = new List<int> ();
		for (int i=0; i<Mathf.CeilToInt(maxLevel/maxLevelMod); i++) {
			skillList [skillId].Add ((i + 1) * maxLevelMod);
		}
		this.costModifier [skillId] = costModifier;
		useSkill = true;
	}

	public List<int> GetShopList (int level)
	{
		List<int> shoppinglist = new List<int> ();
		if (useSkill) {
			foreach (int list in skillList.Keys) {
				shoppinglist.Add (list);
			}
		} else {
			foreach (KeyValuePair<int,List<int>> list in itemList) {
				if (level >= list.Key) {
					foreach (int item in list.Value) {
						shoppinglist.Add (item);
					}
				}
			}
		}
		return shoppinglist;
	}

	public List<int> GetAbilityList (int level)
	{
		List<int> shoppinglist = new List<int> ();
		foreach (KeyValuePair<int,List<int>> list in abilityList) {
			if (level >= list.Key) {
				foreach (int item in list.Value) {
					shoppinglist.Add (item);
				}
			}
		}
		return shoppinglist;
	}

	public int GetSize (int level)
	{
		int count = 0;
		if (useSkill) {
			count = skillList.Count;
			foreach (KeyValuePair<int,List<int>> list in abilityList) {
				if (level >= list.Key) {
					count += list.Value.Count;
				}
			}
		} else {
			foreach (KeyValuePair<int,List<int>> list in itemList) {
				if (level >= list.Key) {
					count += list.Value.Count;
				}
			}
		}
		return count;
	}
}

