using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class ExtensionMethods
{

	public static void SetSize (this Transform list, int count, int slotSize)
	{
		list.GetComponent<RectTransform> ().offsetMax = new Vector2 (list.GetComponent<RectTransform> ().offsetMax.x, 0);
		list.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, count * slotSize);
		if (list.GetComponent<RectTransform> ().rect.height > list.parent.GetComponent<RectTransform> ().rect.height)
			list.GetComponent<ScrollRect> ().vertical = true;
		else
			list.GetComponent<ScrollRect> ().vertical = false;
		
	}

	public static void SetSize (this Transform list, ScrollRect rect, int count, int slotSize)
	{
		list.GetComponent<RectTransform> ().offsetMax = new Vector2 (list.GetComponent<RectTransform> ().offsetMax.x, 0);
		list.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, count * slotSize);
		if (list.GetComponent<RectTransform> ().rect.height > list.parent.GetComponent<RectTransform> ().rect.height)
			rect.vertical = true;
		else
			rect.vertical = false;
	}

	public static void GeneratePrefab (this List<GameObject> prefablist, int index, GameObject prefab, string nameprefix, Transform parent)
	{
		if (index + 1 > prefablist.Count) {
			prefablist.Add (GameObject.Instantiate (prefab) as GameObject);
			prefablist [index].transform.SetParent (parent);

			prefablist [index].ResetTransform ();
			prefablist [index].name = nameprefix + " " + index.ToString ();
		} else if (prefablist [index].activeSelf == false) {
			prefablist [index].SetActive (true);
		}


	}

	public static void ResetTransform (this GameObject slot)
	{
		slot.transform.localPosition = new Vector3 (0, 0, 0);
		slot.transform.localScale = new Vector3 (1, 1, 1);
	}

	public static Monster GenerateMonster (this Dictionary<Monster,int> monsterlist, int RNG, int averagelevel)
	{
		Monster selectedmonster = null;
		int appearanceRate = 0;
		int level = 1;
		if (averagelevel - 2 > 1) {
			level = Random.Range (averagelevel - 2, averagelevel + 2);
		} else {
			level = Random.Range (1, averagelevel + 2);
		}
		foreach (KeyValuePair<Monster,int> monster in monsterlist) {

			if (monster.Value >= RNG) {
				if (selectedmonster == null) {
					selectedmonster = monster.Key;
					appearanceRate = monster.Value;
				} else if (monster.Value < appearanceRate) {
					selectedmonster = monster.Key;
					appearanceRate = monster.Value;
				}
			}
		}
		return new Monster (selectedmonster, level);
	}

	public static void TurnStart (this List<Member> members, List<Monster> monsters,int turn)
	{
		if (monsters != null) {
			Debug.Log ("Player turn" + turn.ToString());
			foreach (Member member in members) {
				if (member.stats ["CurrentHealth"] > 0) {
					if (member.stats ["CurrentHealth"] / member.stats ["MaxHealth"] * 100 < 30 && member.HasHealingItems()) {
						member.UseHealingItem();
					} else{
						Monster target = monsters.Alive () [Random.Range (0, monsters.Alive ().Count)];
						member.Attack (target);
						if (target.stats ["CurrentHealth"] > 0) {
							target.Attack (member);
							if (member.stats ["CurrentHealth"] > 0) { 
								if (member.stats ["Agility"] >= Mathf.RoundToInt (target.stats ["Agility"] * 1.5f)&&member.stats ["Agility"] >= Mathf.RoundToInt (target.stats ["Agility"]+4)) {
									member.Attack (target);
								} else if (target.stats ["CurrentHealth"] > 0 && target.stats ["Agility"] >= Mathf.RoundToInt (member.stats ["Agility"] * 1.5f)&& target.stats ["Agility"] >= Mathf.RoundToInt (member.stats ["Agility"] +4)) {
									target.Attack (member);
								}
							}
						}
						if (target.stats ["CurrentHealth"] <= 0) {
							Debug.Log (target.name + " dies");
							member.GiveExp (null, 30 + (target.level - member.level) * 3);
						} else {
							member.GiveExp (null, 10 + (target.level - member.level) * 2);
						}
						if (member.stats ["CurrentHealth"] <= 0) {
							Debug.Log (target.name + " is knocked out");
						}
					}
					if (monsters.Alive ().Count == 0 || members.Alive ().Count == 0) {
						return;
					}
				}
			}
		}
	}

	public static void TurnStart (this List<Monster> monsters, List<Member> members,int turn)
	{

		if (monsters != null) {
			Debug.Log ("Enemy turn." + turn.ToString());
			foreach (Monster monster in monsters) {
				if (monster.stats ["CurrentHealth"] > 0) {
					Member target = members.Alive() [Random.Range (0, members.Alive().Count)];
					monster.Attack (target);
					if (target.stats ["CurrentHealth"] > 0) {
						target.Attack (monster);
						if (monster.stats ["CurrentHealth"] > 0) { 
							if (monster.stats ["Agility"] >= Mathf.RoundToInt (target.stats ["Agility"] * 1.5f)&&monster.stats ["Agility"] >= Mathf.RoundToInt (target.stats ["Agility"] +4)) {
								monster.Attack (target);
							} else if (target.stats ["CurrentHealth"] > 0 && target.stats ["Agility"] >= Mathf.RoundToInt (monster.stats ["Agility"] * 1.5f)&&target.stats ["Agility"] >= Mathf.RoundToInt (monster.stats ["Agility"]+4)) {
								target.Attack (monster);
							}
						}
					}
					if (monster.stats ["CurrentHealth"] <= 0) {
						target.GiveExp (null, 30 + (monster.level - target.level) * 3);
						Debug.Log (monster.name + " dies");
					} else {
						target.GiveExp (null, 10 + (monster.level - target.level) * 2);
					}
					if (target.stats ["CurrentHealth"] <= 0) {
						Debug.Log (target.name + " is knocked out");
					}
					if (monsters.Alive ().Count == 0 || members.Alive ().Count == 0) {
						return;
					}
				}
			}
		}
	}

	public static List<Member> Alive (this List<Member> allmembers)
	{
		List<Member> livingmembers = new List<Member> ();
		if (allmembers.Count > 0) {
			foreach (Member member in allmembers) {
				if (member.stats ["CurrentHealth"] > 0) {
					livingmembers.Add (member);
				}
			}
		}
		return livingmembers;
	}

	public static List<Monster> Alive (this List<Monster> allmonsters)
	{
		List<Monster> livingmonsters = new List<Monster> ();
		if (allmonsters.Count > 0) {
			foreach (Monster monster in allmonsters) {
				if (monster.stats ["CurrentHealth"] > 0) {
					livingmonsters.Add (monster);
				}
			}
		}
		return livingmonsters;
	}

	public static void SortInventory(this List<InventorySlot> inventory){
		inventory=inventory.OrderByDescending(slot=>slot.quantity).ToList();
	}
}
