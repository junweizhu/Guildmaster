using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlotInfo : MonoBehaviour {
	public Text slotNumber;
	public Text slotName;
	public Text slotStatus;
	public Text slotQuality;
	public Text slotQuantity;
	public Text slotCost;
	public Text slotDuration;
	public Text slotSkillLevel;
	public Image slotIcon;
	public Text slotLevel;
	public Slider slotExp;
	public Button slotAdd;
	public Button slotRed;
	public int id;
	public int cost;
	public bool selected=false;

	void Update()
	{
		if(GetComponent<Image>()!=null){
			GetComponent<Image>().enabled=selected;
		}
	}
	public void FillSlotWithQuest(int slotnr, Quest quest,string status, float days)
	{
		slotName.text=quest.name;
		slotNumber.text=slotnr.ToString();
		id=quest.id;
		slotStatus.text=status;
		if (days>0)
		{
			slotDuration.text=quest.duration.ToString("f1")+ " Days";
		}
		else
		{
			slotDuration.text="";
		}
	}
	public void FillSlotWithArea(Area area)
	{
		slotName.text=area.name;
		id=area.id;
	}
	public void FillSlotWithMember(Member member)
	{
		if (slotNumber!=null)
		{
			slotNumber.text=member.guildnr.ToString();
		}
		slotName.text=member.name;
		if(slotStatus!=null)
		{
			slotStatus.text=member.status;
		}
		id=member.guildnr;
	}
	public void FillSlotWithMember(Member member, int slotnr)
	{
		slotNumber.text=slotnr.ToString();
		slotName.text=member.name;
		id=member.guildnr;
	}
	public void FillSlotWithMember(Member member, Skill skill)
	{
		slotNumber.text=member.guildnr.ToString();
		slotName.text=member.name;
		id=member.guildnr;
		slotSkillLevel.text=member.skillLevel[skill].ToString();

	}
	public void FillSlotWithItem(int slotnr, Item item, int quality,int quantity)
	{
		slotNumber.text=slotnr.ToString();
		slotName.text=item.name;
		slotQuality.text=quality.ToString();
		slotQuantity.text=quantity.ToString();
		id=item.id;
	}
	public void FillSlotWithItem(int slotnr, Item item, int quality)
	{
		id= item.id;
		slotName.text=item.name;
		slotQuality.text=quality.ToString();
		slotQuantity.text=0.ToString();
		slotCost.text=0.ToString();
		cost=item.sellValue;
	}
	public void FillSlotWithItem(InventorySlot slot)
	{
		slotName.text=slot.item.name;
		slotQuality.text=slot.quality.ToString();
	}
	public void FillSlotWithReward(Item item,int quantity)
	{
		slotName.text=item.name;
		slotQuantity.text=quantity.ToString();
	}
	public void FillSlotWithSkill(Skill skill, int level, int exp)
	{
		slotName.text=skill.name;
		slotLevel.text="Lvl. "+level.ToString();
		slotExp.value=exp;
	}
	public void DisplayStat(string type)
	{
		if (type=="member")
		{
			GameObject.FindObjectOfType<GameManager>().DisplayMemberStats(this);
		}
		if (type=="item")
		{
			GameObject.FindObjectOfType<WarehouseScreenDisplay>().DisplayItemStats(this);
		}
		if (type=="quest")
		{
			GameObject.FindObjectOfType<GameManager>().DisplayQuestStats(this);
		}
		if (type=="area")
		{
			GameObject.FindObjectOfType<OutsideScreenDisplay>().DisplayAreaStats(this);
		}
	}
	public void Recruit()
	{
		GameObject.Find("MainGame").GetComponent<GameManager>().RecruitMember(id);
	}
	public void ChangeQuantity(string choice)
	{
		if (choice=="+")
			slotQuantity.text=(int.Parse(slotQuantity.text)+1).ToString();
		else if (choice=="-"&& int.Parse(slotQuantity.text)>0)
			slotQuantity.text=(int.Parse(slotQuantity.text)-1).ToString();
		slotCost.text=(int.Parse(slotQuantity.text)*cost).ToString();
		if (!selected)
		{
			Select ();
			GameObject.FindObjectOfType<ShopScreenDisplay>().UpdateItemInfo(this);
		}
	}
	public int GetItemId()
	{
		return id;
	}

	public int GetItemQuantity()
	{
		return int.Parse(slotQuantity.text);
	}
	public void Select()
	{
		selected=!selected;
	}

	public void SelectSlot()
	{
		if (GameObject.FindObjectOfType<MemberSelectScreenDisplay>().show)
		{
			GameObject.FindObjectOfType<MemberSelectScreenDisplay>().SelectMember(this);
		}
		else if (GameObject.FindObjectOfType<SearchScreenDisplay>().show)
		{
			GameObject.FindObjectOfType<SearchScreenDisplay>().SelectMember(this);
		}
	}
	public void ResetSelection()
	{
		selected=false;
	}
}
