using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ItemSelectScreenDisplay: MonoBehaviour
{
	public GameObject selectPrefab;
	public List<GameObject> prefabList = new List<GameObject> ();
	public Text dialogue;
	public Transform selectList;
	public ScrollRect scrollRect;
	public bool show = false;
	public int maxSelection;
	List<InventorySlot> items;
	public SlotInfo selectedItem;
	public int slotId;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (show) {
			GetComponent<CanvasGroup> ().alpha = 1;
			GetComponent<CanvasGroup> ().blocksRaycasts = true;
			//UpdateSlotButtons ();

		} else {
			GetComponent<CanvasGroup> ().alpha = 0;
			GetComponent<CanvasGroup> ().blocksRaycasts = false;
		}

	}

	public void UpdateText (Inventory storage, string description, int equipslotid)
	{
		slotId=equipslotid;
		if(slotId==0){
			items = storage.GetAllItems ("Weapon");
		} else if (slotId==1){
			items = storage.GetAllItems ("Armor");
		} else {
			items = storage.GetAllItems ("Accessory");
			items.AddRange(storage.GetAllItems ("Consumable"));
		}
		items.Add (new InventorySlot(999));
		dialogue.text = description;
		selectedItem=null;
		for (int i=0; i<items.Count; i++) {
			prefabList.GeneratePrefab (i, selectPrefab, "Item", selectList);
			prefabList [i].GetComponent<SlotInfo> ().FillSlotWithItem (items [i]);
			prefabList [i].GetComponent<SlotInfo> ().ResetSelection ();
		}
		if (items.Count< prefabList.Count) {
			for (int i=items.Count; i<prefabList.Count; i++) {
				prefabList [i].SetActive (false);
			}
		}

		selectList.SetSize (scrollRect, items.Count, 64);
	}

/*	public void UpdateSlotButtons ()
	{
		
		for (int i=0; i<prefabList.Count; i++) {
			if (prefabList [i].activeSelf == true) {
				if (TotalQuantity () >= maxSelection || prefabList [i].GetComponent<SlotInfo> ().GetItemQuantity () >= items [i].quantity) {
					prefabList [i].GetComponent<SlotInfo> ().slotAdd.interactable = false;
				} else {
					prefabList [i].GetComponent<SlotInfo> ().slotAdd.interactable = true;
				}
				if (prefabList [i].GetComponent<SlotInfo> ().GetItemQuantity () == 0) {
					prefabList [i].GetComponent<SlotInfo> ().slotRed.interactable = false;
				} else {
					prefabList [i].GetComponent<SlotInfo> ().slotRed.interactable = true;
				}
			}else{
				break;
			}

		}
	}

	public int TotalQuantity ()
	{
		int quantity = 0;
		foreach (KeyValuePair<int,int> item in GetItemToTake()) {
			if (item.Value > 0) {
				quantity += item.Value;
			}
		}
		return quantity;
	}

	public Dictionary<int,int> GetItemToTake ()
	{
		Dictionary<int,int> itemList = new Dictionary<int, int> ();
		foreach (GameObject item in prefabList) {
			if (item.activeSelf == true) {
				if (item.GetComponent<SlotInfo> ().GetItemQuantity () > 0) {
					itemList [item.GetComponent<SlotInfo> ().id] = item.GetComponent<SlotInfo> ().GetItemQuantity ();
				}
			}
			
		}
		return itemList;
	}*/

	public void SelectItem (SlotInfo slot)
	{
		selectedItem=slot;
		GameObject.FindObjectOfType<GameManager>().SelectItem();
	}
	public void CloseScreen ()
	{
		show = false;
	}

}
