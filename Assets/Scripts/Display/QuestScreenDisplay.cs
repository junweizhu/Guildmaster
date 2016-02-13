using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class QuestScreenDisplay : MonoBehaviour {
	public GameObject questslotPrefab;
	public Transform questslotList;
	public List<GameObject> slotPrefabList=new List<GameObject>();
	public int questCount;

	public void UpdateText(List<Quest> questlist)
	{
		questCount=questlist.Count;
		for(int i=0; i<questlist.Count;i++)
		{
			if(questlist[i].name==null)
			{
				if (slotPrefabList.Count>(i+1))
				{
					slotPrefabList[i].SetActive(false);
				}
				questCount=i;
				break;
			}
			slotPrefabList.GeneratePrefab(i,questslotPrefab,questslotPrefab.name,questslotList);
			slotPrefabList[i].GetComponent<SlotInfo>().FillSlotWithQuest(i+1,questlist[i]);
		}
		if(questCount<slotPrefabList.Count)
		{
			for (int i=questCount;i<slotPrefabList.Count;i++)
			{
				slotPrefabList[i].SetActive(false);
			}
		}
		questslotList.transform.SetSize(questCount,64);
	}
}
