using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class ExtensionMethods
{
	public static void SetShow (this CanvasGroup canvasgroup, bool show)
	{
		if (show) {
			canvasgroup.blocksRaycasts = true;
			canvasgroup.interactable = true;
			canvasgroup.alpha = 1;
		} else {
			canvasgroup.alpha = 0;
			canvasgroup.blocksRaycasts = false;
			canvasgroup.interactable = true;
		}

	}

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

	public static Character GenerateCharacter (this Dictionary<Monster,int> monsterlist, int RNG, int averagelevel)
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
		return new Character (selectedmonster, level);
	}

	public static void TurnStart (this List<Character> attackers, Task task, List<Character> defenders, int turn)
	{
		if (defenders != null) {
			if (attackers[0].isEnemy){
				Debug.Log ("Enemy turn." + turn.ToString ());
			} else{
				Debug.Log ("Player turn" + turn.ToString ());
			}
			foreach (Character attacker in attackers) {
				if (attacker.totalStats ["CurrentHealth"] > 0) {
					if ((float)attacker.totalStats ["CurrentHealth"] / attacker.totalStats ["MaxHealth"] * 100 < 30 && attacker.HasHealingItems ()) {
						attacker.UseHealingItem ();
					} else if(attackers.NeedsHealing(attacker)){
						attacker.Heal(attackers.GetCharacterToHeal(attacker));
					}else {
						Character defender = defenders [Random.Range (0, defenders.Count)];
						Ability attack=attacker.ChooseAttack();
						Ability counter=defender.ChooseCounterAttack(attack.range);
						attacker.Attack (defender,attack);
						if (defender.canAttack&&defender.totalStats ["CurrentHealth"] > 0) {
							defender.Attack (attacker,counter);
							if (attacker.canAttack&&CanAttack (attacker.totalStats ["CurrentHealth"], attacker.totalStats ["Speed"], defender.totalStats ["Speed"])) {
								attacker.Attack (defender,attack);
							} else if (defender.canAttack&&attacker.totalStats ["CurrentHealth"] > 0 && CanAttack (defender.totalStats ["CurrentHealth"], defender.totalStats ["Speed"], attacker.totalStats ["Speed"])) {
								defender.Attack (attacker,counter);
							}
						}
						if (attacker.isEnemy){
							DistributeExp (defender, attacker, task);
						} else{
							DistributeExp (attacker, defender, task);
						}
						if (defenders.Alive ().Count == 0 || attackers.Alive ().Count == 0) {
							return;
						}
					}
				}
				defenders = defenders.Alive ();
			}
		}
	}

	public static bool CanAttack (int currentHealth, int firstUnitSpeed, int secondUnitSpeed)
	{
		if (currentHealth > 0) {
			if (firstUnitSpeed >= Calculate (secondUnitSpeed, 1.5f) && firstUnitSpeed >= secondUnitSpeed + 4) {
				return true;
			}
		}
		return false;
	}

	public static void DistributeExp (Character character, Character monster, Task task)
	{
		if (monster.totalStats ["CurrentHealth"] <= 0) {
			Debug.Log (monster.name + " dies."+System.DateTime.Now.Millisecond.ToString());
			task.GiveExp (character, null, 30 + (monster.level - character.level) * 3);
		} else if (character.didDamage) {
			task.GiveExp (character, null, 10 + (monster.level - character.level) * 2);
		} else {
			task.GiveExp (character, null, 1);
		}
		if (character.totalStats ["CurrentHealth"] <= 0) {
			Debug.Log (character.name + " is knocked out "+character.totalStats ["CurrentHealth"].ToString()+" "+System.DateTime.Now.Millisecond.ToString());
		} else {
			Debug.Log (character.name+ " has " +character.totalStats ["CurrentHealth"]+ " health. "+System.DateTime.Now.Millisecond.ToString());
		}
		character.didDamage = false;
	}

	public static int GetInventorySpace (this List<Character> selectedcharacters)
	{
		int space = 0;
		foreach (Character character in selectedcharacters) {
			space += character.totalStats ["CarrySize"];
		}
		return space;
	}

	public static List<Character> Alive (this List<Character> allcharacters)
	{
		List<Character> livingcharacters = new List<Character> ();
		if (allcharacters.Count > 0) {
			foreach (Character character in allcharacters) {
				if (character.totalStats ["CurrentHealth"] > 0) {
					livingcharacters.Add (character);
				}
			}
		}
		return livingcharacters;
	}

	public static bool NeedsHealing(this List<Character> characters, Character attacker){
		if (attacker.HasHealingAbility()&&GetCharacterToHeal(characters,attacker)!=null){
			return true;
		}
		return false;

	}

	public static Character GetCharacterToHeal(this List<Character> characters, Character healer){
		foreach (Character character in characters) {
			if (character.totalStats ["CurrentHealth"] != 0 && character.totalStats ["MaxHealth"]-character.totalStats ["CurrentHealth"] >= Mathf.RoundToInt(healer.totalStats["MAttack"]/2)) {
				return character;
			}
		}
		return null;
	}
	public static bool IsHurt (this List<Character> characters)
	{
		foreach (Character character in characters) {
			if (character.totalStats ["CurrentHealth"] != 0 && (float)character.totalStats ["CurrentHealth"] * 100 / character.totalStats ["MaxHealth"] < 75) {
				return true;
			}
			Debug.Log((float)character.totalStats ["CurrentHealth"]* 100 / character.totalStats ["MaxHealth"] );
		}
		return false;
	}

	public static bool IsInjured (this List<Character> characters)
	{
		foreach (Character character in characters) {
			if (character.totalStats ["CurrentHealth"] <= 0) {
				return true;
			}
		}
		return false;
	}

	public static void Rest (this List<Character> characters)
	{
		foreach (Character character in characters) {
			if (character.totalStats ["CurrentHealth"] != character.totalStats ["MaxHealth"]) {
				character.Heal (10, "Percent", "Health");
				character.Heal (5, "Percent", "Mana");
			}
		}
	}
	public static void UpdateEquipmentStats(this Dictionary<string,int>equipmentStats,List<InventorySlot> equipment, ref int blockItem){
		List<string> stats=new List<string>(equipmentStats.Keys);
		foreach (string stat in stats){
			equipmentStats[stat]=0;
		}
		blockItem=0;
		foreach (InventorySlot equip in equipment) {
			if (equip.filled) {
				Item item = Database.items.FindItem (equip.itemId);
				if (item.type != "Consumable") {
					foreach (KeyValuePair<string,int> stat in item.stats) {
						if (equipmentStats.ContainsKey (stat.Key)) {
							equipmentStats [stat.Key] += stat.Value;
						}
					}
				}
				if (item.subType=="Shield"){
					blockItem=equip.id;
				}
			} else if (equip.id == 0) {
				equipmentStats ["Accuracy"] += 85;
				equipmentStats ["Evade"] += 15;
			}
		}
		if (equipment[blockItem].filled){
			equipmentStats["Block"]=Database.items.FindItem(equipment[blockItem].itemId).stats["Block"];
			equipmentStats["BlockChance"]=Database.items.FindItem(equipment[blockItem].itemId).stats["BlockChance"];
		}
	}
	public static void UpdateStats (this Dictionary<string,int> totalStats, Dictionary<string,int> baseStats,Dictionary<string,int> bonusStats,Dictionary<string,int>equipmentStats,string status)
	{
		foreach (KeyValuePair<string,int> stat in equipmentStats){
			if (!stat.Key.Contains("Current")){
				totalStats[stat.Key]=stat.Value;
			}
		}

		totalStats ["MaxHealth"] += baseStats ["Health"]+bonusStats["Health"];
		if (totalStats ["CurrentHealth"] > totalStats ["MaxHealth"]|| status=="Idle") {
			totalStats ["CurrentHealth"] = totalStats ["MaxHealth"];
		}
		totalStats ["MaxMana"] += baseStats ["Mana"]+bonusStats["Mana"];
		if (totalStats ["CurrentMana"] > totalStats ["MaxMana"]|| status=="Idle") {
			totalStats ["CurrentMana"] = totalStats ["MaxMana"];
		}
		totalStats ["PAttack"] += baseStats ["Strength"]+bonusStats ["Strength"];
		totalStats ["MAttack"] += baseStats ["Intelligence"]+bonusStats ["Intelligence"];
		totalStats ["Accuracy"] += baseStats ["Dexterity"]+bonusStats ["Dexterity"];
		totalStats ["Evade"] += baseStats ["Agility"]+bonusStats ["Agility"];
		totalStats ["BlockChance"] += baseStats ["Dexterity"]+bonusStats ["Dexterity"];
		totalStats ["Speed"] += baseStats ["Agility"]+bonusStats ["Agility"];
		if (totalStats ["Weight"] < baseStats ["Strength"]+bonusStats ["Strength"]&& totalStats.ContainsKey("CarrySize")) {
			totalStats ["CarrySize"]+=1;
		}
		if (totalStats ["Weight"] > baseStats ["Strength"]+bonusStats ["Strength"]) {
			totalStats ["Accuracy"] += (baseStats ["Strength"]+bonusStats ["Strength"] - totalStats ["Weight"]) * 5;
			totalStats ["Evade"] += (baseStats ["Strength"]+bonusStats ["Strength"] - totalStats ["Weight"]) * 4;
			totalStats ["BlockChance"] += (baseStats ["Strength"]+bonusStats ["Strength"] - totalStats ["Weight"]) * 3;
			totalStats ["Speed"] += baseStats ["Strength"]+bonusStats ["Strength"] - totalStats ["Weight"];
		}
	}

	public static int Calculate (int totalstat, float multiplier)
	{
		float result = totalstat * multiplier;
		if (result >= Mathf.FloorToInt (result) + 0.5f) {
			return Mathf.CeilToInt (result);
		} else {
			return Mathf.FloorToInt (result);
		}
	}
}
