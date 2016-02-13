using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ShopScreenDisplay : MonoBehaviour
{

	public Text shopName;
	public List<Text> buyerNames=new List<Text>();
	public List<Character> buyers = new List<Character> ();
	public GameObject shopItemPrefab;
	public Transform shopItemList;
	public List<GameObject> prefabList = new List<GameObject> ();
	public Text totalQuantity;
	public Text totalCost;
	public Text maxQuantity;
	public Text itemDescription;
	public List<Text> statlist = new List<Text> ();
	public Dictionary<string,Text> stats = new Dictionary<string, Text> ();
	public bool show = false;
	public int maxBuy;
	public Button buyButton;
	private SlotInfo lastSelected;
	public int guildMoney;
	public Text guildFunds;

	void Start ()
	{
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
		if (buyers.Count > 0 && GetItemToBuy ().Count > 0 &&TotalCost()<guildMoney) {
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
		buyers = new List<Character> ();
		shopName.text = name;
		FillShop (shoplist);
		maxBuy = 0;
		UpdateItemInfo (null);
		FillBuyers();
	}

	public void FillShop (List<Item> shoplist)
	{
		for (int i=0; i<shoplist.Count; i++) {
			prefabList.GeneratePrefab (i, shopItemPrefab, "Buy", shopItemList);
			prefabList [i].GetComponent<SlotInfo> ().FillSlotWithItem (i, shoplist [i]);
			prefabList [i].GetComponent<SlotInfo> ().ResetSelection ();
		}
		if (shoplist.Count < prefabList.Count) {
			for (int i=shoplist.Count; i<prefabList.Count; i++) {
				prefabList [i].SetActive (false);
			}
		}
		shopItemList.SetSize (shoplist.Count, 64);
		guildFunds.text=guildMoney.ToString()+" G";
	}

	public void LeaveShop ()
	{
		show = false;
	}

	public void UpdateBuyer (List<Character> characters)
	{
		buyers=characters.OrderBy(character=>character.guildnr).ToList();
		FillBuyers();
		maxBuy=0;
		if (buyers.Count>1){
			for (int i=0;i<buyers.Count;i++){
				maxBuy+=buyers[i].totalStats["CarrySize"];
			}
		} else if (buyers.Count==1){
			maxBuy=buyers[0].totalStats["CarrySize"];
		}
	}

	public void FillBuyers(){
		for(int i=0;i<buyerNames.Count;i++){
			if (i<buyers.Count){
				buyerNames[i].text=buyers[i].name;
			} else{
				buyerNames[i].text="";
			}
		}

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
				cost += Database.items.FindItem (item.Key).sellValue * item.Value;
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
			if (slot.activeSelf == true) {
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
	}

	public void UpdateItemInfo (SlotInfo slot)
	{
		if (lastSelected != slot || slot == null) {
			foreach (KeyValuePair<string,Text> stat in stats) {
				stat.Value.text = "  ";
			}
			itemDescription.text = "";
			if (slot != null) {
				Item item = Database.items.FindItem (slot.id);
				itemDescription.text = item.description;
				if (itemDescription.text!="")
					itemDescription.text+=" ";
				if (item.subType=="Heal"){
					itemDescription.text+=string.Format(Database.strings.GetString("Heal"),item.GetStatString());
				}
				for (int i=0; i<prefabList.Count; i++) {
					if (prefabList [i].GetComponent<SlotInfo> () == slot) {
						foreach (KeyValuePair<string,int> stat in item.stats) {
							if (stats.ContainsKey (stat.Key)) {
								if (stat.Value < 0) {
									stats [stat.Key].text = "-";
								}
								if (stat.Value != 0) {
									stats [stat.Key].text += stat.Value.ToString ();
								}
							}
						}
					}
				}
			}
			if (lastSelected != null) {
				if (lastSelected.selected){
					lastSelected.Select ();
				}
			}
			if (slot!=null){
				slot.Select();
			}
			lastSelected = slot;
		}
	}
}
