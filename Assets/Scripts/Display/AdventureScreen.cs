using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AdventureScreen : MonoBehaviour
{
	
	public Task task;
	public List<CharacterStatusDisplay> characterSlots;
	public List<CharacterStatusDisplay> enemySlots;

	public Transform characterList;
	public Transform monsterList;
	private int time;
	private Adventure adventure;
	private int characterId;
	//team id of character that's supposed to fight
	private int enemyId;
	//team id of enemy that's supposed to fight
	private Character character;
	private Character ally;
	private Character enemy;
	private Ability attack;
	private Ability counter;
	private CanvasGroup canvasGroup;
	private List<string> choices = new List<string> ();
	private List<Ability> abilityChoices;
	private List<int> itemSlotChoices;
	private string turn = ""; //player or enemy turn
	private string phase = ""; //for players only: Choose ability, item or enemy.
	private string action;
	private int turnsUntilTimeIncrease;
	private string choice="";
	public int slotSize;
	public Text clock;
	public Text progress;
	public Text inventory;
	private Dictionary<int,List<int>> items=new Dictionary<int, List<int>>();



	// Use this for initialization
	void Start ()
	{
		slotSize=150;
		canvasGroup = GetComponent<CanvasGroup> ();
		Debug.Log (slotSize);
	}

	public void StartAdventure (Task task)
	{
		this.task = task;
		for (int i = 0; i < task.characters.Count; i++) {
			characterSlots [i].SetCharacter (task.characters [i]);
		}
		SetListSize(characterList, task.characters.Count);
		adventure = new Adventure (task.characters, task.area, task);
		time = 0;
		task.adventure = adventure;
		task.success = true;
		Database.game.screenDisplay = "Adventure";
		Database.game.dialogueScreen.adventure = true;
		Database.game.dialogueScreen.ShowAdventureText ("You entered the " + adventure.area.name);
		UpdateText ();
	}
	void SetListSize(Transform list,int count){
		list.GetComponent<RectTransform> ().offsetMin = new Vector2(0, slotSize*(5-count)/2);
		list.GetComponent<RectTransform>().offsetMax= new Vector2(0, -slotSize*(5-count)/2);
	}
	void UpdateText(){
		int hourSpent=Mathf.FloorToInt((float)time/6);
		int min = (time - (hourSpent * 6)) * 10;

		int hour = hourSpent + 8;
		if (hour > 23) {
			hour -= 24;
		}
		clock.text = (hourSpent + 8).ToString ("00") + ":"+min.ToString ("00");
		progress.text = (100 * task.newStepsCount / task.area.size).ToString ()+" %";
		inventory.text = adventure.inventorySpace.ToString ();
		for (int i = 0; i < enemySlots.Count; i++) {
			enemySlots [i].UpdateCharacter ();
		}
	}
	public void PickChoice ()
	{
		UpdateText ();
		choices.Clear ();
		if (adventure.action == "Idle" || adventure.action == "Gathering") {
			if (phase == "Choose Item") {
				for (int i = 0; i < adventure.livingCharacters.Count; i++) {
					items [i] = adventure.livingCharacters[i].GetConsumables();
					for (int j = 0; j < items [i].Count; j++) {
						int itemId = adventure.livingCharacters [i].equipment [items [i] [j]].itemId;
						int itemDurability = adventure.livingCharacters [i].equipment [items [i] [j]].durability;
						string characterName = adventure.livingCharacters [i].name;
						choices.Add(Database.items.FindItem(itemId).name+" "+itemDurability.ToString()+" ("+characterName+")");
					}
					choices.Add ("Back");
				}
			} else{
				choices.Add ("Explore");
				if (adventure.status == "Found GatheringSpot") {
					choices.Add ("Gather");
				}
				choices.Add ("Rest");
				choices.Add ("Items");
				choices.Add ("Return");
			} 
		}
		if (adventure.action == "Battle") {
			SetListSize(monsterList, adventure.livingMonsters.Count);
			if (adventure.status == "Pre-emptive") {
				choices.Add ("Attack");
				choices.Add ("Ignore");
				choices.Add ("Return");
			} else {
				if (phase == "Choose Action") {
					if (character.HasConsumables()) {
						choices.Add ("Ability");
						choices.Add ("Item");
					} else {
						phase = "Choose Ability";
					}
					adventure.actionList.Add (character.name + "'s turn");
				}
				if (phase == "Choose Ability") {
					if (turn == "Enemy Turn") {
						
						abilityChoices = character.GetUsableAbilities (false, attack.range);
					} else {
						abilityChoices = character.GetUsableAbilities (true,0,true);
					}
					if (abilityChoices.Count > 0) {
						for (int i = 0; i < abilityChoices.Count; i++) {
							choices.Add (abilityChoices [i].name);
						}
					}
					if (turn == "Player Turn") {
						choices.Add ("Back");
					}
				} else if (phase == "Choose Item") {
					itemSlotChoices = character.GetConsumables ();
					if (itemSlotChoices.Count > 0) {
						for (int i = 0; i < itemSlotChoices.Count; i++) {
							Item item = Database.items.FindItem (character.tempEquipment [itemSlotChoices [i]].itemId);
							choices.Add (item.name);
						}
						choices.Add ("Back");
					}
				} else {
					if (phase == "Choose Ally") {
						for (int i = 0; i < adventure.livingCharacters.Count; i++) {
							choices.Add (adventure.livingCharacters [i].name);
						}
						choices.Add ("Back");
					}
					else if (phase == "Choose Enemy") {
						Debug.Log (adventure.livingMonsters.Count);
						for (int i = 0; i < adventure.livingMonsters.Count; i++) {
							choices.Add (adventure.livingMonsters [i].name);
						}
						choices.Add ("Back");
					}
				}
			}
		}
		if (adventure.action == "Wipeout") {
			choices.Add ("Return");
		}
		Database.game.choiceScreen.PresentChoice (choices, "Adventure");
	}

	public void Continue (int id)
	{
		choice = choices [id];

		Debug.Log (choice+ " "+turn+" "+phase);

		if (adventure.action == "Idle" || adventure.action == "Gathering") {
			if (phase == "Choose Item") {
				UseItem (id);
				phase = "";
			} else {
				if (choice == "Explore") {
					adventure.Explore ();
				}
				if (choice == "Gather") {
					adventure.Gather ();
				}
				if (choice == "Rest") {
					adventure.Rest ();
				}
				if (choice == "Return") {
					Finish ();
				}
				if (choice == "Items") {
					if (TeamHasItems ()) {
						phase = "Choose Item";
						PickChoice ();
					} else {
						adventure.actionList.Add ("Nobody has any items to use.");
					}

				} else {
					time += 1;
				}
			}
		}
		if (adventure.action == "Wipeout") {
			task.success = false;
			Finish ();
		}
		if (adventure.action == "Battle") {
			for (int i = 0; i < adventure.battleMonster.Count; i++) {
				enemySlots [i].SetCharacter (adventure.battleMonster [i]);
			}
			SetListSize(monsterList, adventure.livingMonsters.Count);
			if (adventure.status == "Ambush") {
				turn = "Enemy Turn";
				enemyId = 0;
				EnemyTurn ();
				phase = "Choose Ability";
				turnsUntilTimeIncrease = 1;
			} else if (adventure.status == "Pre-emptive") {
				if (choice == "Ignore") {
					adventure.action = "Idle";
					adventure.status = "";
					adventure.KillAllMonsters ();
					UpdateText ();
					adventure.actionList.Add ("Ignored the monsters.");
				} else if (choice == "Attack") {
					turnsUntilTimeIncrease = 1;
					PlayerTurn ();
					adventure.status = "";
				} else if (action == "Return") {
					Finish ();
				}
			} else if (adventure.status == ""&&phase=="") {
				turnsUntilTimeIncrease = 2;
				PlayerTurn ();
			} else{
				if (phase == "Choose Action") {
					phase = "Choose " + choice;
					PickChoice ();
				} else if (phase == "Choose Ability") {
					if (turn == "Player Turn") {
						if (choice == "Back") {
							phase = "Choose Action";
						} else {
							attack = abilityChoices [id];
							phase = "Choose Enemy";
						}
					} else {
						Debug.Log ("Test");
						counter = abilityChoices [id];
						adventure.Fight (enemy, attack, character, counter);
						EnemyTurn ();
					}
				} else if (phase == "Choose Item") {
					if (choice == "Back") {
						phase = "Choose Action";
					} else {
						character.UseConsumable (itemSlotChoices [id], adventure);
						phase = "Next Character";
					}
				} else if (phase == "Choose Enemy") {
					if (choice == "Back") {
						phase = "Choose Ability";
					} else {
						enemyId = id;
						enemy = adventure.livingMonsters [id];
						counter = enemy.ChooseCounterAttack (attack.range, character);
						adventure.Fight (character, attack, enemy, counter);
						phase = "Next Character";
					}
				} else if (phase == "Choose Ally") {
					if (choice == "Back") {
						phase = "Choose Ability";
					} else {
						ally = adventure.livingCharacters [id];
						if (attack.element == "Healing") {
							character.Heal (ally,adventure);
							phase = "Next Character";
						}
					}
				}
				if (phase == "Next Character") {
					characterId++;
					phase = "Choose Action";
					if (adventure.livingCharacters.Count <= characterId) {
						NextTurn ();
					} else {
						character = adventure.livingCharacters [characterId];
						adventure.actionList.Add (character.name + "'s turn.");
					}
				}
				if (adventure.livingMonsters.Alive ().Count == 0) {
					adventure.actionList.Add ("The battle is won!");
					phase = "";
					adventure.action = "Idle";
					time++;
				} else if (adventure.livingCharacters.Alive ().Count == 0) {
					adventure.actionList.Add ("Party is wiped out.");
					phase="Wipeout";
					adventure.action="Wipeout";
				}
			}

			if (phase == "Choose Action") {
				adventure.actionList.Add ("What will you do?");
			} else if (phase == "Choose Ability") {
				adventure.actionList.Add ("Choose an ability to use");
			} else if (phase == "Choose Item") {
				adventure.actionList.Add ("Choose an item to use");
			} else if (phase == "Choose Enemy") {
				adventure.actionList.Add ("Choose your target");
			} else if (phase == "Choose Ally") {
				adventure.actionList.Add ("Who will you use it on?");
			}
		}
		Database.game.dialogueScreen.ShowAdventureText (adventure.actionList);
		adventure.actionList.Clear ();
	}

	public bool TeamHasItems(){
		for (int i = 0; i < task.characters.Count; i++) {
			if (!task.characters [i].HasConsumables ()) {
				return false;
			}
		}
		return true;
	}
	public void UseItem(int id){
		int count = 0;
		for (int i = 0; i < adventure.livingCharacters.Count; i++) {
			for (int j = 0; j < items [i].Count; j++) {
				if (count == id) {
					adventure.livingCharacters [i].UseConsumable (items[i][j], adventure);
					return;
				} 
				count++;
			}
		}
	}
	public void EnemyTurn ()
	{
		if (adventure.livingMonsters.Count > enemyId) {
			enemy = adventure.livingMonsters [enemyId];
			adventure.PlayerTurn (enemy, adventure.livingMonsters, adventure.livingCharacters, ref action, ref character, ref attack);
			enemyId++;
			if (action != "Fight") {
				EnemyTurn ();
			} else {
				phase="Choose Ability";
			}
		} else {
			NextTurn ();
		}
	}

	public void NextTurn(){
		turnsUntilTimeIncrease--;
		if (adventure.livingMonsters.Alive ().Count > 0) {
			if (turnsUntilTimeIncrease == 0) {
				turnsUntilTimeIncrease = 2;
				time++;
				PlayerTurn ();
			} else {
				adventure.actionList.Add ("Enemy Turn");
				turn = "Enemy Turn";
				enemyId = 0;
				EnemyTurn ();
				phase = "Choose Ability";
			}
		}
	}
	public void PlayerTurn(){
		adventure.actionList.Add ("Player Turn");
		turn = "Player Turn";
		phase = "Choose Action";
		characterId = 0;
		character = adventure.livingCharacters [characterId];
	}

	public void Finish ()
	{
		Database.game.FinishManualTask (task);
		Database.game.dialogueScreen.adventure = false;
	}
}
