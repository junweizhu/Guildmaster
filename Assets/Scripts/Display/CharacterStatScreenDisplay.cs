using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterStatScreenDisplay : MonoBehaviour {
	public Text characterNumber;
	public Text characterName;
	public Text characterStatus;
	public Text characterGender;
	public Text characterLevel;
	public Text characterExp;
	public Text characterMoney;
	public List<Text> characterStats;
	public Transform inventoryList;
	public List<GameObject> itemSlotList=new List<GameObject>();
	public GameObject inventorySlot;
	public GameObject skillSlot;
	public List<GameObject> skillSlotList=new List<GameObject>();
	public Transform skillList;
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

	// Update is called once per frame
	void Update () {
		GetComponent<CanvasGroup>().SetShow(show);
		if (showSub=="inventory"&&inventoryList.GetComponent<CanvasGroup>().alpha==0)
		{
			inventoryList.GetComponent<CanvasGroup>().alpha=1;
			inventoryList.GetComponent<CanvasGroup>().blocksRaycasts=true;
			inventoryList.GetComponent<ScrollRect>().normalizedPosition=new Vector2(0,1);
		}
		else if(showSub!="inventory"&&inventoryList.GetComponent<CanvasGroup>().alpha==1)
		{
			inventoryList.GetComponent<CanvasGroup>().alpha=0;
			inventoryList.GetComponent<CanvasGroup>().blocksRaycasts=false;
		}
		if(showSub=="skill"&&skillList.GetComponent<CanvasGroup>().alpha==0)
		{
			skillList.GetComponent<CanvasGroup>().alpha=1;
			skillList.GetComponent<CanvasGroup>().blocksRaycasts=true;
			skillList.GetComponent<ScrollRect>().normalizedPosition=new Vector2(0,1);
		}
		else if (showSub!="skill"&&skillList.GetComponent<CanvasGroup>().alpha==1)
		{
			skillList.GetComponent<CanvasGroup>().alpha=0;
			skillList.GetComponent<CanvasGroup>().blocksRaycasts=false;
		}
	}

	public void FillSlot(int id)
	{
		Character character=new Character();
		if (playerType=="Character"){
			character=myGuild.GetCharacter(id);
			prevButton.interactable=(id>1);
			nextButton.interactable=(id<myGuild.characterlist.Count);
			giveButton.SetActive(true);
			recruitButton.SetActive(false);
			characterNumber.text=character.guildnr.ToString();
		} else if (playerType=="Recruit"){
			character=Database.characters.GetRecruitables()[id];
			prevButton.interactable=(id>0);
			nextButton.interactable=(id<Database.characters.GetRecruitables().Count-1);
			giveButton.SetActive(false);
			recruitButton.SetActive(true);
			characterNumber.text="";
		}
		characterName.text=character.name;
		if (character.statusAdd==""){
			characterStatus.text=Database.strings.GetString(character.status);
		} else{
			characterStatus.text=string.Format(Database.strings.GetString(character.status),character.statusAdd);
		}
		characterGender.text=Database.strings.GetString(character.gender);
		characterLevel.text=character.level.ToString();
		characterExp.text=character.exp.ToString();
		foreach (Text slot in characterStats)
		{
			if(character.totalStats ["Weight"] > character.baseStats ["Strength"]&& (slot.name=="Accuracy"||slot.name=="Evade"||slot.name=="BlockChance"||slot.name=="Speed")){
				slot.color=debuffColor;
			} else{
				slot.color=normalColor;
			}
			if (slot.name=="Health" ||slot.name=="Mana")
			{
				slot.text=character.totalStats["Max"+slot.name].ToString();
			}
			else if (slot.name=="Fame"){
				slot.text=character.baseStats[slot.name].ToString();
			} else if (slot.name=="Weight"){
				slot.text=character.totalStats[slot.name].ToString()+"/"+character.baseStats["Strength"];
			}else {
				slot.text=character.totalStats[slot.name].ToString();
			}

		}

		characterMoney.text=character.money.ToString();
		FillInventory(character.equipment);
		FillSkill(character.skillLevel,character.skillExp);
		if (!show){
			showSub="inventory";
		}

	}
	public void CloseScreen()
	{
		show=false;
	}
	public void FillInventory(List<InventorySlot> inventory)
	{
		for (int i=0;i<inventory.Count;i++)
		{
			itemSlotList[i].GetComponent<SlotInfo>().FillSlotWithItem(inventory[i]);
			if (playerType!="Character"){
				itemSlotList[i].GetComponent<Button>().interactable=false;
			} else{
				itemSlotList[i].GetComponent<Button>().interactable=true;
			}
		}
		inventoryList.SetSize(inventory.Count,64);
	}

	public void FillSkill(Dictionary<int,int> levels,Dictionary<int,int> exp)
	{
		List<Skill> list=Database.skills.SkillList();
		for (int i=0; i<list.Count;i++)
		{
			skillSlotList.GeneratePrefab(i,skillSlot,"Skill",skillList);
			skillSlotList[i].GetComponent<SlotInfo>().FillSlotWithSkill(list[i],levels[i],exp[i]);
		}
		skillList.SetSize(list.Count,64);
	}

	public void SetShow(string display)
	{
		showSub=display;
	}
}
