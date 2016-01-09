using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopScreenDisplay : MonoBehaviour
{

	public Text shopName;
	public Text buyerName;
	public List<Member> buyers = new List<Member> ();
	public GameObject shopItemPrefab;
	public Transform shopItemList;
	public List<GameObject> prefabList = new List<GameObject> ();
	public Text totalQuantity;
	public Text totalCost;
	public Text maxQuantity;
	public Text itemDescription;
	public List<Text> statlist = new List<Text> ();
	public Dictionary<string,Text> stats = new Dictionary<string, Text> ();
	public int firstId;
	public bool show = false;
	public int maxBuy;
	public Button buyButton;
	private ItemDatabase idb;
	private SlotInfo lastSelected;
	private int guildMoney;

	void Start ()
	{
		idb = GameObject.FindObjectOfType<ItemDatabase> ();
		foreach (Text stat in statlist) {
			stats [stat.name] = stat;
		}
	}

	void Update ()
	{
		if (show) {
			GetComponent<CanvasGroup> ().alpha = 1;
			GetComponent<CanvasGroup> ().blocksRaycasts = true;
		} else {
			GetComponent<CanvasGroup> ().alpha = 0;
			GetComponent<CanvasGroup> ().blocksRaycasts = false;
		}
		if (buyers.Count > 0 && GetItemToBuy ().Count > 0) {
			buyButton.interactable = true;
		} else {
			buyButton.interactable = false;
		}
		totalQuantity.text = TotalQuantity ().ToString ();
		maxQuantity.text = "/" + maxBuy.ToString ();
		totalCost.text = TotalCost ().ToString ();
		UpdateSlotButtons ();
	}

	public void UpdateText (string name, List<Item> shoplist)
	{
		buyers = new List<Member> ();
		shopName.text = name;
		buyerName.text = "Select";
		FillShop (shoplist);
		maxBuy = buyers.Count * 5;
		UpdateItemInfo(null);
	}

	public void FillShop (List<Item> shoplist)
	{
		firstId = 0;
		for (int i=0; i<shoplist.Count; i++) {
			prefabList.GeneratePrefab(i,shopItemPrefab,"Item",shopItemList);
			prefabList [i].GetComponent<SlotInfo> ().FillSlotWithItem (i, shoplist [i], 100);
			prefabList [i].GetComponent<SlotInfo> ().ResetSelection();
		}
		if (shoplist.Count < prefabList.Count) {
			for (int i=shoplist.Count; i<prefabList.Count; i++) {
				prefabList [i].SetActive (false);
			}
		}
		shopItemList.SetSize (shoplist.Count, 60);
	}

	public void LeaveShop ()
	{
		show = false;
	}

	public void UpdateBuyer (List<Member> members, int buyerid, string buyername)
	{
		foreach (Member member in buyers) {
			if (!members.Contains (member)) {
				buyers.Remove (member);
			}
		}
		foreach (Member member in members) {
			if (!buyers.Contains (member)) {
				buyers.Add (member);
			}

		}
		firstId = buyerid;
		buyerName.text = buyername;
		maxBuy = buyers.Count * 5;
	}

	public Dictionary<int,int> GetItemToBuy ()
	{
		Dictionary<int,int> shoppingList = new Dictionary<int, int> ();
		foreach (GameObject item in prefabList) {
			if (item.activeSelf == true) {
				if (item.GetComponent<SlotInfo> ().GetItemQuantity () > 0) {
					shoppingList [item.GetComponent<SlotInfo> ().id] = item.GetComponent<SlotInfo> ().GetItemQuantity ();
				}
			}
			
		}
		return shoppingList;
	}

	public int TotalQuantity ()
	{
		int quantity = 0;
		foreach (KeyValuePair<int,int> item in GetItemToBuy()) {
			if (item.Value > 0) {
				quantity += item.Value;
			}
		}
		return quantity;
	}

	public int TotalCost ()
	{
		int cost = 0;
		foreach (KeyValuePair<int,int> item in GetItemToBuy()) {
			if (item.Value > 0) {
				cost += idb.FindItem (item.Key).sellValue * item.Value;
			}
		}
		return cost;
	}

	public bool BuyingSomething ()
	{
		if (GetItemToBuy ().Count > 0) {
			return true;
		} else 
			return false;
	}

	public void UpdateSlotButtons ()
	{

		foreach (GameObject slot in prefabList) {
			if (TotalQuantity () >= maxBuy) {
				slot.GetComponent<SlotInfo> ().slotAdd.interactable = false;
			} else {
				slot.GetComponent<SlotInfo> ().slotAdd.interactable = true;
			}
			if (slot.GetComponent<SlotInfo> ().GetItemQuantity () == 0) {
				slot.GetComponent<SlotInfo> ().slotRed.interactable = false;
			} else {
				slot.GetComponent<SlotInfo> ().slotRed.interactable = true;
			}
		}
	}

	public void UpdateItemInfo (SlotInfo slot)
	{
		if (lastSelected != slot ||slot==null) {
			foreach (KeyValuePair<string,Text> stat in stats) {
				stat.Value.text = "  ";
			}
			itemDescription.text="";
			if (slot != null) {
			Item item = idb.FindItem (slot.id);
				itemDescription.text = item.description;
				for (int i=0; i<prefabList.Count; i++) {
					if (prefabList [i].GetComponent<SlotInfo>() == slot) {
						foreach (KeyValuePair<string,int> stat in item.stats){
							if (stats.ContainsKey(stat.Key)){
								if (stat.Value>0){
									stats[stat.Key].text="+";
								} else if (stat.Value<0){
									stats[stat.Key].text="-";
								}
								if (stat.Value!=0){
									stats[stat.Key].text+=stat.Value.ToString();
								}
							}
						}
						/*
						if (item.modifier1 != null) {
							if (stats.ContainsKey (item.modifier1)) {
								if (item.value1 > 0){
									stats [item.modifier1].text = "+";

								}
								if (item.value1 != 0)
									stats [item.modifier1].text += item.value1.ToString ();
							}
						}*/
					}
				}
			}
			if (lastSelected!=null){
				lastSelected.Select();
			}
			lastSelected=slot;
		}
	}
}
