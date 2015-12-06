using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MemberScreenDisplay : MonoBehaviour {
	public GameObject memberslotPrefab;
	public GameObject memberslotList;
	private List<GameObject> slotPrefabList=new List<GameObject>();
	
	public void UpdateText(List<Member> memberlist)
	{
		for(int i=0; i<memberlist.Count;i++)
		{
			if (slotPrefabList.Count<(i+1))
			{
				slotPrefabList.Add (GameObject.Instantiate(memberslotPrefab) as GameObject);
				slotPrefabList[i].transform.SetParent(memberslotList.transform);
				slotPrefabList[i].GetComponent<SlotInfo>().ResetTransform();
				slotPrefabList[i].name = "Member " + i.ToString ();
			}
			else if(slotPrefabList[i].activeSelf==false){
				slotPrefabList[i].SetActive(true);
			}
			slotPrefabList[i].GetComponent<SlotInfo>().FillSlotWithMember(memberlist[i]);
		}
		if (memberlist.Count<slotPrefabList.Count)
		{
			for (int i=memberlist.Count; i<slotPrefabList.Count;i++)
			{
				slotPrefabList[i].SetActive(false);
			}
		}
		memberslotList.GetComponent<RectTransform>().sizeDelta= new Vector2(0,memberlist.Count*45);
		if (memberlist.Count>22)
			memberslotList.GetComponent<ScrollRect>().vertical=true;
		else
			memberslotList.GetComponent<ScrollRect>().vertical=false;
	}
}
