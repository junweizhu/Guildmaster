using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MemberScreenDisplay : MonoBehaviour {
	public GameObject memberslotPrefab;
	public Transform memberslotList;
	private List<GameObject> slotPrefabList=new List<GameObject>();
	
	public void UpdateText(List<Member> memberlist)
	{
		for(int i=0; i<memberlist.Count;i++)
		{
			slotPrefabList.GeneratePrefab(i,memberslotPrefab,"Member",memberslotList);
			slotPrefabList[i].GetComponent<SlotInfo>().FillSlotWithMember(memberlist[i]);
		}
		if (memberlist.Count<slotPrefabList.Count)
		{
			for (int i=memberlist.Count; i<slotPrefabList.Count;i++)
			{
				slotPrefabList[i].SetActive(false);
			}
		}
		memberslotList.SetSize(memberlist.Count,48);
	}
}
