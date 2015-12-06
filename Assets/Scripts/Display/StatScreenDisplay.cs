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
	public GameObject inventoryList;
	public List<GameObject> itemSlotList=new List<GameObject>();
	public GameObject inventorySlot;
	public GameObject skillSlot;
	public List<GameObject> skillSlotList=new List<GameObject>();
	public GameObject skillList;
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
		memberName.text=member.memberName;
		memberStatus.text=member.memberStatus;
		memberGender.text=member.memberGender.ToString();
		memberLevel.text=member.memberLevel.ToString();
		memberExp.text=member.memberExp.ToString();
		memberHealth.text=member.memberHealth.ToString();
		memberMana.text=member.memberMana.ToString();
		memberStrength.text=member.memberStrength.ToString();
		memberIntelligence.text=member.memberIntelligence.ToString();
		memberDexterity.text=member.memberDexterity.ToString();
		memberAgility.text=member.memberAgility.ToString();
		memberFame.text=member.memberFame.ToString();
		memberMoney.text=member.memberMoney.ToString()+" Gold";
		FillInventory(member.memberInventory, member.memberItemQuality);
		FillSkill(member.skillList,member.memberSkillLevel,member.memberSkillExp);
	}
	public void CloseScreen()
	{
		show=false;
	}
	public void FillInventory(List<Item> inventory, List<int> itemQuality)
	{
		for (int i=0;i<inventory.Count;i++)
		{
			if (i+1>itemSlotList.Count)
			{
				itemSlotList.Add (GameObject.Instantiate(inventorySlot) as GameObject);
				itemSlotList[i].transform.SetParent(inventoryList.transform);
				itemSlotList[i].GetComponent<SlotInfo>().ResetTransform();
			}
			else if (itemSlotList[i].activeSelf==false)
			{
				itemSlotList[i].SetActive(true);
			}
			itemSlotList[i].GetComponent<SlotInfo>().FillSlotWithItem(inventory[i],itemQuality[i]);
		}
		if(inventory.Count<itemSlotList.Count)
		{
			for (int i=inventory.Count;i<itemSlotList.Count;i++)
			{
				itemSlotList[i].SetActive(false);
			}
		}
		inventoryList.GetComponent<RectTransform>().sizeDelta=new Vector2(0, inventory.Count*60);
		if (inventory.Count>4)
			inventoryList.GetComponent<ScrollRect>().vertical=true;
		else
			inventoryList.GetComponent<ScrollRect>().vertical=false;
	}

	public void FillSkill(List<Skill> list,Dictionary<Skill,int> levels,Dictionary<Skill,int> exp)
	{
		for (int i=0; i<list.Count;i++)
		{
			
			if (i+1>skillSlotList.Count)
			{
				skillSlotList.Add (GameObject.Instantiate(skillSlot) as GameObject);
				skillSlotList[i].transform.SetParent(skillList.transform);
				skillSlotList[i].GetComponent<SlotInfo>().ResetTransform();
			}
			skillSlotList[i].GetComponent<SlotInfo>().FillSlotWithSkill(list[i],levels[list[i]],exp[list[i]]);
		}
		skillList.GetComponent<RectTransform>().sizeDelta=new Vector2(0, list.Count*60);
		if (list.Count>4)
			skillList.GetComponent<ScrollRect>().vertical=true;
		else
			skillList.GetComponent<ScrollRect>().vertical=false;
	}
	public void SetShow(string display)
	{
		showSub=display;
	}
}
