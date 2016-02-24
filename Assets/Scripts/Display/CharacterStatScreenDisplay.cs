using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterStatScreenDisplay : MonoBehaviour
{
	public Text characterNumber;
	public Text characterName;
	public Text characterStatus;
	public Text characterGender;
	public Text characterLevel;
	public Text characterExp;
	public List<Text> characterStats;
	public GameObject inventorySlot;
	public Transform inventoryList;
	public List<GameObject> itemSlotList = new List<GameObject> ();
	public GameObject skillSlot;
	public Transform skillList;
	public List<GameObject> skillSlotList = new List<GameObject> ();
	public GameObject abilitySlot;
	public Transform abilityList;
	public List<GameObject> abilitySlotList = new List<GameObject> ();
	public Guild myGuild;
	public bool show;
	public string showSub;
	public string playerType;
	public Button nextButton;
	public Button prevButton;
	public GameObject giveButton;
	public GameObject recruitButton;
	public Color normalColor;
	public Color debuffColor;
	Character character = new Character ();

	// Update is called once per frame
	void Update ()
	{
		GetComponent<CanvasGroup> ().SetShow (show);
		if (showSub == "inventory") {
			ShowList (inventoryList);
		} else {
			HideList (inventoryList);
		}
		if (showSub == "skill") {
			ShowList (skillList);
		} else {
			HideList (skillList);
		}
		if (showSub == "ability"){
			ShowList (abilityList);
		} else {
			HideList (abilityList);
		}
	}

	private void ShowList (Transform list)
	{
		if (list.GetComponent<CanvasGroup> ().alpha == 0) {
			list.GetComponent<CanvasGroup> ().alpha = 1;
			list.GetComponent<CanvasGroup> ().blocksRaycasts = true;
			list.GetComponent<ScrollRect> ().normalizedPosition = new Vector2 (0, 1);
		}
	}

	private void HideList (Transform list)
	{
		if (list.GetComponent<CanvasGroup> ().alpha == 1) {
			list.GetComponent<CanvasGroup> ().alpha = 0;
			list.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		}
	}

	public void FillSlot (int id)
	{
		myGuild = Database.myGuild;
		if (playerType == "Character") {
			character = myGuild.GetCharacter (id);
			prevButton.interactable = (id > 1);
			nextButton.interactable = (id < myGuild.characterlist.Count);
			giveButton.SetActive (true);
			recruitButton.SetActive (false);
			characterNumber.text = character.guildnr.ToString ();
		} else if (playerType == "Recruit") {
			character = Database.characters.GetRecruitables () [id];
			prevButton.interactable = (id > 0);
			nextButton.interactable = (id < Database.characters.GetRecruitables ().Count - 1);
			giveButton.SetActive (false);
			recruitButton.SetActive (true);
			characterNumber.text = "";
		}
		characterName.text = character.name;
		if (character.statusAdd == "") {
			characterStatus.text = Database.strings.GetString (character.status);
		} else {
			characterStatus.text = string.Format (Database.strings.GetString (character.status), character.statusAdd);
		}
		characterGender.text = Database.strings.GetString (character.gender);
		characterLevel.text = character.level.ToString ();
		characterExp.text = character.exp.ToString ();
		foreach (Text slot in characterStats) {
			if (character.totalStats ["Weight"] > character.baseStats ["Strength"] && (slot.name == "Accuracy" || slot.name == "Evade" || slot.name == "BlockChance" || slot.name == "Speed")) {
				slot.color = debuffColor;
			} else {
				slot.color = normalColor;
			}
			if (slot.name == "Health" || slot.name == "Mana") {
				slot.text = character.totalStats ["Max" + slot.name].ToString ();
			} else if (slot.name == "Fame") {
				slot.text = character.baseStats [slot.name].ToString ();
			} else if (slot.name == "Weight") {
				slot.text = character.totalStats [slot.name].ToString () + "/" + character.baseStats ["Strength"];
			} else {
				slot.text = character.totalStats [slot.name].ToString ();
			}

		}
		FillInventory (character.equipment);
		FillSkill (character.skillLevel, character.skillExp);
		FillAbility (character.abilities);
		if (!show) {
			showSub = "inventory";
		}

	}

	public void CloseScreen ()
	{
		show = false;
	}

	public void FillInventory (List<InventorySlot> inventory)
	{
		for (int i=0; i<inventory.Count; i++) {
			itemSlotList [i].GetComponent<SlotInfo> ().FillSlotWithItem (inventory [i]);
			if (playerType != "Character" || character.status != "Idle") {
				itemSlotList [i].GetComponent<Button> ().interactable = false;
			} else {
				itemSlotList [i].GetComponent<Button> ().interactable = true;
			}
		}
		inventoryList.SetSize (inventory.Count, 64);
	}

	public void FillSkill (Dictionary<int,int> levels, Dictionary<int,int> exp)
	{
		List<Skill> list = Database.skills.SkillList ();
		for (int i=0; i<list.Count; i++) {
			skillSlotList.GeneratePrefab (i, skillSlot, "Skill", skillList);
			skillSlotList [i].GetComponent<SlotInfo> ().FillSlotWithSkill (list [i], levels [i], exp [i]);
		}
		skillList.SetSize (list.Count, 64);
	}

	public void FillAbility (List<int> abilities)
	{
		for (int i=0; i<abilities.Count; i++) {
			abilitySlotList.GeneratePrefab (i, abilitySlot, "Ability", abilityList);
			abilitySlotList [i].GetComponent<SlotInfo> ().FillSlotWithAbility (Database.skills.GetAbility (abilities [i]));
		}
		if (abilities.Count < abilitySlotList.Count) {
			for (int i=abilities.Count; i<abilitySlotList.Count; i++) {
				abilitySlotList [i].SetActive (false);
			}
		}
		abilityList.SetSize (abilities.Count, 64);
	}

	public void SetShow (string display)
	{
		showSub = display;
	}
}
