using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class AdventureScreen : MonoBehaviour {
	
	public Task task;
	public List<CharacterStatusDisplay> characterSlots;
	public List<CharacterStatusDisplay> enemySlots;
	public float slotSize;
	public Rect characterList;
	public Rect monsterList;
	private int time;
	private Adventure adventure;
	private bool tookANewStep = false;
	private int characterId; //team id of character that's supposed to fight
	private int enemyId; //team id of enemy that's supposed to fight

	// Use this for initialization
	void Start () {
		slotSize = characterList.height / 5;
	}
	
	public void StartAdventure(Task task){
		this.task = task;
		for (int i = 0; i < task.characters.Count; i++) {
			characterSlots [i].SetCharacter(task.characters [i]);
		}
		characterList.Set (characterList.x, characterList.y, characterList.width, slotSize * task.characters.Count);
		adventure = new Adventure (task.characters, task.area);
		time = 0;
	}

	public void Explore(){
		adventure.Explore (time, task);
		time += 1;
	}

	public void Gather(){
		adventure.Gather (task, time);
	}
}
