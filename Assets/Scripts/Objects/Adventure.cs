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
	private Task task;

	public Adventure (List<Character> characters, Area area, Task task)
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
		this.task = task;
	}
	

	public void Explore ()
	{
		status = "";
		actionList.Add ("We walk");
		if (Random.Range (0, 100) < 100 - Mathf.RoundToInt (100 * task.newStepsCount / area.size)||livingCharacters.Contains(Database.characters.GetCharacter(0))) {
			task.newStepsCount++;
			tookANewStep = true;
		}

		//if (Random.Range (0, 100) < 10) { //monster(s) encounter
		if (true){
			action = "Battle";
			status = "";
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
			if (Random.Range (0, 100) < 15 + Mathf.FloorToInt (combatLevel / 2)) {//ambushed or not?
				status = "Pre-emptive";
				actionList.Add ("But they haven't noticed us yet");
				for (int i = 0; i < livingCharacters.Count; i++) {
					task.GiveExp (livingCharacters [i], combatSkillId, 10);
				}
			} else if (Random.Range (0, 101) < 15 - Mathf.FloorToInt (combatLevel / 2)) {
				status = "Ambush";
				actionList.Add ("Ambushed!");
			} else {
				status = "";
			}
			if (livingMonsters.Alive ().Count == 0) {
				action = "Idle";
			}
		} else if (((tookANewStep && task.gatheringPointsFound < area.maxGatheringPoints) || !tookANewStep) && Random.Range (0, 101) < 6 + Mathf.RoundToInt (fieldSkillLevel / 2) + Database.myGuild.foundGatheringPoints [area.id] || (!Database.events.GetTrigger (107).activated && gatheringCount == 0)) { //Found a gathering point!
			if (Random.Range (0, 100) < Mathf.RoundToInt ((float)task.gatheringPointsFound * 100 / area.maxGatheringPoints) && !tookANewStep) {
				actionList.Add ("We already gathered here.");
			} else {
				actionList.Add ("Gathering Spot Found.");
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
					actionList.Add ("This gathering spot is new to us.");
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
			actionList.Add ("We found another area.");
			if (Database.myGuild.knownAreas.Contains (area.linkedAreas [RNG]) || task.foundNewArea.Contains (area.linkedAreas [RNG])) {
				actionList.Add ("We know this area.");
			} else {
				actionList.Add ("This is a new area.");
				task.foundNewArea.Add (area.linkedAreas [RNG]);
				actionList.Add ("We can now go to the "+Database.areas.FindArea(area.linkedAreas [RNG]).name);
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
	public void Rest(){
		livingCharacters.Rest ();
		actionList.Add ("Taking a rest");
	}
	public void Gather ()
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
				actionList.Add (livingCharacters [i].name + " looked around: Gathered " + gathered + " " + Database.items.FindItem (type).name);
			} else {
				actionList.Add (livingCharacters [i].name + " looked around: Found nothing of value.");
			}
			if (gatheringCount == 0) {
				actionList.Add ("There is nothing more of value here.");
				status = "";
				break;
			}
		}
	}

	public void RefreshBattleCharacterList(){
		livingCharacters = task.characters.Alive ();
		livingMonsters = battleMonster.Alive ();
	}

	public void PlayerTurn (Character attacker,List<Character> allies, List<Character> enemies, ref string action, ref Character enemy, ref Ability attack)
	{
		if (attacker.totalStats ["CurrentHealth"] > 0) {
			action = attacker.ChooseBattleAction (allies,this);
			if (action == "Fight") {
				enemy = enemies [Random.Range (0, enemies.Count)];

				attack = attacker.ChooseAttack (enemy);
				actionList.Add (attacker.name + " is going to attack " + enemy.name);
			}
		}
	}
	public void KillAllMonsters(){
		for (int i = 0; i < battleMonster.Count; i++) {
			battleMonster [i].totalStats ["CurrentHealth"] = 0;
		}
	}
	public void Fight(Character attacker,Ability attack, Character defender, Ability counter){
		attacker.totalStats ["CurrentMana"] -= attack.manaCost;
		defender.totalStats ["CurrentMana"] -= counter.manaCost;
		attacker.Attack (defender,attack,this);
		AttackSkillExpGain(attacker,attack.element,3);
		if (defender.canAttack&&defender.totalStats ["CurrentHealth"] > 0) {
			defender.Attack (attacker,counter,this);
			AttackSkillExpGain(defender,counter.skill,3);
			if (attacker.canAttack&&CanAttack (attacker.totalStats ["CurrentHealth"], attacker.totalStats ["Speed"], defender.totalStats ["Speed"])) {
				attacker.Attack (defender,attack,this);
				AttackSkillExpGain(attacker,attack.element,3);
			} else if (defender.canAttack&&attacker.totalStats ["CurrentHealth"] > 0 && CanAttack (defender.totalStats ["CurrentHealth"], defender.totalStats ["Speed"], attacker.totalStats ["Speed"])) {
				defender.Attack (attacker,counter,this);
				AttackSkillExpGain(defender,counter.element,3);
			}
		}
		if (attacker.isEnemy){
			DistributeExp (defender, attacker);
			task.GiveExp(defender,task.combatSkillId,3);
		} else{
			DistributeExp (attacker, defender);
			task.GiveExp(attacker,task.combatSkillId,3);
		}
		livingCharacters = livingCharacters.Alive ();
		livingMonsters = livingMonsters.Alive ();
	}

	public void AttackSkillExpGain(Character character, string skill,int amount){
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

	public void DistributeExp (Character character, Character monster)
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
