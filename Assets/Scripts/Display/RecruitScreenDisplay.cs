using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RecruitScreenDisplay : MonoBehaviour {
	public GameObject recruitslotPrefab;
	public GameObject recruitslotList;
	private List<GameObject> slotPrefabList=new List<GameObject>();
	private int recruitCount;
	// Use this for initialization
	void Start () {
	
	}
	
	public void UpdateText(List<Member> recruits)
	{
		for(int i=0; i<recruits.Count;i++)
		{
			if (slotPrefabList.Count<(i+1))
			{
				slotPrefabList.Add (GameObject.Instantiate(recruitslotPrefab) as GameObject);
				slotPrefabList[i].transform.SetParent(recruitslotList.transform);
				slotPrefabList[i].GetComponent<SlotInfo>().ResetTransform();
				slotPrefabList[i].name = "Recruit " + i.ToString ();
			}
			else if(slotPrefabList[i].activeSelf==false){
				slotPrefabList[i].SetActive(true);
			}
			slotPrefabList[i].GetComponent<SlotInfo>().FillSlotWithMember(recruits[i],i+1);
		}
		if (recruits.Count<slotPrefabList.Count)
		{
			for (int i=recruits.Count; i<slotPrefabList.Count;i++)
			{
				slotPrefabList[i].SetActive(false);
			}
		}
		recruitslotList.GetComponent<RectTransform>().sizeDelta= new Vector2(0,recruits.Count*45);
		if (recruits.Count>22)
			recruitslotList.GetComponent<ScrollRect>().vertical=true;
		else
			recruitslotList.GetComponent<ScrollRect>().vertical=false;
	}
}
