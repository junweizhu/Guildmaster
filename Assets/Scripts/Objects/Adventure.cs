using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Adventure
{
	bool tookANewStep;
	public string action;
	public string status;
	GatheringPoint gatheringPoint;
	public List<Character> battleMonster = new List<Character> ();
	Dictionary<Monster,int> monsters;
	int averageLevel = 0;
	public List<Character> livingMonsters;
	public List<Character> livingCharacters;
	public int inventorySpace;
	int fieldSkillLevel = 0;
	int combatLevel = 0;
	public int gatheringCount = 0;
	int newGatheringpointsFound = 0;
	public int weaponSkillId = 0;
	public int magicSkillId = 1;
	public int combatSkillId = 2;
	public int fieldSkillId = 3;
	public int socialSkillId = 4;
	public Area area;
	public List<string> actionList = new List<string> ();

	public Adventure (List<Character> characters, Area area)
	{
		action = "Idle";
		this.area = area;
		inventorySpace = 0;
		for (int i = 0; i < characters.Count; i++) {
			inventorySpace += characters [i].totalStats ["CarrySize"];
		}

		monsters = Database.monsters.GetAreaMonsters (area.name);

		for (int i = 0; i < characters.Count; i++) {
			averageLevel += characters [i].level;
			if (characters [i].skillLevel [fieldSkillId] > fieldSkillLevel) {
				fieldSkillLevel = characters [i].skillLevel [fieldSkillId];
			}
			if (characters [i].skillLevel [combatSkillId] < combatLevel) {
				combatLevel = characters [i].skillLevel [combatSkillId];
			}
		}
		averageLevel /= characters.Count;
		if (averageLevel < area.level) {
			averageLevel = area.level;

		}
		tookANewStep = false;
		livingCharacters = characters.Alive ();
		livingMonsters = battleMonster.Alive ();
	}
	


	public void Explore (int time, Task task)
	{
		actionList.Add ("Exploring");
		if (Random.Range (0, 100) < 100 - Mathf.RoundToInt (100 * task.newStepsCount / area.size)) {
			task.newStepsCount++;
			tookANewStep = true;
		}

		if (Random.Range (0, 101) < 6) { //monster(s) encounter
			action = "Battle";
			bool generateMonster = true;
			battleMonster.Clear ();
			while (generateMonster) {
				int RNG = Random.Range (0, 101);
				battleMonster.Add (monsters.GenerateCharacter (RNG, averageLevel));
				if (Random.Range (0, 100) > battleMonster.Count * 30) {
					generateMonster = true;
				} else {
					generateMonster = false;
				}
			}
			livingMonsters = battleMonster;
			actionList.Add ("Monster Encounter! " + battleMonster.Count.ToString () + " monsters");
			if (Random.Range (0, 101) < 15 + Mathf.FloorToInt (combatLevel / 2)) {//ambushed or not?
				status = "Pre-emptive";
				actionList.Add ("Pre-emptive battle" + time);
				for (int i = 0; i < livingCharacters.Count; i++) {
					task.GiveExp (livingCharacters [i], combatSkillId, 10);
				}
			} else if (Random.Range (0, 101) < 15 - Mathf.FloorToInt (combatLevel / 2)) {
				status = "Ambush";
				actionList.Add ("Ambushed!" + time);
			} else {
				status = "";
			}
			if (livingMonsters.Alive ().Count == 0) {
				action = "Idle";
			}
		} else if (((tookANewStep && task.gatheringPointsFound < area.maxGatheringPoints) || !tookANewStep) && Random.Range (0, 101) < 6 + Mathf.RoundToInt (fieldSkillLevel / 2) + Database.myGuild.foundGatheringPoints [area.id] || (!Database.events.GetTrigger (107).activated && gatheringCount == 0)) { //Found a gathering point!
			actionList.Add ("Gathering Point Found " + time.ToString ());
			if (Random.Range (0, 100) < Mathf.RoundToInt ((float)task.gatheringPointsFound * 100 / area.maxGatheringPoints) || !tookANewStep) {
				actionList.Add ("This gathering point was already discovered today ");
			} else {
				gatheringPoint = area.FindRandomGatheringPoint ();
				Debug.Log (gatheringPoint);
				task.gatheringPointsFound++;
				if (gatheringPoint != null) {
					gatheringCount = Random.Range (1, gatheringPoint.maxQuantity);
				}	
				if (Random.Range (0, 100) < 100 - Mathf.RoundToInt ((float)(Database.myGuild.foundGatheringPoints [area.id] - newGatheringpointsFound) * 100 / area.maxGatheringPoints)) {
					task.guildExp += 1 + 3 * area.level;
					for (int i = 0; i < livingCharacters.Count; i++) {
						task.GiveExp (livingCharacters [i], fieldSkillId, 5);
					}
					Database.myGuild.foundGatheringPoints [area.id]++;
					newGatheringpointsFound++;
					actionList.Add ("It's a newly discovered gatheringpoint! ");
				} else {
					task.guildExp += 2 * area.level;
					for (int i = 0; i < livingCharacters.Count; i++) {
						task.GiveExp (livingCharacters [i], fieldSkillId, 3);
					}
				}
				status = "Found GatheringSpot";
			}
		} else if (tookANewStep && Random.Range (0, 101) < Mathf.RoundToInt (100 * task.newStepsCount / area.size) && area.linkedAreas != null) {//Found a different area
			int RNG = Random.Range (0, area.linkedAreas.Count);
			actionList.Add ("We found a link to another area.");
			if (Database.myGuild.knownAreas.Contains (area.linkedAreas [RNG]) || task.foundNewArea.Contains (area.linkedAreas [RNG])) {
				actionList.Add ("We know this area.");
			} else {
				actionList.Add ("This is a new area.");
				task.foundNewArea.Add (area.linkedAreas [RNG]);
				task.guildExp += 3 + 3 * area.level;
				for (int i = 0; i < livingCharacters.Count; i++) {
					task.GiveExp (livingCharacters [i], fieldSkillId, 10);
				}
			}
		} else {
			task.guildExp += 1 * area.level;
			for (int i = 0; i < livingCharacters.Count; i++) {
				task.GiveExp (livingCharacters [i], fieldSkillId, 1);
			}
		}
		tookANewStep = false;
	}

	public void CheckBattleIsFinished ()
	{
		if (livingMonsters.Alive ().Count == 0) {
			action = "Idle";
		}
	}

	public void Gather (Task task, int time)
	{
		for (int i = 0; i < livingCharacters.Count; i++) {
			if (Random.Range (0, 101) < 25 + livingCharacters [i].skillLevel [fieldSkillId]) {
				int maxAmount = 1 + Mathf.FloorToInt (livingCharacters [i].skillLevel [fieldSkillId] / 10);
				int gathered = 1;
				if (maxAmount > 1) {
					gathered = Random.Range (1, maxAmount);
				}
				if (gathered > inventorySpace) {
					gathered = inventorySpace;
				} 
				if (gathered > gatheringCount) {
					gathered = gatheringCount;
				}
				inventorySpace -= gathered;
				task.GiveExp (livingCharacters [i], fieldSkillId, gathered * 2);
				int type = gatheringPoint.FindRandomItem ().id;
				if (task.itemList.ContainsKey (type)) {
					task.itemList [type] += gathered;
				} else {
					task.itemList [type] = gathered;
				}
				gatheringCount -= gathered;
				if (inventorySpace == 0) {
					break;
				}
				if (gatheringCount == 0) {
					actionList.Add ("Finished Gathering ");
					break;
				}
				actionList.Add (livingCharacters [i].name + " gathered " + gathered + " " + Database.items.FindItem (type).name);
			} else {
				actionList.Add (livingCharacters [i].name + " failed to gather anything");
			}
		}
		if (time >= 36) {
			status = "Finished";
		}

	}

	public void Fight(Character attacker,Ability attack, Character defender, Ability counter,Task task){
		attacker.Attack (defender,attack,this);
		AttackSkillExpGain(attacker,attack.element,3,task);
		if (defender.canAttack&&defender.totalStats ["CurrentHealth"] > 0) {
			defender.Attack (attacker,counter,this);
			AttackSkillExpGain(defender,counter.skill,3,task);
			if (attacker.canAttack&&CanAttack (attacker.totalStats ["CurrentHealth"], attacker.totalStats ["Speed"], defender.totalStats ["Speed"])) {
				attacker.Attack (defender,attack,this);
				AttackSkillExpGain(attacker,attack.element,3,task);
			} else if (defender.canAttack&&attacker.totalStats ["CurrentHealth"] > 0 && CanAttack (defender.totalStats ["CurrentHealth"], defender.totalStats ["Speed"], attacker.totalStats ["Speed"])) {
				defender.Attack (attacker,counter,this);
				AttackSkillExpGain(defender,counter.element,3,task);
			}
		}
		if (attacker.isEnemy){
			DistributeExp (defender, attacker, task);
			task.GiveExp(defender,task.combatSkillId,3);
		} else{
			DistributeExp (attacker, defender, task);
			task.GiveExp(attacker,task.combatSkillId,3);
		}
	}

	public void AttackSkillExpGain(Character character, string skill,int amount,Task task){
		if (!character.isEnemy){
			if (skill=="Physical"){
				task.GiveExp(character,task.weaponSkillId,amount);
			} else{
				task.GiveExp(character,task.magicSkillId,amount);
			}
			if (skill == "Mixed") {
				task.GiveExp (character, task.weaponSkillId, amount);
			}
		}
	}

	public bool CanAttack (int currentHealth, int firstUnitSpeed, int secondUnitSpeed)
	{
		if (currentHealth > 0) {
			if (firstUnitSpeed >= ExtensionMethods.Calculate (secondUnitSpeed, 1.5f) && firstUnitSpeed >= secondUnitSpeed + 4) {
				return true;
			}
		}
		return false;
	}

	public void DistributeExp (Character character, Character monster, Task task)
	{
		int expGain=1;
		if (monster.totalStats ["CurrentHealth"] <= 0) {
			actionList.Add (monster.name + " dies.");
			expGain= 10 + (monster.level - character.level) * 3;
			task.monstercount++;
		} else if (character.didDamage) {
			expGain= 5 + (monster.level - character.level) * 2;
		} 
		if (character.totalStats ["CurrentHealth"] <= 0) {
			actionList.Add (character.name + " is knocked out.");
		} else {
			Debug.Log (character.name+ " has " +character.totalStats ["CurrentHealth"]+ " health.");
		}
		if (expGain<1){
			expGain=1;
		}
		task.GiveExp (character, 99, expGain);
		character.didDamage = false;
	}
}
