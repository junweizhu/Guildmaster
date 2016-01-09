using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class StatScreenDisplay : MonoBehaviour {
	public Text memberNumber;
	public Text memberName;
	public Text memberStatus;
	public Text memberGender;
	public Text memberLevel;
	public Text memberExp;
	public Text memberHealth;
	public Text memberMana;
	public Text memberStrength;
	public Text memberIntelligence;
	public Text memberDexterity;
	public Text memberAgility;
	public Text memberFame;
	public Text memberMoney;
	public List<Text> memberStats;
	public Transform inventoryList;
	public List<GameObject> itemSlotList=new List<GameObject>();
	public GameObject inventorySlot;
	public GameObject skillSlot;
	public List<GameObject> skillSlotList=new List<GameObject>();
	public Transform skillList;
	public bool show;
	public string showSub;
	public string playerType;
	
	// Update is called once per frame
	void Update () {
		if(show)
		{
			GetComponent<CanvasGroup>().alpha=1;
			GetComponent<CanvasGroup>().blocksRaycasts=true;
		}
		else 
		{
			GetComponent<CanvasGroup>().alpha=0;
			GetComponent<CanvasGroup>().blocksRaycasts=false;
		}
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

	public void FillSlot(int number,Member member)
	{
		memberNumber.text=number.ToString();
		memberName.text=member.name;
		memberStatus.text=member.status;
		memberGender.text=member.gender.ToString();
		memberLevel.text=member.level.ToString();
		memberExp.text=member.exp.ToString();
		foreach (Text slot in memberStats)
		{
			if (slot.name=="Health" ||slot.name=="Mana")
			{
				slot.text=member.stats["Current"+slot.name].ToString()+"/"+member.stats["Max"+slot.name].ToString();
			}
			else
			{
				slot.text=member.stats[slot.name].ToString();
			}

		}
		memberMoney.text=member.money.ToString()+" Gold";
		FillInventory(member.GetInventory());
		FillSkill(member.skillList,member.skillLevel,member.skillExp);
	}
	public void CloseScreen()
	{
		show=false;
	}
	public void FillInventory(List<InventorySlot> inventory)
	{
		for (int i=0;i<inventory.Count;i++)
		{
			itemSlotList.GeneratePrefab(i,inventorySlot,"Item",inventoryList);
			itemSlotList[i].GetComponent<SlotInfo>().FillSlotWithItem(inventory[i]);
		}
		if(inventory.Count<itemSlotList.Count)
		{
			for (int i=inventory.Count;i<itemSlotList.Count;i++)
			{
				itemSlotList[i].SetActive(false);
			}
		}
		inventoryList.SetSize(inventory.Count,60);
	}

	public void FillSkill(List<Skill> list,Dictionary<Skill,int> levels,Dictionary<Skill,int> exp)
	{
		for (int i=0; i<list.Count;i++)
		{
			skillSlotList.GeneratePrefab(i,skillSlot,"Skill",skillList);
			skillSlotList[i].GetComponent<SlotInfo>().FillSlotWithSkill(list[i],levels[list[i]],exp[list[i]]);
		}
		skillList.SetSize(list.Count,60);
	}
	public void SetShow(string display)
	{
		showSub=display;
	}
}
