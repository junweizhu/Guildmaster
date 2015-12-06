using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopScreenDisplay : MonoBehaviour {

	public Text shopName;
	public Text buyerName;
	public Member buyer;
	public GameObject shopItemPrefab;
	public Transform shopItemList;
	public List<GameObject> prefabList=new List<GameObject>();
	public Text itemHealth;
	public Text itemMana;
	public Text itemAttack;
	public Text itemDefense;
	public Text itemStrength;
	public Text itemIntelligence;
	public Text itemDexterity;
	public Text itemAgility;
	public Text itemDescription;
	public int buyerId;
	public bool show =false;

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
	}
	public void UpdateText(string name,Member member, List<Item> shoplist)
	{
		shopName.text=name;
		UpdateBuyer(member);
		FillShop(shoplist);
	}

	public void FillShop(List<Item> shoplist)
	{
		for (int i=0;i<shoplist.Count;i++)
		{
			if (i+1>prefabList.Count)
			{
				prefabList.Add (GameObject.Instantiate(shopItemPrefab) as GameObject);
				prefabList[i].transform.SetParent(shopItemList);
				prefabList[i].GetComponent<SlotInfo>().ResetTransform();
			}
			else if (prefabList[i].activeSelf==false)
			{
				prefabList[i].SetActive(true);
			}
			prefabList[i].GetComponent<SlotInfo>().FillSlotWithItem(i, shoplist[i],100);
		}
		if(shoplist.Count<prefabList.Count)
		{
			for (int i=shoplist.Count;i<prefabList.Count;i++)
			{
				prefabList[i].SetActive(false);
			}
		}
		shopItemList.GetComponent<RectTransform>().sizeDelta=new Vector2(0, shoplist.Count*60);
		if (shoplist.Count>4)
			shopItemList.GetComponent<ScrollRect>().vertical=true;
		else
			shopItemList.GetComponent<ScrollRect>().vertical=false;
	}

	public void LeaveShop()
	{
		show=false;
	}
	public void UpdateBuyer(Member member)
	{
		buyer=member;
		buyerName.text=buyer.memberName;
		buyerId=buyer.memberId;
	}

	public Dictionary<int,int> GetItemToBuy()
	{
		Dictionary<int,int> shoppingList=new Dictionary<int, int>();
		foreach (GameObject item in prefabList)
		{
			if (item.activeSelf==true)
			{
				if (item.GetComponent<SlotInfo>().GetItemQuantity()>0)
				{
					shoppingList[item.GetComponent<SlotInfo>().id]=item.GetComponent<SlotInfo>().GetItemQuantity();
				}
			}
			
		}
		return shoppingList;
	}
	public bool BuyingSomething()
	{
		if (GetItemToBuy().Count>0)
		{
			return true;
		}
		else 
			return false;
	}
}
