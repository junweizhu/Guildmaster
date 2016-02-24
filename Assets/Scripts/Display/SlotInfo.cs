using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SlotInfo : MonoBehaviour
{
	public Text slotNumber;
	public Text slotName;
	public Text slotStatus;
	public Text slotDurability;
	public Text slotQuantity;
	public Text maxQuantity;
	public Text slotCost;
	public Text slotRange;
	public Text slotDuration;
	public Text slotSkillLevel;
	public Image slotIcon;
	public Text slotLevel;
	public Slider slotExp;
	public Button slotAdd;
	public Button slotRed;
	public Text slotDescription;
	public int id;
	public int cost;
	public bool selected = false;
	public bool isAbility = false;
	public Task task;
	public Character character;
	public string longDescription;
	public CanvasGroup itemText;
	private Color unselectedColor;
	private Color selectedColor;
	private ColorBlock colorBlock;
	public Button recruitRequestButton;


	void Start ()
	{
		if (GetComponent<Button> () != null) {
			unselectedColor = GetComponent<Button> ().colors.normalColor;
			selectedColor = GetComponent<Button> ().colors.highlightedColor;
			colorBlock = GetComponent<Button> ().colors;
		}
	}

	void Update ()
	{

		if (GetComponent<Button> () != null) {
			if (selected && (colorBlock.normalColor == unselectedColor || colorBlock.highlightedColor == unselectedColor)) {
				colorBlock.normalColor = selectedColor;
				colorBlock.highlightedColor = selectedColor;
			} else if (!selected && (colorBlock.normalColor == selectedColor || colorBlock.highlightedColor == selectedColor)) {
				colorBlock.normalColor = unselectedColor;
				colorBlock.highlightedColor = unselectedColor;
			}
			GetComponent<Button> ().colors = colorBlock;
		}
	}

	public void FillSlotWithUpgrade(Upgrade upgrade, int level){
		id=upgrade.id;
		slotName.text=upgrade.name;
		slotLevel.text=level.ToString();
		slotDescription.text=upgrade.description;
	}

	public void FillSlotWithQuest (int slotnr, Quest quest)
	{
		slotName.text = quest.name;
		slotNumber.text = slotnr.ToString ();
		id = quest.id;
		if (slotStatus != null) {
			slotStatus.text = Database.strings.GetString (quest.status);
		}
		if (quest.duration > 0) {
			slotDuration.text = string.Format (Database.strings.GetString ("Duration"), quest.duration.ToString ("f1"));
		} else {
			slotDuration.text = "";
		}
	}

	public void FillSlotWithSkill (Skill skill, int costModifier)
	{
		itemText.SetShow (false);
		id = skill.id;
		slotName.text = skill.name;
		slotCost.text = string.Format (Database.strings.GetString ("Currency"), 0.ToString ());
		cost = costModifier;
		isAbility = false;
		slotDurability.text = 0.ToString ();
		slotQuantity.text = 0.ToString ();
	}

	public void FillSlotWithAbility (Ability ability)
	{
		id = ability.id;
		slotName.text = ability.name;
		if (itemText!=null){
			itemText.SetShow (false);
			cost = ability.teachingCost;
			slotCost.text = string.Format (Database.strings.GetString ("Currency"), cost.ToString ());
			isAbility = true;
			slotDurability.text = 0.ToString ();
			slotQuantity.text = 0.ToString ();
		} else{
			slotCost.text =ability.manaCost.ToString()+" "+Database.strings.GetString("Mana");
			if (ability.range==1){
				slotRange.text=Database.strings.GetString ("CloseRange");
			} else{
				slotRange.text=Database.strings.GetString ("LongRange");
			}
		}
	}

	public void FillSlotWithArea (Area area)
	{
		slotName.text = area.name;
		id = area.id;
	}

	public void TaskLog (Task task)
	{
		this.task = task;

		character = null;
		if (task != null) {
			longDescription = task.Details ();
			slotDescription.text = task.ShortDescription ();
		} else {
			longDescription = "";
			slotDescription.text = Database.strings.GetString ("NoTask");
		}
	}

	public void CharacterLog (Character character)
	{

		slotDescription.text = character.ShortDescription ();
		longDescription = character.Details ();
		this.character = character;
		task = null;
	}

	public void FillSlotWithCharacter (Character character)
	{

		slotName.text = character.name;
		if (slotStatus != null) {
			if (character.statusAdd != "") {
				slotStatus.text = string.Format (Database.strings.GetString (character.status), character.statusAdd);
			} else {
				slotStatus.text = Database.strings.GetString (character.status);
			}
		}
		if (character.recruited) {
			if (slotNumber != null) {
				slotNumber.text = character.guildnr.ToString ();
			}
			id = character.guildnr;
		} else {
			id = Database.characters.RecruitableId (character);
		}
	}

	public void FillSlotWithCharacter (Character character, int skill)
	{
		slotNumber.text = character.guildnr.ToString ();
		slotName.text = character.name;
		id = character.guildnr;
		slotSkillLevel.text = character.skillLevel [skill].ToString ();

	}

	public void FillSlotWithItem (int slotnr, InventorySlot slot)
	{
		if (slotNumber != null) {
			slotNumber.text = slotnr.ToString ();
		}
		if (itemText!=null){
			itemText.SetShow (true);
		}
		if (name.Contains ("Shop")) {
			Item item = Database.items.FindItem (slot.itemId);
			int cost = ExtensionMethods.Calculate (item.sellValue, (float)slot.durability / item.durability);
			FillSlotWithItem (slotnr, item, slot.quantity, cost, true);
		} else {
			FillSlotWithItem (slot);
		}

	}

	public void FillSlotWithItem (int slotnr, Item item, int quantity=0, int calculatedCost=-1, bool useSlotnumberasId=false)
	{
		if (useSlotnumberasId) {
			id = slotnr;
		} else {
			id = item.id;
		}
		if (calculatedCost == -1) {
			cost = item.sellValue;
		} else {
			cost = calculatedCost;
		}
		slotName.text = item.name;
		slotDurability.text = item.durability.ToString ();
		slotQuantity.text = 0.ToString ();
		if (quantity != 0) {
			maxQuantity.text = "/" + quantity.ToString ();
		} else {
			maxQuantity.text = "";
		}
		slotCost.text = string.Format (Database.strings.GetString ("Currency"), cost.ToString ());
		if (itemText!=null){
			itemText.SetShow (true);
		}
	}

	public void FillSlotWithItem (InventorySlot slot)
	{
		if (slot.filled) {
			Item item = Database.items.FindItem (slot.itemId);
			slotName.text = item.name;
			if (slot.durability > 0) {
				slotDurability.text = slot.durability.ToString ();
			} else {
				slotDurability.text = "";
			}
			if (slotQuantity != null) {
				if (maxQuantity != null) {
					slotQuantity.text = 0.ToString ();
					maxQuantity.text = "/" + slot.quantity.ToString ();
				} else {
					slotQuantity.text = slot.quantity.ToString ();
				}
			}
			if (slotIcon != null) {
				slotIcon.gameObject.SetActive (true);
			}
		} else {
			slotName.text = "<Unequipped>";
			if (slot.id == 999) {
				slotName.text = "Unequip";
			}
			if (slot.id==998){
				slotName.text = "Cannot unequip";
			}
			slotDurability.text = "";
			if (slotQuantity != null) {
				slotQuantity.text = "";
			}
			if (slotIcon != null) {
				slotIcon.gameObject.SetActive (false);
			}
		}
		id = slot.id;
	}

	public void FillSlotWithReward (Item item, int quantity)
	{
		slotName.text = item.name;
		slotQuantity.text = quantity.ToString ();
	}

	public void FillSlotWithSkill (Skill skill, int level, int exp)
	{
		slotName.text = skill.name;
		slotLevel.text = "Lvl. " + level.ToString ();
		slotExp.value = exp;
	}

	public void DisplayStat (string type)
	{
		if (type == "character") {
			GameObject.FindObjectOfType<GameManager> ().DisplayCharacterStats (this);
			return;
		}
		if (type == "item") {
			GameObject.FindObjectOfType<WarehouseScreenDisplay> ().DisplayItemStats (this);
			return;
		}
		if (type == "quest") {
			GameObject.FindObjectOfType<GameManager> ().DisplayQuestStats (this);
			return;
		}
		if (type == "area") {
			GameObject.FindObjectOfType<OutsideScreenDisplay> ().DisplayAreaStats (this);
			return;
		}
		if (type == "task") {
			GameObject.FindObjectOfType<NextDayScreenDisplay> ().SelectSlot (this);
			return;
		}
		if (type== "upgrade"){
			GameObject.FindObjectOfType<UpgradeScreenDisplay>().DisplayUpgradeStats(this);
			return;
		}
	}

	public void Recruit ()
	{
		GameObject.Find ("MainGame").GetComponent<GameManager> ().RecruitCharacter (id);
	}

	public void Request ()
	{
		GameObject.Find ("MainGame").GetComponent<GameManager> ().AcceptQuest (id);
	}

	public void ChangeQuantity (string choice)
	{
		if (choice == "+")
			slotQuantity.text = (int.Parse (slotQuantity.text) + 1).ToString ();
		else if (choice == "-" && int.Parse (slotQuantity.text) > 0)
			slotQuantity.text = (int.Parse (slotQuantity.text) - 1).ToString ();
		if (slotQuantity.text == "0") {
			slotCost.text = string.Format (Database.strings.GetString ("Currency"), cost.ToString ());
		} else {
			slotCost.text = string.Format (Database.strings.GetString ("Currency"), (int.Parse (slotQuantity.text) * cost).ToString ());
		}
		if (name.Contains ("Shop")) {
			SelectSlot ();
		}
	}

	public int SetCost (List<Character> characters)
	{
		int totalcost = 0;
		if (isAbility) {
			totalcost = cost * characters.Count;
		} else {
			foreach (Character character in characters) {
				totalcost += cost * character.level;
			}
		}
		slotCost.text = string.Format (Database.strings.GetString ("Currency"), totalcost.ToString ());
		return totalcost;
	}

	public int GetItemId ()
	{
		return id;
	}

	public int GetItemQuantity ()
	{
		if (slotQuantity.text!=""){
			return int.Parse (slotQuantity.text);
		} else{
			return 0;
		}
	}

	public void Select ()
	{
		selected = !selected;
	}

	public void SelectSlot ()
	{
		if (GameObject.FindObjectOfType<CharacterSelectScreenDisplay> ().show) {
			GameObject.FindObjectOfType<CharacterSelectScreenDisplay> ().SelectCharacter (this);
		} else if (GameObject.FindObjectOfType<SearchScreenDisplay> ().show) {
			GameObject.FindObjectOfType<SearchScreenDisplay> ().SelectCharacter (this);
		} else if (GameObject.FindObjectOfType<ItemSelectScreenDisplay> ().show) {
			GameObject.FindObjectOfType<ItemSelectScreenDisplay> ().SelectItem (this);
		} else if (GameObject.FindObjectOfType<ShopScreenDisplay> ().show) {
			GameObject.FindObjectOfType<ShopScreenDisplay> ().UpdateItemInfo (this);
		}
	}

	public void ResetSelection ()
	{
		selected = false;
	}
}
