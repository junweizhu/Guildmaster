using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TaskScreenDisplay : MonoBehaviour {
	public GameObject taskslotPrefab;
	public Transform taskslotList;
	public List<GameObject> slotPrefabList = new List<GameObject> ();
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
		
		Guild guild = Database.myGuild;
		List<Task> tasklist=guild.taskList;
		int count=tasklist.Count;
		if (tasklist.Count<slotPrefabList.Count){
			count=slotPrefabList.Count;
		}
		for(int i=0; i<count;i++)
		{
			slotPrefabList.GeneratePrefab(i,taskslotPrefab,taskslotPrefab.name,taskslotList);
			if (i<tasklist.Count){
				slotPrefabList[i].GetComponent<SlotInfo>().FillSlotWithTask(i+1,tasklist[i]);
			} else{
				slotPrefabList[i].SetActive(false);
			}
		}
		taskslotList.SetSize(tasklist.Count,144);
	}
}
