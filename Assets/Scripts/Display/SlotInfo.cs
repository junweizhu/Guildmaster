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
	public Text slotSkillLevel;
	public Image slotIcon;
	public Text slotLevel;
	public Slider slotExp;
	public int id;
	public int cost;

	public void FillSlotWithMember(Member member)
	{
		slotNumber.text=member.guildnr.ToString();
		slotName.text=member.memberName;
		slotStatus.text=member.memberStatus;
		id=member.memberId;
	}
	public void FillSlotWithMember(Member member, int slotnr)
	{
		slotNumber.text=slotnr.ToString();
		slotName.text=member.memberName;
		id=member.memberId;
	}
	public void FillSlotWithMember(Member member, Skill skill)
	{
		slotName.text=member.memberName;
		id=member.guildnr;
		slotSkillLevel.text=member.memberSkillLevel[skill].ToString();

	}
	public void FillSlotWithItem(int slotnr, Item item, int quality,int quantity)
	{
		slotNumber.text=slotnr.ToString();
		slotName.text=item.itemName;
		slotQuality.text=quality.ToString();
		slotQuantity.text=quantity.ToString();
		id=item.itemId;
	}
	public void FillSlotWithItem(int slotnr, Item item, int quality)
	{
		id= item.itemId;
		slotName.text=item.itemName;
		slotQuality.text=quality.ToString();
		slotQuantity.text=0.ToString();
		slotCost.text=0.ToString();
		cost=item.itemMoney;
	}
	public void FillSlotWithItem(Item item, int quality)
	{
		slotName.text=item.itemName;
		slotQuality.text=quality.ToString();
	}
	public void FillSlotWithSkill(Skill skill, int level, int exp)
	{
		slotName.text=skill.skillName;
		slotLevel.text="Lvl. "+level.ToString();
		slotExp.value=exp;
	}
	public void DisplayStat(string type)
	{
		if (type=="member")
		{

			GameObject.Find("MainGame").GetComponent<GameManager>().DisplayMemberStats(this);
		}
		if (type=="item")
		{
			
			GameObject.Find("MainGame").GetComponent<GameManager>().DisplayItemStats(this);
		}
	}

	public void ResetTransform()
	{
		transform.localPosition=new Vector3(0,0,0);
		transform.localScale=new Vector3(1,1,1);
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
		if (GetComponent<Image>().isActiveAndEnabled)
			GetComponent<Image>().enabled=false;
		else
			GetComponent<Image>().enabled=true;
	}

	public void SelectBuyer()
	{
		GameObject.FindObjectOfType<BuyerScreenDisplay>().SelectMember(this);
	}
}
