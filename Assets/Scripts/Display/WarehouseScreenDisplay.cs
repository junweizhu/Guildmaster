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
	private SlotInfo lastSelected;

	public void Start ()
	{
	}

	public void UpdateText (Inventory itemlist)
	{
		for (int i=0; i<itemlist.inventory.Count; i++) {
			if (!itemlist.inventory [i].filled) {
				if (slotPrefabList.Count > (i + 1)) {
					slotPrefabList [i].SetActive (false);
				}
				itemCount = i;
				break;
			}
			slotPrefabList.GeneratePrefab(i,itemslotPrefab,"Item",itemslotList);
			slotPrefabList [i].GetComponent<SlotInfo> ().FillSlotWithItem (i + 1, itemlist.inventory [i]);
			slotPrefabList [i].GetComponent<SlotInfo> ().ResetSelection();
		}
		if(itemCount<slotPrefabList.Count)
		{
			for (int i=itemCount;i<slotPrefabList.Count;i++)
			{
				slotPrefabList[i].SetActive(false);
			}
		}
		itemslotList.SetSize(itemCount,64);
		itemStatScreen.FillSlot (null);
		if (lastSelected != null) {
			lastSelected = null;
		}
	}

	public void DisplayItemStats (SlotInfo itemslot)
	{
		if(lastSelected!=itemslot) {
			itemslot.Select ();
			InventorySlot slot=GameObject.FindObjectOfType<GameManager>().myGuild.inventory.GetInventorySlot(itemslot.id);
			itemStatScreen.FillSlot (Database.items.FindItem (slot.itemId));
			if (lastSelected != null) {
				lastSelected.Select ();
			}
			lastSelected = itemslot;
		}
	}
}
