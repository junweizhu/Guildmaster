using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WarehouseScreenDisplay : MonoBehaviour {
	public GameObject itemslotPrefab;
	public GameObject itemslotList;
	private List<GameObject> slotPrefabList=new List<GameObject>();
	private int itemCount;

	public void UpdateText(Inventory itemlist)
	{
		for(int i=0; i<itemlist.inventory.Count;i++)
		{
			if(itemlist.inventory[i].itemName==null)
			{
				if (slotPrefabList.Count>(i+1))
				{
					slotPrefabList[i].SetActive(false);
				}
				itemCount=i;
				break;
			}
			if (slotPrefabList.Count<(i+1))
			{
				slotPrefabList.Add (GameObject.Instantiate(itemslotPrefab) as GameObject);
				slotPrefabList[i].transform.SetParent(itemslotList.transform);
				slotPrefabList[i].GetComponent<SlotInfo>().ResetTransform();
				slotPrefabList[i].name = "Item " + i.ToString ();
			}
			if (slotPrefabList[i].activeSelf==false)
			{
				slotPrefabList[i].SetActive(true);
			}
			slotPrefabList[i].GetComponent<SlotInfo>().FillSlotWithItem(i+1,itemlist.inventory[i],itemlist.itemQuality[i],itemlist.itemQuantity[i]);
		}
		itemslotList.GetComponent<RectTransform>().sizeDelta= new Vector2(0,itemCount*45);
		if (itemCount>22)
			itemslotList.GetComponent<ScrollRect>().vertical=true;
		else
			itemslotList.GetComponent<ScrollRect>().vertical=false;
	}
}
