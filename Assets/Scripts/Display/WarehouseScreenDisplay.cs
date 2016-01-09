using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WarehouseScreenDisplay : MonoBehaviour
{
	public GameObject itemslotPrefab;
	public Transform itemslotList;
	private List<GameObject> slotPrefabList = new List<GameObject> ();
	private int itemCount;
	public ItemStatScreenDisplay itemStatScreen;
	private ItemDatabase idb;
	private SlotInfo lastSelected;

	public void Start ()
	{
		idb = GameObject.FindObjectOfType<ItemDatabase> ();
	}

	public void UpdateText (Inventory itemlist)
	{
		for (int i=0; i<itemlist.inventory.Count; i++) {
			if (itemlist.inventory [i].item.name == null) {
				if (slotPrefabList.Count > (i + 1)) {
					slotPrefabList [i].SetActive (false);
				}
				itemCount = i;
				break;
			}
			slotPrefabList.GeneratePrefab(i,itemslotPrefab,"Item",itemslotList);
			slotPrefabList [i].GetComponent<SlotInfo> ().FillSlotWithItem (i + 1, itemlist.inventory [i].item, itemlist.inventory [i].quality, itemlist.inventory [i].quantity);
			slotPrefabList [i].GetComponent<SlotInfo> ().ResetSelection();
		}
		itemslotList.SetSize(itemCount,48);
		itemStatScreen.FillSlot (null);
		if (lastSelected != null) {
			lastSelected = null;
		}
	}

	public void DisplayItemStats (SlotInfo itemslot)
	{
		if(lastSelected!=itemslot) {
			itemslot.Select ();
			itemStatScreen.FillSlot (idb.FindItem (itemslot.id));
			if (lastSelected != null) {
				lastSelected.Select ();
			}
			lastSelected = itemslot;
		}
	}
}
