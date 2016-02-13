using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlotInfo : MonoBehaviour
{
	public Text slotNumber;
	public Text slotName;
	public Text slotStatus;
	public Text slotDurability;
	public Text slotQuantity;
	public Text slotCost;
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
	public Text maxQuantity;
	public Task task;
	public Character character;
	public string longDescription;
	private Color unselectedColor;
	private Color selectedColor;
	private ColorBlock colorBlock;

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
			if (selected && colorBlock.normalColor == unselectedColor) {
				colorBlock.normalColor = selectedColor;
			} else if (!selected && colorBlock.normalColor == selectedColor) {
				colorBlock.normalColor = unselectedColor;
			}
			GetComponent<Button> ().colors = colorBlock;
		}
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
		slotNumber.text = slotnr.ToString ();
		FillSlotWithItem (slot);
	}

	public void FillSlotWithItem (int slotnr, Item item)
	{
		id = item.id;
		cost = item.sellValue;
		slotName.text = item.name;
		slotDurability.text = item.durability.ToString ();
		slotQuantity.text = 0.ToString ();
		slotCost.text = cost.ToString ()+" G";;
	}

	public void FillSlotWithItem (InventorySlot slot)
	{
		if (slot.filled) {
			Item item = Database.items.FindItem (slot.itemId);
			slotName.text = item.name;
			slotDurability.text = slot.durability.ToString () + "/" + item.durability.ToString ();
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
		if (slotQuantity.text=="0"){
			slotCost.text = cost.ToString ()+" G";
		} else{
			slotCost.text = (int.Parse (slotQuantity.text) * cost).ToString ()+" G";
		}
		if (!selected && name.Contains ("Buy")) {
			Select();
			SelectSlot();
		}
	}

	public int GetItemId ()
	{
		return id;
	}

	public int GetItemQuantity ()
	{
		return int.Parse (slotQuantity.text);
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
		} else if (GameObject.FindObjectOfType<ShopScreenDisplay>().show){
			GameObject.FindObjectOfType<ShopScreenDisplay> ().UpdateItemInfo (this);
		}

	}

	public void ResetSelection ()
	{
		selected = false;
	}
}
