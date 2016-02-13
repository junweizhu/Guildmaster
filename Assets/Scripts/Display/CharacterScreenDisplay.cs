using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterScreenDisplay : MonoBehaviour {
	public GameObject memberslotPrefab;
	public Transform memberslotList;
	private List<GameObject> slotPrefabList=new List<GameObject>();
	
	public void UpdateText(List<Character> memberlist)
	{
		for(int i=0; i<memberlist.Count;i++)
		{

			slotPrefabList.GeneratePrefab(i,memberslotPrefab,memberslotPrefab.name,memberslotList);
			slotPrefabList[i].GetComponent<SlotInfo>().FillSlotWithCharacter(memberlist[i]);
		}
		if (memberlist.Count<slotPrefabList.Count)
		{
			for (int i=memberlist.Count; i<slotPrefabList.Count;i++)
			{
				slotPrefabList[i].SetActive(false);
			}
		}
		memberslotList.SetSize(memberlist.Count,64);
	}
}
