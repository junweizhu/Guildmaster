using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ShopScreenDisplay : MonoBehaviour
{

	public Text shopName;
	public List<Text> buyerNames = new List<Text> ();
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
	public bool buying;
	public Text buybuttonText;
	public Shop shop;
	public CanvasGroup itemText;
	private int shopslotCount;

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
			if ((buyers.Count > 0 && GetItemToBuyOrSell ().Count > 0 && TotalCost () < guildMoney) || (buyers.Count > 0 &&shop != null && shop.useSkill && lastSelected != null)||(!buying&&buyers.Count > 0)) {
				buyButton.interactable = true;
			} else {
				buyButton.interactable = false;
			}
			totalQuantity.text = TotalQuantity ().ToString ();
			maxQuantity.text = "/" + maxBuy.ToString ();
			totalCost.text = TotalCost ().ToString ();
		} else {
			GetComponent<CanvasGroup> ().alpha = 0;
			GetComponent<CanvasGroup> ().blocksRaycasts = false;
		}
	}

	public void UpdateText (Shop shop, bool buying=true)
	{
		this.buying = buying;
		Debug.Log (buying);
		buyers = new List<Character> ();
		if (shop != null) {
			shopName.text = shop.name;
			itemText.SetShow (!shop.useSkill);
		} else {
			shopName.text = "Marketplace";
			itemText.SetShow (true);
		}
		this.shop = shop;
		maxBuy = 0;
		UpdateItemInfo (null);
		FillBuyers ();
		FillShop ();
		UpdateSlotButtons ();
		buybuttonText.text = "Send";
	}

	public void FillShop ()
	{
		List<int> shoplist;
		List<int> abilitylist = new List<int> ();
		shopslotCount = 0;
		if (buying) {
			shoplist = shop.GetShopList (Database.myGuild.level);
			abilitylist = shop.GetAbilityList (Database.myGuild.level);
			shopslotCount = shop.GetSize (Database.myGuild.level);
		} else {
			shoplist = Database.myGuild.inventory.GetAllFilledSlotId ();
			shopslotCount = shoplist.Count;
		}
		for (int i=0; i<shopslotCount; i++) {
			prefabList.GeneratePrefab (i, shopItemPrefab, "Shop", shopItemList);
			if (buying) {
				if (shop.useSkill) {
					if (i < shoplist.Count) {
						prefabList [i].GetComponent<SlotInfo> ().FillSlotWithSkill (Database.skills.GetSkill (shoplist [i]), shop.costModifier [shoplist [i]]);
					} else {
						prefabList [i].GetComponent<SlotInfo> ().FillSlotWithAbility (Database.skills.GetAbility (abilitylist [i - shoplist.Count]));
					}
				} else {

					prefabList [i].GetComponent<SlotInfo> ().FillSlotWithItem (i, Database.items.FindItem (shoplist [i]));
				}
			} else {
				prefabList [i].GetComponent<SlotInfo> ().FillSlotWithItem (i, Database.myGuild.inventory.GetInventorySlot (shoplist [i]));
			}
			prefabList [i].GetComponent<SlotInfo> ().ResetSelection ();
		}
		if (shopslotCount < prefabList.Count) {
			for (int i=shoplist.Count; i<prefabList.Count; i++) {
				prefabList [i].SetActive (false);
			}
		}
		shopItemList.SetSize (shopslotCount, 64);
		guildFunds.text = guildMoney.ToString () + " G";
		UpdateSlotButtons ();
	}

	public void LeaveShop ()
	{
		show = false;
	}

	public void UpdateBuyer (List<Character> characters)
	{
		buyers = characters.OrderBy (character => character.guildnr).ToList ();
		FillBuyers ();
		maxBuy = 0;
		if (buyers.Count > 1) {
			for (int i=0; i<buyers.Count; i++) {
				maxBuy += buyers [i].totalStats ["CarrySize"];
			}
		} else if (buyers.Count == 1) {
			maxBuy = buyers [0].totalStats ["CarrySize"];
		}
		if (buying && shop.useSkill) {
			for (int i=0; i<shopslotCount; i++) {
				int cost = prefabList [i].GetComponent<SlotInfo> ().SetCost (characters);
				if (cost > Database.myGuild.money) {
					prefabList [i].GetComponent<Button> ().interactable = false;
				} else {
					prefabList [i].GetComponent<Button> ().interactable = true;
				}
			}
		}
		UpdateSlotButtons ();
	}

	public SlotInfo GetSelected ()
	{
		return lastSelected;
	}

	public void FillBuyers ()
	{
		for (int i=0; i<buyerNames.Count; i++) {
			if (i < buyers.Count) {
				buyerNames [i].text = buyers [i].name;
			} else {
				buyerNames [i].text = "";
			}
		}

	}

	public Dictionary<int,int> GetItemToBuyOrSell ()
	{
		Dictionary<int,int> shoppingList = new Dictionary<int, int> ();
		if (prefabList.Count > 0) {
			foreach (GameObject item in prefabList) {
				if (item.activeSelf == true) {
					if (item.GetComponent<SlotInfo> ().GetItemQuantity () > 0) {
						if (buying){
							shoppingList [item.GetComponent<SlotInfo> ().id] = item.GetComponent<SlotInfo> ().GetItemQuantity ();
						} else{
							shoppingList [item.GetComponent<SlotInfo> ().inventorySlotId] = item.GetComponent<SlotInfo> ().GetItemQuantity ();
						}
					}
				}
			
			}
		}
		return shoppingList;
	}

	public int TotalQuantity ()
	{
		int quantity = 0;
		foreach (KeyValuePair<int,int> item in GetItemToBuyOrSell()) {
			if (item.Value > 0) {
				quantity += item.Value;
			}
		}
		return quantity;
	}

	public int TotalCost ()
	{
		int cost = 0;
		foreach (KeyValuePair<int,int> item in GetItemToBuyOrSell()) {
			if (item.Value > 0) {
				if (buying) {
					cost += Database.items.FindItem (item.Key).sellValue * item.Value;
				} else {
					cost += Database.items.FindItem (Database.myGuild.inventory.GetInventorySlot (item.Key).itemId).sellValue * item.Value;
				}
			}
		}
		return cost;
	}

	public bool BuyingOrSellingSomething ()
	{
		if (GetItemToBuyOrSell ().Count > 0) {
			return true;
		} else 
			return false;
	}

	public void UpdateSlotButtons ()
	{

		foreach (GameObject slot in prefabList) {
			if (slot.activeSelf == true) {
				SlotInfo slotinfo = slot.GetComponent<SlotInfo> ();
				bool cannotAdd;
				if (buying) {
					cannotAdd = TotalQuantity () >= maxBuy || TotalCost () + slotinfo.cost > guildMoney || !Database.myGuild.inventory.CanAddThisItem (slot.GetComponent<SlotInfo> ().id, new List<int> (GetItemToBuyOrSell ().Keys));
				} else {
					cannotAdd = TotalQuantity () >= maxBuy || int.Parse (slotinfo.slotQuantity.text) >= Database.myGuild.inventory.GetInventorySlot (slotinfo.inventorySlotId).quantity;
				}
				if (cannotAdd) {
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
			{
				if (shop == null || !shop.useSkill) {
					foreach (KeyValuePair<string,Text> stat in stats) {
						stat.Value.text = "  ";
					}
					itemDescription.text = "";
					if (slot != null) {
						Item item;
						if (buying) {
							item = Database.items.FindItem (slot.id);
						} else {
							item = Database.items.FindItem (Database.myGuild.inventory.GetInventorySlot (slot.inventorySlotId).itemId);
						}
						itemDescription.text = item.description;
						if (itemDescription.text != "")
							itemDescription.text += " ";
						if (item.subType == "Heal") {
							itemDescription.text += string.Format (Database.strings.GetString ("Heal"), item.GetStatString ());
						}
						for (int i=0; i<prefabList.Count; i++) {
							if (prefabList [i].GetComponent<SlotInfo> () == slot) {
								if (item.stats != null) {
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
						stats ["Weight"].text = item.weight.ToString ();
					}
				} else if (slot != null) {
					if (slot.isAbility) {
						itemDescription.text = string.Format (Database.strings.GetString ("Ability"), slot.slotName.text);
					}
				}
			}
			if (lastSelected != null) {
				if (lastSelected.selected) {
					lastSelected.Select ();
				}
			}
			if (slot != null) {
				slot.Select ();
			}
			lastSelected = slot;
		}
		UpdateSlotButtons ();
	}
}
