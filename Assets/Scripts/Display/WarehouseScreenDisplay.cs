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
	public Text storageSize;
	public Button deleteButton;
	private bool refresh;
	private CanvasGroup canvasGroup;
	void Start(){
		canvasGroup=GetComponent<CanvasGroup>();
	}
	void Update(){
		if(canvasGroup.alpha!=1){
			refresh=true;
		} else if(refresh){
			refresh=false;
			UpdateText();
		}
	}

	public void UpdateText ()
	{

		Guild guild=Database.myGuild;
		Inventory itemlist=guild.inventory;
		List<int> slotId=guild.inventory.GetAllFilledSlotId();
		itemCount=slotId.Count;
		int count=itemCount;
		if (itemCount<slotPrefabList.Count){
			count=slotPrefabList.Count;
		}
		for (int i=0; i<count;i++){
			slotPrefabList.GeneratePrefab(i,itemslotPrefab,"Item",itemslotList);
			if (i<itemCount){
				slotPrefabList [i].GetComponent<SlotInfo> ().FillSlotWithItem (i + 1, itemlist.GetInventorySlot(slotId [i]));
				slotPrefabList [i].GetComponent<SlotInfo> ().ResetSelection();
			} else{
				slotPrefabList[i].SetActive(false);
			}
		}
		itemslotList.SetSize(itemCount,64);
		itemStatScreen.FillSlot (null);
		if (lastSelected != null) {
			lastSelected = null;
		}
		storageSize.text=slotId.Count.ToString()+"/"+Database.upgrades.GetUpgrade(1).MaxSize(guild.upgradelist[1]);
		deleteButton.interactable=(lastSelected!=null);
	}

	public void DisplayItemStats (SlotInfo itemslot)
	{
		if(lastSelected!=itemslot) {
			itemslot.Select ();
			InventorySlot slot=Database.myGuild.inventory.GetInventorySlot(itemslot.inventorySlotId);
			itemStatScreen.FillSlot (Database.items.FindItem (slot.itemId));
			if (lastSelected != null) {
				lastSelected.Select ();
			}
			lastSelected = itemslot;
			deleteButton.interactable=(lastSelected!=null);
		}

	}

	public void DeleteItem(){
		Database.myGuild.inventory.GetInventorySlot(lastSelected.id).EmptyItem();
		UpdateText();
	}
}
